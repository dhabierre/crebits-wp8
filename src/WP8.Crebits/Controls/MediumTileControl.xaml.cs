
namespace WP8.Crebits.Controls
{
    using Microsoft.Phone.Shell;

    using Cimbalino.Phone.Toolkit.Extensions; // SavePng

    using System;
    using System.Diagnostics;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using WP8.Crebits.Helpers;
    using WP8.Toolkit.Helpers;

    public partial class MediumTileControl : UserControl
    {
        #region [ Constants ]

        private const int MEDIUM_TILE_HEIGHT = 336;
        private const int MEDIUM_TILE_WIDTH = 336;
        private const int MEDIUM_TILE_QUALITY = 90;

        private const string shellContentPath = "Shared/ShellContent";

        #endregion

        #region [ Constructor ]

        public MediumTileControl(string backgroundImagePath, string text, bool hasData)
        {
            this.InitializeComponent();

            this.BackgroundImagePath = backgroundImagePath;
            this.Text = text;
            this.HasData = hasData;
        }

        #endregion

        #region [ Properties ]

        public string BackgroundImagePath { get; private set; }

        public bool HasData { get; private set; }

        public string Text { get; private set; }

        #endregion

        #region [ Methods ]

        public void Update()
        {
            var tile = ShellTile.ActiveTiles.FirstOrDefault();
            if (tile != null)
            {
                var backImage = new BitmapImage(new Uri(this.BackgroundImagePath, UriKind.RelativeOrAbsolute));
                backImage.CreateOptions = BitmapCreateOptions.None;

                backImage.ImageOpened += (s, args) =>
                {
                    try
                    {
                        this.LayoutRoot.Background = this.GetBackgroundSolidColorBrush();

                        this.BackgroundImage.Source = backImage;
                        this.TextOverlayTextBox.Text = this.Text;

                        this.UpdateLayout();
                        this.Measure(new Size(MEDIUM_TILE_WIDTH, MEDIUM_TILE_HEIGHT));
                        this.UpdateLayout();
                        this.Arrange(new Rect(0, 0, MEDIUM_TILE_WIDTH, MEDIUM_TILE_HEIGHT));

                        var wBitmap = new WriteableBitmap(MEDIUM_TILE_WIDTH, MEDIUM_TILE_HEIGHT);

                        wBitmap.Render(this.LayoutRoot, null);
                        wBitmap.Invalidate();

                        string fileName = string.Format("MediumTile_{0}.jpg", Guid.NewGuid());

                        var store = IsolatedStorageFile.GetUserStoreForApplication();

                        if (!store.DirectoryExists(shellContentPath))
                            store.CreateDirectory(shellContentPath);

                        using (var fileStream = store.CreateFile(string.Format("{0}/{1}", shellContentPath, fileName)))
                        {
                            //wBitmap.SaveJpeg(fileStream, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, MEDIUM_TILE_QUALITY);
                            wBitmap.SavePng(fileStream);
                        }

                        var filesTodelete = from f in store.GetFileNames(string.Format("{0}/MediumTile_*", shellContentPath))
                                            where !f.EndsWith(fileName)
                                            select f;

                        foreach (var file in filesTodelete)
                        {
                            store.DeleteFile(string.Format("{0}/{1}", shellContentPath, file));
                        }

                        var imageUri = new Uri(string.Format("isostore:/{0}", string.Format("{0}/{1}", shellContentPath, fileName)), UriKind.RelativeOrAbsolute);

                        var tileData = new FlipTileData
                        {
                            BackgroundImage = imageUri, // WideBackgroundImage
                            Title = AppHelper.ApplicationTitle
                        };

                        tile.Update(tileData);
                    }
                    catch (Exception)
                    {
                        if (Debugger.IsAttached)
                            Debugger.Break();
                    }
                };
            }
        }

        private SolidColorBrush GetBackgroundSolidColorBrush()
        {
            //var brush = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]); // Current theme color
            var brush = new SolidColorBrush(Colors.Transparent); // Transparent by default

            if (this.HasData)
            {
                int? minCashLimitValue = SettingsHelper.GetMinCashLimitValue();
                if (minCashLimitValue != null)
                {
                    double cash = Convert.ToDouble(this.Text);
                    if (cash <= minCashLimitValue)
                    {
                        switch (SettingsHelper.GetMinCashLimitColor())
                        {
                            case "Red":
                                brush = new SolidColorBrush(Colors.Red);
                                break;
                            case "Yellow":
                                brush = new SolidColorBrush(Colors.Yellow);
                                break;
                            default: // Orange
                                brush = new SolidColorBrush(Colors.Orange);
                                break;
                        }
                    }
                }
            }

            return brush;
        }

        #endregion
    }
}


namespace WP8.Crebits.Controls
{
	using GoogleAds;

	using Microsoft.Phone.Tasks;

	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	public partial class AdControlEx : UserControl
	{
		#region [ Constructor ]

		public AdControlEx()
		{
			InitializeComponent();

			AdView adView = new AdView
			{
				Height = 75,
				Width = 480,
				Format = AdFormats.SmartBanner,
				AdUnitID = "ca-app-pub-9047384572277809/1255923390",
			};

			adView.ReceivedAd += OnAdReceived;
			adView.FailedToReceiveAd += OnAdFailedToReceive;

			Grid.SetRow(adView, 0);

			this.AdsGrid.Children.Add(adView);
		}

		#endregion

		#region [ Events ]

		private void TrialBorder_Tap(object sender, GestureEventArgs e)
		{
			MarketplaceDetailTask marketPlace = new MarketplaceDetailTask();
			marketPlace.Show();
		}

		private void PubBorder_Tap(object sender, GestureEventArgs e)
		{
			WebBrowserTask webBrowserTask = new WebBrowserTask
			{
				Uri = new Uri("http://www.layercake-generator.net/?source=crebits-wp8", UriKind.Absolute)
			};

			webBrowserTask.Show();
		}

		private void OnAdReceived(object sender, AdEventArgs e)
		{
			this.AdsGrid.Visibility = Visibility.Visible;
		}

		private void OnAdFailedToReceive(object sender, GoogleAds.AdErrorEventArgs e)
		{
			this.AdsGrid.Visibility = Visibility.Collapsed;
		}

		#endregion
	}
}

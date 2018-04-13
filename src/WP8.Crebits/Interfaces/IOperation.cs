
namespace WP8.Crebits
{
    using System;

    public interface IOperation
    {
        int Id { get; set; }

        DateTime Date { get; set; }

        string Caption { get; set; }

        double Value { get; set; }

        double? OverrideValue { get; set; }

        bool IsMonthly { get; set; }

        bool IsDisabled { get; set; }

        double CurrentValue { get; }
    }
}

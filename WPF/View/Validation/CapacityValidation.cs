using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.WPF.View.Validation
{
    public class CapacityValidation : ValidationRule
    {
        public int Minimum { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var m = (string)value;
                int result;
                if (int.TryParse(m, out result))
                {
                    if (result < Minimum)
                    {
                     return new ValidationResult(false, "Min number is " + Minimum);
                    }
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Value must be number");
            }
            catch
            {
                return new ValidationResult(false, "Unkown error occured");
            }
        }
    }
    public class DurationValidation : ValidationRule
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var m = (string)value;
                double result;
                if (double.TryParse(m, out result))
                {
                    if (result < Minimum) { return new ValidationResult(false, "Min duration is " + Minimum); }
                    if (result > Maximum) { return new ValidationResult(false, "Max duration is " + Maximum); }
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Value must be number");
            }
            catch
            {
                return new ValidationResult(false, "Unkown error occured");
            }
        }
    }
    public class HoursMinutesValidation : ValidationRule 
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string inputString = (string)value;
                if (string.IsNullOrEmpty(inputString))
                {
                    return new ValidationResult(false, "Cannot be empty.");
                }
                TimeSpan timeSpan;
                if (TimeSpan.TryParseExact(inputString, "hh\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out timeSpan))
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "Format is HH:mm.");
                }
            }
            catch
            {
                return new ValidationResult(false, "Unkown error occured");
            }
        }
    }

}

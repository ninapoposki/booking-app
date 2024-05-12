using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.WPF.View.Owner
{

    public class ConverterStarsOwner : IValueConverter
    {
        //adasadsdad
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            double grade = (double)value;
                List<string> stars = new List<string>();

                
                int fullStars = (int)grade;
                for (int i = 0; i < fullStars; i++)
                {
                    stars.Add(@"..\..\..\Resources\Icons\Owner\icon_fullstar.png");
                }

                if (grade % 1 != 0)
                {
                    stars.Add(@"..\..\..\Resources\Icons\Owner\icon_halfstar.png");
                }

                int emptyStars = 5 - fullStars - (grade % 1 != 0 ? 1 : 0);
                for (int i = 0; i < emptyStars; i++)
                {
                    stars.Add(@"..\..\..\Resources\Icons\Owner\icon_emptystar.png");
                }

                return stars;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
}

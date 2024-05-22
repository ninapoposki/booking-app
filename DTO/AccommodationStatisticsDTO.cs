using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.DTO
{
    public class AccommodationStatisticsDTO :INotifyPropertyChanged
    {
        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (value != year)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }
        private string monthName;
        public string MonthName
        {
            get { return monthName; }
            set
            {
                if (value != monthName)
                {
                    monthName = value;
                    OnPropertyChanged("MonthName");
                }
            }
        }
        private int madeReservations;
        public int MadeReservations
        {
            get { return madeReservations; }
            set
            {
                if (value != madeReservations)
                {
                    madeReservations = value;
                    OnPropertyChanged("MadeReservations");
                }
            }
        }
        private int cancelledReservations;
        public int CancelledReservations
        {
            get { return cancelledReservations; }
            set
            {
                if (value != cancelledReservations)
                {
                    cancelledReservations = value;
                    OnPropertyChanged("CancelledReservations");
                }
            }
        }
        private int postponedReservations;
        public int PostponedReservations
        {
            get { return postponedReservations; }
            set
            {
                if (value != postponedReservations)
                {
                    postponedReservations = value;
                    OnPropertyChanged("PostponedReservations");
                }
            }
        }
        private int recommendedReservations;
        public int RecommendedReservations
        {
            get { return recommendedReservations; }
            set
            {
                if (value != recommendedReservations)
                {
                    recommendedReservations = value;
                    OnPropertyChanged("RecommendedReservations");
                }
            }
        }
        private int accommId;
        public int AccomId
        {
            get { return accommId; }
            set
            {
                if (value != accommId)
                {
                    accommId = value;
                    OnPropertyChanged("AccomId");
                }
            }
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
}

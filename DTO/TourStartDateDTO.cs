using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourStartDateDTO: INotifyPropertyChanged
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        private DateTime startTime;
        public DateTime StartTime
        {

            get { return startTime; }



            set
            {

                if (value != startTime)
                {

                    startTime = value;
                    OnPropertyChanged("StartTime");


                }

            }
        }

        public TourStartDateDTO() { }

        public TourStartDateDTO(TourStartDate tourStartDate)
        {

            Id = tourStartDate.Id;
          TourId = tourStartDate.TourId;
            StartTime = tourStartDate.StartTime;


        }

        public TourStartDate ToTourStartDate()
        {
            return new TourStartDate(Id,TourId,startTime);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;


    }
}


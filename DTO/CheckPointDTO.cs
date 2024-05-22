using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class CheckPointDTO
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get { return name; }

            set
            {

                if (value != name)
                {


                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public int TourId { get; set; }


        private string type;
        public string Type
        {

            get { return type; }
            set
            {



                if (value != type)
                {


                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        private bool isCurrent;

        public bool IsCurrent
        {
            get { return isCurrent; }
            set
            {
                if (isCurrent != value)
                {
                    isCurrent = value;
                    OnPropertyChanged("IsCurrent");  
                }
            }
        }
        public CheckPoint ToCheckPoint()
        {
            return new CheckPoint(Id,name,TourId,type);
        }

        public CheckPointDTO() { }


        public CheckPointDTO(CheckPoint checkPoint)
        {
           Id = checkPoint.Id;
           Name = checkPoint.Name;
           TourId = checkPoint.TourId;
           Type = checkPoint.Type;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

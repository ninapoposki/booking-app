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
    public class VoucherDTO: INotifyPropertyChanged
    {
        public int Id { get; set; }

        public int TourReservationId { get; set; }

        private string startDate;
        public string StartDate
        {

            get { return startDate; }


            set
            {

                if (value != startDate)
                {

                    startDate = value;
                    OnPropertyChanged("StartDate");


                }

            }
        }
        private string expirationDate;
        public string ExpirationDate
        {

            get { return expirationDate; }


            set
            {

                if (value != expirationDate)
                {

                    expirationDate = value;
                    OnPropertyChanged("ExpirationDate");


                }

            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }

            }
        }

        private Status status;
        public Status Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        public VoucherDTO() { }

        public VoucherDTO(Voucher voucher)
        {
            Id=voucher.Id;
            TourReservationId = voucher.TourReservationId;
            StartDate = voucher.StartDate.ToString("dd/MM/yyyy");
            ExpirationDate = voucher.ExpirationDate.ToString("dd/MM/yyyy");
            Description = voucher.Description;
            Status= voucher.Status;
        }
        public Voucher ToVoucher()
        {
            return new Voucher(Id, TourReservationId, DateOnly.ParseExact(startDate, "dd/MM/yyyy"), DateOnly.ParseExact(expirationDate, "dd/MM/yyyy"), description, status);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}

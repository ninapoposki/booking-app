using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BookingApp.Domain.Model
{
    public enum Status { USED, VALID, INVALID};
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public int TourReservationId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

        public Voucher() { }

        public Voucher(int id, int tourReservationId, DateOnly startDate, DateOnly expirationDate, string description, Status status)
        {
            Id = id;
            TourReservationId = tourReservationId;
            StartDate = startDate;
            ExpirationDate = expirationDate;
            Description = description;
            Status = status;
        }
        public Voucher(int tourReservationId, DateOnly startDate, DateOnly expirationDate, string description, Status status)
        {
            TourReservationId = tourReservationId;
            StartDate = startDate;
            ExpirationDate = expirationDate;
            Description = description;
            Status = status;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourReservationId = Convert.ToInt32(values[1]);
            StartDate = DateOnly.ParseExact(values[2], "dd/MM/yyyy");
            ExpirationDate = DateOnly.ParseExact(values[3], "dd/MM/yyyy");
            Description = values[4];
            if (values[5] == "USED"){
                Status = Status.USED;
            }
            else if (values[5] =="VALID") {
                Status = Status.VALID;
            }
            else{
                Status = Status.INVALID;
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourReservationId.ToString(),
                StartDate.ToString("dd/MM/yyyy"),
                ExpirationDate.ToString("dd/MM/yyyy"),
                Description,
                Status.ToString(),
            };
            return csvValues;   
        }
    }
}

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

        public int UserId {  get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

        public Voucher() { }

        public Voucher(int userId,string description)
        {
            UserId = userId;
            Description = description;
            StartDate = DateOnly.FromDateTime(DateTime.Today);
            ExpirationDate= DateOnly.FromDateTime(DateTime.Today.AddMonths(12));
            TourReservationId = -1;//kada napravis novu rez treba da smestis id te rez za ovo polje RECIMO
            Status=Status.VALID;
        }
        public Voucher(int id,int userId ,int tourReservationId, DateOnly startDate, DateOnly expirationDate, string description, Status status)
        {
            Id = id;
            UserId = userId;    
            TourReservationId = tourReservationId;
            StartDate = startDate;
            ExpirationDate = expirationDate;
            Description = description;
            Status = status;
        }
        public Voucher(int userId,int tourReservationId, DateOnly startDate, DateOnly expirationDate, string description, Status status)
        {
            UserId= userId;
            TourReservationId = tourReservationId;
            StartDate = startDate;
            ExpirationDate = expirationDate;
            Description = description;
            Status = status;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId= Convert.ToInt32(values[1]);
            TourReservationId = Convert.ToInt32(values[2]);
            StartDate = DateOnly.ParseExact(values[3], "dd/MM/yyyy");
            ExpirationDate = DateOnly.ParseExact(values[4], "dd/MM/yyyy");
            Description = values[5];
            if (values[6] == "USED"){
                Status = Status.USED;
            }
            else if (values[6] =="VALID") {
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
                UserId.ToString(),
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

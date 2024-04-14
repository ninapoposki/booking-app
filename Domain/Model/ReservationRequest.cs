using System;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public enum RequestStatus { ONHOLD, ACCEPTED, DECLINED };
    public class ReservationRequest : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime NewInitialDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public string Status { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string Comment { get; set; }
        

        public ReservationRequest()
        {
            

        }

        public ReservationRequest(int id, int reservationId, DateTime newInitialDate, DateTime newEndDate, RequestStatus requestStatus, string comment)
        {
            Id = id;
            ReservationId = reservationId;
            NewInitialDate = newInitialDate;
            NewEndDate = newEndDate;
            RequestStatus = requestStatus;
            Comment = comment;
          
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                Id.ToString(),
                ReservationId.ToString(),
                NewInitialDate.ToString(),
                NewEndDate.ToString(),
                RequestStatus.ToString(),
                Comment
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId= int.Parse(values[1]);
            NewInitialDate = DateTime.Parse(values[2]);
            NewEndDate = DateTime.Parse(values[3]);
            if (values[4] == "ONHOLD")
            {
                RequestStatus = RequestStatus.ONHOLD;
            }
            else if (values[4] == "ACCEPTED")
            {
                RequestStatus = RequestStatus.ACCEPTED;
            }
            else
            {
                RequestStatus = RequestStatus.DECLINED;
            }
            Comment = values[5];

        }
    }
}

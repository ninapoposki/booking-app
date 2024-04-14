using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestNotificationsVM:ViewModelBase
    {
        private readonly ReservationRequestService reservationRequestService;
        private readonly ImageService imageService;
        private readonly AccommodationService accommodationService;
        public ObservableCollection<ReservationRequestDTO> AllReservationRequests { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }


        private ReservationRequestDTO _selectedReservationRequest;
        public ReservationRequestDTO SelectedReservationRequest
        {
            get => _selectedReservationRequest;
            set
            {
                if (_selectedReservationRequest != value)
                {
                    _selectedReservationRequest = value;
                    OnPropertyChanged(nameof(SelectedReservationRequest));
                }
            }
        }
        public GuestNotificationsVM()
        {
            AllReservationRequests = new ObservableCollection<ReservationRequestDTO>();
            SelectedReservationRequest = new ReservationRequestDTO();
            reservationRequestService = new ReservationRequestService();
            accommodationService = new AccommodationService();
            imageService = new ImageService();
            Images = new ObservableCollection<ImageDTO>();
            Update();
        }

        public void Update()
        {

            AllReservationRequests.Clear();
            foreach (ReservationRequestDTO reservationRequestDTO in reservationRequestService.GetAll())
            {
                var requestDTO = reservationRequestService.GetOneRequest(reservationRequestDTO);
                AllReservationRequests.Add(requestDTO);
            }
        }
    }
}

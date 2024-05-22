using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AccommodationStatisticsVM : ViewModelBase
    {
        public readonly NavigationService navigationService;
        public CancelledReservationsService cancelledReservationsService { get; set; }
        public AccommodationReservationService accommodationReservationService { get; set; }
        public AccommodationService accommodationService { get; set; }
        public ReservationRequestService reservationRequestService {  get; set; }
        public RenovationRecommendationService recommendationService { get; set; }
        public AccommodationDTO AccommodationDTO { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ObservableCollection<AccommodationStatisticsDTO> Years { get; set; }
        public int BestYear {  get; set; }
        public MyICommand<AccommodationStatisticsDTO> MonthStatistics {  get; private set; }

        public AccommodationStatisticsVM(AccommodationDTO accommodationDTO, NavigationService navigationService)
        {
            cancelledReservationsService = new CancelledReservationsService(Injector.Injector.CreateInstance<ICancelledReservationsRepository>(),
               Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
               Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(),
               Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>(),
               Injector.Injector.CreateInstance<IReservationRequestRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                          Injector.Injector.CreateInstance<IGuestRepository>(),
                          Injector.Injector.CreateInstance<IUserRepository>(),
                          Injector.Injector.CreateInstance<IAccommodationRepository>(),
                          Injector.Injector.CreateInstance<IImageRepository>(),
                          Injector.Injector.CreateInstance<ILocationRepository>(),
                          Injector.Injector.CreateInstance<IOwnerRepository>());
            reservationRequestService = new ReservationRequestService(Injector.Injector.CreateInstance<IReservationRequestRepository>(),
               Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
               Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(),
               Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            recommendationService = new RenovationRecommendationService(Injector.Injector.CreateInstance<IRenovationRecommendationRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            AccommodationDTO = accommodationDTO;
            Images = new ObservableCollection<ImageDTO>();
            this.navigationService = navigationService;
            if (accommodationDTO.Images != null && accommodationDTO.Images.Any())
            {
                Images.Add(accommodationDTO.Images[0]);
            }
            else
            {
                Images[0].Path = "../../../Resources/Icons/Owner/accommodation_placeholder.jpg";
            }

            Years = new ObservableCollection<AccommodationStatisticsDTO>();
            Update();
            BestYear = GetBestYear(accommodationDTO.Id, Years);
            MonthStatistics = new MyICommand<AccommodationStatisticsDTO>(GetMonthStatistics);
           // CalculateOccupancy(AccommodationDTO, 2024);
        }
        public void Update()
        {
            for (int year = 2024; year >= 2022; year--)
            {
                var statistics = MakeStatistics(year, AccommodationDTO.Id);
             
                Years.Add(statistics);
                CalculateOccupancy(AccommodationDTO, year);
            }

        }
        public int GetBestYear(int accommodationId, ObservableCollection<AccommodationStatisticsDTO> Years)
        {
            int BestYear = 2024;
            int max_value = 0;
            foreach (var item in Years)
            {

                int made = CalculateOccupancy(AccommodationDTO, item.Year);
                if (made> max_value)
                {
                    max_value = made;
                    BestYear = item.Year;
                }
            }
            
            return BestYear;
        }
    

        public AccommodationStatisticsDTO MakeStatistics(int years, int accommodationId)
        {
            
            List<CancelledReservationsDTO> cancellationRes = cancelledReservationsService.GetAll();
            int cancelled = cancellationRes.Count(reservation => reservation.InitialDate.Year == years && reservation.AccommodationId == accommodationId);
            int made = accommodationReservationService.GetAll().Count(reservation => reservation.InitialDate.Year == years && reservation.AccommodationId == accommodationId);
              List<RenovationRecommendationDTO> Renrecommended = recommendationService.GetAll();
            int recommended =GetRecommendedNumber(accommodationId, years);
            int postponed = GetPostponedNumber(accommodationId, years);
            return new AccommodationStatisticsDTO() { CancelledReservations = cancelled, MadeReservations = made, PostponedReservations =postponed, RecommendedReservations = recommended, Year= years, AccomId = accommodationId };
        }
        public int GetRecommendedNumber(int accommodationId, int years)
        {
            int recommended = 0;
            List<RenovationRecommendationDTO> Renrecommended = recommendationService.GetAll();
            foreach (var recommendation in Renrecommended)
            {
                int id = accommodationReservationService.GetById(recommendation.ReservationId).AccommodationId;
                AccommodationDTO accommodationDTO = accommodationService.GetByIdDTO(id);
                AccommodationReservationDTO accommodationReservationDTO = accommodationReservationService.GetByIdDTO(recommendation.ReservationId);
                if (accommodationId == id && accommodationReservationDTO.InitialDate.Year == years)
                {
                    recommended++;
                }
               
            }
             return recommended;
        }
        public int GetPostponedNumber(int accommodationId, int years)
        {
            int postponed = 0;    
            List<ReservationRequestDTO> requested = reservationRequestService.GetAll();
            foreach (var req in requested)
            {
                int id = accommodationReservationService.GetById(req.ReservationId).AccommodationId;
                AccommodationDTO accommodationDTO = accommodationService.GetByIdDTO(id);
                AccommodationReservationDTO accommodationReservationDTO = accommodationReservationService.GetByIdDTO(req.ReservationId);
                if(accommodationId == id &&  req.RequestStatus == Domain.Model.RequestStatus.ACCEPTED && accommodationReservationDTO.InitialDate.Year == years ) {
                    postponed++;
                }
            }

            return postponed;

        }

        public void GetMonthStatistics(AccommodationStatisticsDTO accommodationStatisticsDTO)
        {
            navigationService.Navigate(new MonthStatistics(navigationService, accommodationStatisticsDTO));
        }

      
        public int CalculateOccupancy(AccommodationDTO accommodationDTO, int years)
        {
            int countedDays = 0;
            int totalDays = 365;
            var Reservations = accommodationReservationService.GetAll();

            foreach (var reservation in Reservations)
            {
                if (reservation.AccommodationId == accommodationDTO.Id &&
                    reservation.InitialDate.Year == years   )
                {
                   
                    for (DateTime date = reservation.InitialDate; date <= reservation.EndDate; date = date.AddDays(1))
                    {
                        countedDays++;
                    }
                }
            }

            int Occupancy = (int)(100 * (double)countedDays / (double)totalDays);
            return Occupancy;
        }
    
        public int Occupancy { get; set; }


    }
}

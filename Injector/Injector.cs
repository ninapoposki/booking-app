using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepositories;


namespace BookingApp.Injector
{

    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(IGuestGradeRepository), new GuestGradeRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IOwnerRepository), new OwnerRepository() },
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IImageRepository), new ImageRepository() },
            { typeof(ITourRepository), new TourRepository()},
            { typeof(IVoucherRepository), new VoucherRepository()},
            { typeof(ITourStartDateRepository), new TourStartDateRepository()},
            { typeof(ITourReservationRepository), new TourReservationRepository()},
            {  typeof(ITourGuestRepository), new TourGuestRepository()},
            {  typeof(ILanguageRepository), new LanguageRepository()},
            {  typeof(ICheckPointRepository), new CheckPointRepository()},
            {  typeof(IAccommodationGradeRepository), new AccommodationGradeRepository()},
            {  typeof(IAccommodationReservationRepository), new AccommodationReservationRepository()},
            {  typeof(IGuestRepository), new GuestRepository()},
            {  typeof(ICancelledReservationsRepository), new CancelledReservationsRepository()},
            {  typeof(IReservationRequestRepository), new ReservationRequestRepository()},
            { typeof(ITourGradeRepository), new TourGradeRepository()},
            { typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository()}

        };
        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }

    }
    

}

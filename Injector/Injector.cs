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

        };

        public static void BindComponents()
        {
          
            _implementations.Add(typeof(IAccommodationRepository), new AccommodationRepository());
            _implementations.Add(typeof(IGuestGradeRepository), new GuestGradeRepository());
            _implementations.Add(typeof(ILocationRepository), new LocationRepository());
            _implementations.Add(typeof(IOwnerRepository), new OwnerRepository());
            _implementations.Add(typeof(IUserRepository), new UserRepository());
            _implementations.Add(typeof(IImageRepository), new ImageRepository());
        }

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

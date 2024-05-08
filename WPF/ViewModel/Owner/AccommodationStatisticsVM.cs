using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AccommodationStatisticsVM : ViewModelBase
    {
        public AccommodationDTO AccommodationDTO { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }

        public AccommodationStatisticsVM(AccommodationDTO accommodationDTO)
        {
            AccommodationDTO = accommodationDTO;
            Images = new ObservableCollection<ImageDTO>();

            if (accommodationDTO.Images != null && accommodationDTO.Images.Any())
            {
                Images.Add(accommodationDTO.Images[0]);
            }
            else
            {
                Images[0].Path = "../../../Resources/Images/Owner/accommodation_placeholder.jpg";
            }
        }
    }
}

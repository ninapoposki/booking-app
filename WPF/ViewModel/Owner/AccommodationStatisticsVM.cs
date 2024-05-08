using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        public ObservableCollection<AccommodationStatisticsDTO> Years { get; set; }

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
                Images[0].Path = "../../../Resources/Icons/Owner/accommodation_placeholder.jpg";
            }

            Years = new ObservableCollection<AccommodationStatisticsDTO>();
            Years[0].Year = 2024;
            Years[1].Year = 2023;
            Years[2].Year = 2022;
            Update();
        }
        public void Update()
        {
            foreach(AccommodationStatisticsDTO accommodationDTO in Years) { 
                
            }
        }
        
    }
}

using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AvailableDatesVM : ViewModelBase
    {
        public event EventHandler<Range> DatesSelected;

        private ObservableCollection<Range> dates;
        public ObservableCollection<Range> Dates
        {
            get { return dates; }
            set
            {
                dates = value;
                OnPropertyChanged();

            }
        }

        private Range _selectedDate;
        public Range selectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                BookCommand.RaiseCanExecuteChanged();

            }
        }
        private AccommodationReservationDTO accommodationReservationDTO;

        private AccommodationRenovationDTO selectedRenovation;
        public AccommodationRenovationDTO SelectedRenovation
        {
            get { return selectedRenovation; }
            set
            {
                selectedRenovation = value;
                OnPropertyChanged();
            }
        }
        public AccommodationRenovationService AccommodationRenovationService;
        public MyICommand<Range> BookCommand { get; set; }

        public AvailableDatesVM( List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
         
            Dates = new ObservableCollection<Range>(dates.Select(r => new Range { InitialDate = r.Item1, EndDate = r.Item2 }).ToList());
            SelectedRenovation = new AccommodationRenovationDTO();
            BookCommand = new MyICommand<Range>(OnBookAccommodation);
           
            AccommodationRenovationService = new AccommodationRenovationService(Injector.Injector.CreateInstance<IAccommodationRenovationRepository>());
            accommodationReservationDTO = accommodationReservation;
        }
       
        public void OnBookAccommodation(Range selectedDate)
        {
            SelectedRenovation.InitialDate = accommodationReservationDTO.InitialDate;
            SelectedRenovation.EndDate = accommodationReservationDTO.EndDate;
            SelectedRenovation.Duration = accommodationReservationDTO.DaysToStay;
            DatesSelected?.Invoke(this, selectedDate);
            // AccommodationRenovationService.Add(SelectedRenovation);
            Close();
            // MessageBox.Show("Reservation added successfully");
        }
        private void Close()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
        public class Range
        {
            public DateTime InitialDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}

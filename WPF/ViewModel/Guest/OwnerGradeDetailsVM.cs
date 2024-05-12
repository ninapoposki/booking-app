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
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class OwnerGradeDetailsVM:ViewModelBase
    {
        public NavigationService navigationService { get; set; }
        private GuestGradeDTO _selectedGrade;
        public GuestGradeDTO SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                if (_selectedGrade != value)
                {
                    _selectedGrade = value;
                    OnPropertyChanged(nameof(SelectedGrade));
                }
            }
        }
        public MyICommand ExitCommand { get; set; }

        public OwnerGradeDetailsVM(NavigationService navigationService,GuestGradeDTO guestGradeDTO) 
        {
            SelectedGrade = guestGradeDTO;
            this.navigationService = navigationService;
            ExitCommand = new MyICommand(OnExitGradeDetails);

        }
        public void OnExitGradeDetails()
        {
            navigationService.GoBack();
        }



    }
}

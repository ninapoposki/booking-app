using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Domain.IRepositories;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerMainWindowVM : ViewModelBase
    {
        private string loggedInUserUsername;
        public int loggedInUserId;
        private double gradeOwnerLimit = 4.5;
        int gradeNum;
        
        public OwnerDTO ownerDTO { get; set; }
        public UserService userService;
        public OwnerService ownerService;
        public AccommodationGradeService accommodationGradeService;

        public OwnerMainWindowVM(string username) {
            ownerDTO = new OwnerDTO();
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            ownerService = new OwnerService();
            accommodationGradeService = new AccommodationGradeService();

            loggedInUserUsername = username;
            loggedInUserId = userService.GetByUsername(loggedInUserUsername).Id;
            
            double average = GetAverageGrade();
            AverageGrade = Math.Round(average, 2);
            Update();
        }

        public void Update()
        {
            ownerDTO = ownerService.UpdateOwner(loggedInUserId);
            if (gradeNum > 49){
                ownerDTO.Role = (AverageGrade > gradeOwnerLimit) ? "SUPEROWNER" : "OWNER";
            } else { 
                ownerDTO.Role = "OWNER";
            }
            string Role =ownerDTO.Role;
            ownerService.UpdateOwnerRole(ownerDTO, Role);
        }
        
        public double GetAverageGrade()
        {
            double gradeSum = 0;
            gradeNum = 0;
            foreach( double grade in accommodationGradeService.GetAverageGrades(loggedInUserUsername))
            {
                gradeNum++;
                gradeSum += grade;
            }
            return gradeSum / (double)gradeNum;
        }
        public void AddAccommodationClick()
        {
            AddAccommodation addAccommodationWindow = new AddAccommodation(loggedInUserUsername);
            addAccommodationWindow.ShowDialog();
        }
        public void GradeGuestClick()
        {
            GuestReservations guestReservations = new GuestReservations();
            guestReservations.ShowDialog();
        }
        public void NotificationsClick()
        {
            Notifications notifications = new Notifications();
            notifications.ShowDialog();
        }

        public void ReservationsClick()
        {
            GuestReservations reservations = new GuestReservations();
            reservations.ShowDialog();
        }
        public void MyGradesClick()
        {
            OwnerGrades grades = new OwnerGrades(loggedInUserUsername);
            grades.ShowDialog();
        }
        public void RequestsClick()
        {
            DateChangeRequests datechanges = new DateChangeRequests();
            datechanges.ShowDialog();
        }

        private double averageGrade;
        public double AverageGrade
        {
            get { return averageGrade; }
            set
            {
                averageGrade = value;
                OnPropertyChanged("AverageGrade");
            }
        }
    }
}

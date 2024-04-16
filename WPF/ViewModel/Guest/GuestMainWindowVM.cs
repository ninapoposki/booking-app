﻿using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestMainWindowVM : ViewModelBase
    {
        private readonly AccommodationService accommodationService;
        private readonly ImageService imageService;
        private readonly LocationService locationService;
        private readonly OwnerService ownerService;
        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }
        private AccommodationDTO selectedAccommodation;
        public AccommodationDTO SelectedAccommodation{
            get => selectedAccommodation;
            set { selectedAccommodation = value;  OnPropertyChanged(nameof(SelectedAccommodation));}
        }
        private string nameFilter;
        public string NameFilter{
            get => nameFilter;
            set { nameFilter = value; OnPropertyChanged(nameof(NameFilter)); }
        }
        private string cityFilter;
        public string CityFilter {
            get => cityFilter;
            set{ cityFilter = value; OnPropertyChanged(nameof(CityFilter));}
        }
        private string countryFilter;
        public string CountryFilter{
            get => countryFilter;
            set { countryFilter = value; OnPropertyChanged(nameof(CountryFilter));}
        }
        private AccommodationType? selectedType;
        public AccommodationType? SelectedType {
            get => selectedType;
            set { selectedType = value; OnPropertyChanged(nameof(SelectedType));}
        }
        private string numberOfGuests;
        public string NumberOfGuests {
            get => numberOfGuests;
            set { numberOfGuests = value; OnPropertyChanged(nameof(NumberOfGuests));}
        }
        private string numberOfDaysToStay;
        public string NumberOfDaysToStay{
            get => numberOfDaysToStay;
            set { numberOfDaysToStay = value; OnPropertyChanged(nameof(NumberOfDaysToStay));}
        }
        public GuestMainWindowVM(){
            accommodationService = new AccommodationService();
            imageService = new ImageService();
            locationService = new LocationService();
            ownerService = new OwnerService();
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            Images = new ObservableCollection<ImageDTO>();
            Types = new ObservableCollection<AccommodationType>(Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>());
            Update();
        }
        public void Update(){
            AllAccommodations.Clear();
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            AddAccommodations(allImages, "SUPEROWNER");
            AddAccommodations(allImages, "OWNER");}
        public void AddAccommodations(List<ImageDTO> allImages, string role) {
            var accommodations = accommodationService.GetAll();
            foreach (var accommodation in accommodations){
                var owner = ownerService.GetByIdDTO(accommodation.OwnerId);
                if (owner.Role == role) {
                    var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(accommodation.Id, allImages));
                    LocationDTO location = locationService.GetByIdDTO(accommodation.IdLocation);
                    AllAccommodations.Add(new AccommodationDTO(accommodation.ToAccommodation(), location.ToLocation()) { Images = matchingImages }); }
            }
        }
        public void SearchClick(){
            Update();
            var filteredAccommodations = FilterAccommodations();
            AllAccommodations.Clear();
            filteredAccommodations.ForEach(accommodation => AllAccommodations.Add(accommodation));
        }
        private List<AccommodationDTO> FilterAccommodations(){
            return AllAccommodations
                .Where(accommodation =>
                    (string.IsNullOrEmpty(NameFilter) || accommodation.Name.ToLower().Contains(NameFilter.ToLower())) &&
                    (string.IsNullOrEmpty(CityFilter) || accommodation.Location.City.ToLower().Contains(CityFilter.ToLower())) &&
                    (string.IsNullOrEmpty(CountryFilter) || accommodation.Location.Country.ToLower().Contains(CountryFilter.ToLower())) &&
                    (!SelectedType.HasValue || accommodation.AccommodationType == SelectedType.Value) &&
                    (string.IsNullOrEmpty(NumberOfGuests) || accommodation.Capacity >= int.Parse(NumberOfGuests)) &&
                    (string.IsNullOrEmpty(NumberOfDaysToStay) || accommodation.MinStayDays <= double.Parse(NumberOfDaysToStay))).ToList();
        }
        public void BookAccommodationClick() {
            if (SelectedAccommodation != null){
                var dialog = new ReserveAccommodation(SelectedAccommodation);
                dialog.ShowDialog();
            }else{   MessageBox.Show("You didn't choose accommodation!"); }
        }
        public void OpenReservationsClick() {
            var dialog = new MyReservationsWindow();
            dialog.ShowDialog();}
        public void OpenNotificationsClick(){
            var dialog = new GuestNotifications();
            dialog.ShowDialog(); }
    }
}
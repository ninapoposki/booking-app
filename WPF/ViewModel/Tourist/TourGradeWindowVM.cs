﻿using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Tourist;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{   public class TourGradeWindowVM : ViewModelBase
    {   public TourDTO SelectedTour { get; set; }
        private TourReservationService tourReservationService;
        public TourReservationDTO selectedTourReservation;
        private readonly LocationService locationService;
        public ImageService imageService;
        private int tourStartDateId;
        private TourGradeService tourGradeService { get; set; }
        private TourService tourService;
        private TourStartDateService tourStartDateService;
        public TourGradeDTO tourGradeDTO { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageDTO SelectedImage { get; set; }
        public MyICommand<ImageDTO> RemoveImageCommand { get; set; }
        public MyICommand BrowseImageCommand { get; set; }
        public MyICommand ConfirmCommand { get; set; }
        public TourGradeWindowVM(int tourStartDateId)
        {
            this.tourStartDateId = tourStartDateId;
            SelectedTour = new TourDTO();
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            tourGradeDTO = new TourGradeDTO();
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourGradeService = new TourGradeService(Injector.Injector.CreateInstance<ICheckPointRepository>(), Injector.Injector.CreateInstance<ITourGradeRepository>(),
                Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            Images = new ObservableCollection<ImageDTO>(SelectedTour.Images);
            LoadTourData(tourStartDateId);
            selectedTourReservation = tourReservationService.GetReservationByTourId(tourStartDateId);
            RemoveImageCommand = new MyICommand<ImageDTO>(RemoveImage);
            BrowseImageCommand = new MyICommand(BrowseImage);
            ConfirmCommand = new MyICommand(ExecuteConfirmCommand);
        }
        public void Confirm(int knowledge, int language, int attractions)
        {
            tourGradeService.Add(MakeTourGrade(knowledge, language, attractions).ToTourGrade());
            Update();
            MessageBox.Show("Ocijenili ste turu.");
        }
        private void ExecuteConfirmCommand()
        {
            Confirm(KnowledgeRadio, LanguageRadio, AttractionsRadio);
        }
        private void LoadTourData(int tourStartDateId)
        {
            TourStartDateDTO tourStartDate = tourStartDateService.GetTourStartDate(tourStartDateId);
            if (tourStartDate != null)
            {SelectedTour = tourService.GetTour(tourStartDate.TourId);
                if (SelectedTour != null)
                {
                    SelectedTour.SelectedDateTime = tourStartDate;
                    var location = locationService.GetByIdDTO(SelectedTour.LocationId);
                    var allImages = imageService.GetImagesForEntityType(EntityType.TOUR).Where(img => img.EntityId == SelectedTour.Id).ToList();
                    SelectedTour.Images = new ObservableCollection<ImageDTO>(allImages);
                    var firstImage = allImages.FirstOrDefault();
                    if (firstImage != null)
                    {
                        SelectedTour.Images.Clear();
                        SelectedTour.Images.Add(firstImage);
                    } }}}
        public TourGradeDTO MakeTourGrade(int knowledge, int language, int attractions)
        {
            tourGradeDTO.GuideKnowledge = knowledge;
            tourGradeDTO.LanguageKnowledge = language;
            tourGradeDTO.TourAttractions = attractions;
            tourGradeDTO.Comment = comment;
            tourGradeDTO.TourReservationId = selectedTourReservation.Id;
            return tourGradeDTO;
        }
        private void Update()
        {
            int id = tourGradeService.GetCurrentId();
            foreach (ImageDTO image in Images)
            {imageService.UpdateForGrade(image, id);}
        }

        private int knowledgeRadio;
        public int KnowledgeRadio
        { get { return knowledgeRadio; }
            set{
                if (knowledgeRadio != value)
                {knowledgeRadio = value;
                    OnPropertyChanged("KnowledgeRadio");}}
        }
         private int languageRadio;
        public int LanguageRadio
        {   get { return languageRadio; }
            set{
                if (languageRadio != value)
                {
                    languageRadio = value;
                    OnPropertyChanged("LanguageRadio");}} }

        private int attractionsRadio;
        public int AttractionsRadio
        {get { return attractionsRadio; }
            set{if (attractionsRadio != value){
                    attractionsRadio = value;
                    OnPropertyChanged("AttractionsRadio"); }}
        }
        private string comment;
        public void BrowseImage()
        {   OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources\Images"));
            if (openFileDialog.ShowDialog() == true) { AddImage(openFileDialog.FileName); }
        }
        private void AddImage(string absolutePath)
        {   string relativePath = MakeRelativePath(absolutePath);
            Images.Add(imageService.GetByPath(relativePath));
        }
        private string MakeRelativePath(string absolutPath)
        {   string referencePath = "../../../Resources/Images/";
            string[] pathPieces = absolutPath.Split('\\');
            string relativePath = referencePath + pathPieces[pathPieces.Length - 1];
            return relativePath;
        }
        public void RemoveImage(ImageDTO image)
        {Images.Remove(image);}
    }
 }

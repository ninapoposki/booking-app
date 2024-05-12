using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GradeAccommodationVM : ViewModelBase
    {
        public AccommodationGradeService AccommodationGradeService;
        public NavigationService navigationService;
        public ImageService imageService;
        public RenovationRecommendationService recommendationService;
        private AccommodationReservationDTO _selectedAccommodationReservation;

        public AccommodationReservationDTO selectedAccommodationReservation
        {
            get => _selectedAccommodationReservation;
            set
            {
                if (_selectedAccommodationReservation != value)
                {
                    _selectedAccommodationReservation = value;
                    OnPropertyChanged(nameof(selectedAccommodationReservation));
                }
            }
        }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageDTO SelectedImage { get; set; }
        public AccommodationGradeDTO accommodationGradeDTO { get; set; }
        public RenovationRecommendationDTO recommendationDTO { get; set; }
        public MyICommand<object> SetCleanlinessCommand { get; private set; }
        public MyICommand<object> SetCorrectnessCommand { get; private set; }
        public MyICommand<object> SetRecommendationCommand { get; private set; }
        public MyICommand ExitCommand { get; private set; }
        public MyICommand SubmitCommand {  get; private set; }
        public MyICommand BrowseImageCommand { get; private set; }
        public GradeAccommodationVM(NavigationService navigationService,AccommodationReservationDTO accommodationReservationDTO)
        {
            selectedAccommodationReservation = accommodationReservationDTO;
            AccommodationGradeService = new AccommodationGradeService(Injector.Injector.CreateInstance<IAccommodationGradeRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            recommendationService = new RenovationRecommendationService(Injector.Injector.CreateInstance<IRenovationRecommendationRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            accommodationGradeDTO = new AccommodationGradeDTO();
            recommendationDTO=new RenovationRecommendationDTO()
            {
               
                RecommendationComment = "NORECOMMENDATIONS", // Default vrednost
                RecommendationLevel = 0 // Default nivo preporuke
               
        };
            SetCleanlinessCommand = new MyICommand<object>(SetCleanliness);
            SetCorrectnessCommand = new MyICommand<object>(SetCorrectness);
            SetRecommendationCommand = new MyICommand<object>(SetRecommendation);
            SubmitCommand = new MyICommand(OnConfirmAccommodationGrade);
            ExitCommand = new MyICommand(OnExitPage);
            BrowseImageCommand = new MyICommand(OnBrowseImage);
            this.navigationService=navigationService;
        }
        public void OnBrowseImage()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources\Images"));// Konvertuje relativnu u apsolutnu putanju
            if (openFileDialog.ShowDialog() == true) { AddImage(openFileDialog.FileName); }
        }
        public void AddImage(string absolutePath)
        {
            string referencePath = "../../../Resources/Images/";
            string[] pathPieces = absolutePath.Split('\\');
            string relativePath = (referencePath + pathPieces[pathPieces.Length - 1]);
            Images.Add(imageService.GetByPath(relativePath));
        }
       private void OnExitPage()
       {
            navigationService.GoBack();
       }
        public void OnConfirmAccommodationGrade()
        {
            UpdateImages();
            accommodationGradeDTO.Cleanliness = CleanlinessRadio;
            accommodationGradeDTO.Correctness = CorrectnessRadio;
            accommodationGradeDTO.Comment = Comments;
            recommendationDTO.RecommendationLevel = RecommendationRadio;
            // recommendationDTO.RecommendationComment = RecommendationComment;
            if (string.IsNullOrWhiteSpace(RecommendationComment)) { recommendationDTO.RecommendationComment = "NORECOMMENDATIONS"; }
            else { recommendationDTO.RecommendationComment = RecommendationComment; }
            var linkedAccommodationGradeDTO = AccommodationGradeService.GetOneAccommodationGrade(selectedAccommodationReservation, accommodationGradeDTO);
            var linkedRecommendationDTO = recommendationService.GetOneRecommendation(selectedAccommodationReservation, recommendationDTO);
            AccommodationGradeService.Add(linkedAccommodationGradeDTO.ToAccommodationGrade());
            recommendationService.Add(linkedRecommendationDTO.ToRenovationRecommendation());
            MessageBox.Show("You successfully graded accommodation!");
            navigationService.GoBack();
        }
        private void SetCleanliness(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int cleanlinessValue))
            {
                CleanlinessRadio = cleanlinessValue;
            }
        }
        private void SetCorrectness(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int correctnessValue))
            {
                CorrectnessRadio = correctnessValue;
            }
        }
        private void SetRecommendation(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int recommendationValue))
            {
                RecommendationRadio = recommendationValue;
            }
        }

        private void UpdateImages()
        {
            int id = AccommodationGradeService.GetCurrentId();
            foreach (ImageDTO image in Images)
            {
                imageService.UpdateGuestImages(image, id);
            }
        }
        private int cleanlinessRadio;
        public int CleanlinessRadio
        {
            get { return cleanlinessRadio; }
            set
            {
                if (cleanlinessRadio != value)
                {
                    cleanlinessRadio = value;
                    OnPropertyChanged("CleanlinessRadio");
                }
            }
        }

        private int correctnessRadio;
        public int CorrectnessRadio
        {
            get { return correctnessRadio; }
            set
            {
                if (correctnessRadio != value)
                {
                    correctnessRadio = value;
                    OnPropertyChanged("CorrectnessRadio");
                }
            }
        }

        private string comment;
        public string Comments
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        private int recommendationRadio=0;
        public int RecommendationRadio
        {
            get { return recommendationRadio; }
            set
            {
                if (recommendationRadio != value)
                {
                    recommendationRadio = value;
                    OnPropertyChanged("RecommendationRadio");
                }
            }
        }

        private string recommendationComment;
        public string RecommendationComment
        {
            get { return recommendationComment; }
            set
            {
                if (recommendationComment != value)
                {
                    recommendationComment = value;
                    OnPropertyChanged("RecommendationComment");
                }
            }
        }


    }
}
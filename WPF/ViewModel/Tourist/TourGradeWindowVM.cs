using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
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
{
    public class TourGradeWindowVM : ViewModelBase
    {
        public TourDTO SelectedTour { get; set; }
        private TourStartDateService TourStartDateService { get; set; }
        private readonly TourService tourService;
        private TourReservationService tourReservationService;
        public TourReservationDTO selectedTourReservation;
         public ImageService imageService;
        private TourGradeService tourGradeService { get; set; }
        public TourGradeDTO tourGradeDTO { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
         public ImageDTO SelectedImage { get; set; }
        public TourGradeWindowVM(TourDTO selectedTour)
        {

            SelectedTour = new TourDTO();
            tourService = new TourService();
            TourStartDateService = new TourStartDateService();
            tourGradeDTO = new TourGradeDTO();
            tourGradeService = new TourGradeService();
            tourReservationService = new TourReservationService();
            imageService = new ImageService();
            Images = new ObservableCollection<ImageDTO>();
            selectedTourReservation = tourReservationService.GetReservationByTourId(selectedTour.Id);
        }

        public void Confirm(int knowledge, int language, int attractions)
        {

            KnowledgeRadio = knowledge;
            LanguageRadio = language;
            AttractionsRadio = attractions;
            tourGradeDTO.GuideKnowledge = KnowledgeRadio;
            tourGradeDTO.LanguageKnowledge = LanguageRadio;
            tourGradeDTO.TourAttractions = AttractionsRadio;
            tourGradeDTO.Comment = Comment;
            tourGradeDTO.TourReservationId = selectedTourReservation.Id;

            tourGradeService.Add(new TourGrade
            {
                GuideKnowledge = knowledge,
                LanguageKnowledge = language,
                TourAtrractions = attractions,
                Comment = comment,
                TourReservationId = selectedTourReservation.Id
            });
            Update();
            MessageBox.Show("Ocijenili ste turu.");
        }

         private void Update()
         {
             int id = tourGradeService.GetCurrentId();
             foreach (ImageDTO image in Images)
             {
                 imageService.UpdateForGrade(image, id);
             }
         }

        private int knowledgeRadio;
        public int KnowledgeRadio
        {
            get { return knowledgeRadio; }
            set
            {
                if (knowledgeRadio != value)
                {
                    knowledgeRadio = value;
                    OnPropertyChanged("KnowledgeRadio");
                }
            }
        }

        private int languageRadio;
        public int LanguageRadio
        {
            get { return languageRadio; }
            set
            {
                if (languageRadio != value)
                {
                    languageRadio = value;
                    OnPropertyChanged("LanguageRadio");
                }
            }
        }

        private int attractionsRadio;
        public int AttractionsRadio
        {
            get { return attractionsRadio; }
            set
            {
                if (attractionsRadio != value)
                {
                    attractionsRadio = value;
                    OnPropertyChanged("AttractionsRadio");
                }
            }
        }


        private string comment;
        public string Comment
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

        public void BrowseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images"));
            openFileDialog.ShowDialog();
            AddImage(openFileDialog.FileName);
        }

         private void AddImage(string absolutePath)
         {
             string relativePath = MakeRelativePath(absolutePath);
             Images.Add(imageService.GetByPath(relativePath));
         }

        private string MakeRelativePath(string absolutPath)
         {
             string referencePath = "..\\..\\..\\Resources\\Images\\";
             string[] pathPieces = absolutPath.Split('\\');
             string relativePath = referencePath + pathPieces[pathPieces.Length - 1];
             return relativePath.Replace("/", "\\");
         }
       public void RemoveImage()
        {
            if (SelectedImage != null)
            {
                Images.Remove(SelectedImage);
            }
        }
}
    }

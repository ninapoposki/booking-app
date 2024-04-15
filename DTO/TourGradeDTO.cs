using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourGradeDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int TourReservationId {  get; set; }
        private int guideKnowledge;
        public int GuideKnowledge
        {
            get { return guideKnowledge; }
            set
            {
                if (value != guideKnowledge)
                {
                    guideKnowledge = value;
                    OnPropertyChanged("GuideKnowledge");
                }
            }
        }
        private Validity validity;
        public Validity Validity
        {
            get { return validity; }
            set
            {
                if (value != validity)
                {
                    validity = value;
                    OnPropertyChanged("Validity");
                }
            }
        }
        private int languageKnowledge;
        public int LanguageKnowledge
        {
            get { return languageKnowledge; }
            set
            {
                if (value != languageKnowledge)
                {
                    languageKnowledge = value;
                    OnPropertyChanged("LanguageKnowledge");
                }
            }
        }
        private int tourAttractions;
        public int TourAttractions
        {
            get { return tourAttractions; }
            set
            {
                if (value != tourAttractions)
                {
                    tourAttractions = value;
                    OnPropertyChanged("TourAttractions");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                   comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (value != path)
                {
                    path = value;
                    OnPropertyChanged("Path");
                }
            }
        }
        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (value != fullName)
                {
                    fullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }
        private string checkPointName;
        public string CheckPointName
        {
            get { return checkPointName; }
            set
            {
                if (value != checkPointName)
                {
                    checkPointName = value;
                    OnPropertyChanged("CheckPointName");
                }
            }
        }
        public TourGradeDTO() { }
        public TourGradeDTO(TourGrade tourGrade)
        {
            this.Id= tourGrade.Id;
            this.TourReservationId = tourGrade.TourReservationId;
            this.GuideKnowledge = tourGrade.GuideKnowledge;
            this.LanguageKnowledge = tourGrade.LanguageKnowledge;
            this.TourAttractions = tourGrade.TourAtrractions;
            this.Comment = tourGrade.Comment;
            this.Validity=tourGrade.Validity;
        }
        public TourGrade ToTourGrade()
        {
            return new TourGrade(Id, TourReservationId, guideKnowledge, languageKnowledge,tourAttractions,comment);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

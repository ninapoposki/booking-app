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

        private int tourGuestId;

        public int TourGuestId
        {
            get
            {
                return tourGuestId;
            }
            set
            {
                if (value != tourGuestId)
                {
                    tourGuestId = value;
                    OnPropertyChanged("TourGuestId");
                }
            }

        }

        private int guideKnowledge;
        public int GuideKnowledge
        {
            get
            {
                return guideKnowledge;
            }
            set
            {
                if (value != guideKnowledge)
                {
                    guideKnowledge = value;
                    OnPropertyChanged("GuideKnowledge");
                }
            }
        }

        private int languageKnowledge;
        public int LanguageKnowledge
        {

            get
            {
                return languageKnowledge;
            }
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
            get
            {
                return tourAttractions;
            }
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

            get
            {
                return comment;
            }
            set
            {
                if (value != comment)
                {
                   comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        public TourGradeDTO() { }

        public TourGradeDTO(TourGradeDTO tourGrade)
        {
            this.Id= tourGrade.Id;
            this.TourGuestId = tourGrade.TourGuestId;
            this.GuideKnowledge = tourGrade.GuideKnowledge;
            this.LanguageKnowledge = tourGrade.LanguageKnowledge;
            this.TourAttractions=tourGrade.TourAttractions;
            this.Comment = tourGrade.Comment;
        }

        public TourGrade ToTourGrade()
        {

            return new TourGrade(Id, tourGuestId, guideKnowledge, languageKnowledge,tourAttractions,comment);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ForumCommentDTO : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get; set;
        }
        
        private int userId;
        public int UserId
        {
            get { return userId; }
            set { if (userId != value) { userId = value; OnPropertyChanged("UserId"); } }
        }
        
        private int forumId;
        public int ForumId
        {
            get { return forumId; }
            set
            {
                if (forumId != value) { forumId = value; OnPropertyChanged("ForumId"); }
            }
        }
        private int reportNumber;
        public int ReportNumber
        {
            get { return reportNumber; }
            set
            {
                if (reportNumber != value) { reportNumber = value; OnPropertyChanged("ReportNumber"); }
            }
        }
        
        private string comment;
        public string Comment
        {
            get => comment;
            set { comment = value; OnPropertyChanged(nameof(Comment)); }
        }
        


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

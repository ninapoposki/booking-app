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
        private DateTime creationDate;
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                if (creationDate != value) { creationDate = value; OnPropertyChanged("CreationDate"); }
            }
        }
        private bool isHighlighted;
        public bool IsHighlighted
        {
            get => isHighlighted;
            set { isHighlighted = value; OnPropertyChanged(); }
        }
        public GuestDTO Guest { get; set; }

        public OwnerDTO Owner { get; set; }

        public ForumCommentDTO() { }
        public ForumCommentDTO(ForumComment comment)
        {
            this.Id = comment.Id;
            this.UserId = comment.UserId;
            this.ForumId = comment.ForumId;
            this.Comment = comment.Comment;
            this.ReportNumber = comment.ReportNumber;
            // this.Comments=forum.comments;
            this.CreationDate=comment.CreationDate;
        }

        public ForumComment ToForumComment()
        {
            var comment = new ForumComment();
            comment.Id = this.Id;
            comment.UserId = this.UserId;
            comment.ForumId = this.ForumId;
            comment.Comment = comment.Comment;
            comment.ReportNumber = this.ReportNumber;
            // forum.Comments=this.Comments;
            comment.CreationDate= this.CreationDate;
            return comment;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

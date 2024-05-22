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
    public class ForumDTO:INotifyPropertyChanged
    {
        public int id;
        public int Id
        {
            get { return id; }
            set { if (id != value) { id = value; OnPropertyChanged("Id"); } }
        }
        public int guestId;
        public int GuestId
        {
            get { return guestId; }
            set { if (guestId != value) { guestId = value; OnPropertyChanged("GuestId"); } }
        }
        public int locationId;
        public int LocationId
        {
            get { return locationId; }
            set { if (locationId != value) { locationId = value; OnPropertyChanged("LocationId"); } }
        }

        public ActivationType activationType;
        public ActivationType ActivationType
        {
            get { return activationType; }
            set { if (activationType != value) { activationType = value; OnPropertyChanged("ActivationType"); } }
        }
        public ForumStatusType forumStatus;
        public ForumStatusType ForumStatus
        {
            get { return forumStatus; }
            set { if (forumStatus != value) { forumStatus = value; OnPropertyChanged("StatusType"); } }
        }
        public List<ForumComment> comments;
        public List<ForumComment> Comments
        {
            get { return comments; }
            set { if (comments != value) { comments = value; OnPropertyChanged("Comments"); } }
        }

        public ForumDTO() { }
        public ForumDTO(Forum forum)
        {
            this.Id=forum.Id;
            this.GuestId = forum.GuestId;
            this.LocationId = forum.LocationId;
            this.ActivationType = forum.ActivationType;
            this.ForumStatus = forum.ForumStatus;
           // this.Comments=forum.comments;
        }

        public Forum ToForum()
        {
            var forum = new Forum();
            forum.Id = this.Id;
            forum.GuestId = this.GuestId;
            forum.LocationId= this.LocationId;
            forum.ActivationType = this.ActivationType;
            forum.ForumStatus = this.ForumStatus;
           // forum.Comments=this.Comments;
            return forum;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;




    }
}

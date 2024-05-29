using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ForumDetailsVM:ViewModelBase
    {
        public NavigationService NavigationService { get; set; }
        public ForumService forumService;
        public ForumCommentService forumCommentService;
        public AccommodationReservationService accommodationReservationService;
        public GuestService guestService;
        public OwnerService ownerService;
        public ObservableCollection<ForumCommentDTO> ForumComments { get; set; }
        public ForumDTO forumDTO { get; set; }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public int loggedInUserId { get; set; }
        public MyICommand AddCommentCommand { get; set; }
        public MyICommand <string>FilterCommentsCommand { get; set; }
        public MyICommand ExitCommand { get; set; }

        public ForumDetailsVM(NavigationService navigationService,ForumDTO forumDTO,int loggedInUserId) 
        {
            forumService = new ForumService(Injector.Injector.CreateInstance<IForumRepository>());
            forumCommentService = new ForumCommentService(Injector.Injector.CreateInstance<IForumCommentRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                                     Injector.Injector.CreateInstance<IGuestRepository>(),
                                     Injector.Injector.CreateInstance<IUserRepository>(),
                                     Injector.Injector.CreateInstance<IAccommodationRepository>(),
                                     Injector.Injector.CreateInstance<IImageRepository>(),
                                     Injector.Injector.CreateInstance<ILocationRepository>(),
                                     Injector.Injector.CreateInstance<IOwnerRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            ownerService = new OwnerService(Injector.Injector.CreateInstance<IOwnerRepository>());
            ForumComments = new ObservableCollection<ForumCommentDTO>();
            NavigationService = navigationService;
            this.forumDTO = forumDTO;
            this.loggedInUserId = loggedInUserId;
            FilterCommentsCommand = new MyICommand<string>(OnFilterComments);
            AddCommentCommand = new MyICommand(OnAddComment);
            ExitCommand = new MyICommand(OnExitPage);
            UpdateComments();



        }

        public void OnAddComment()
        {
            if (forumService.IsDisabled(forumDTO))
            {
                MessageBox.Show("This forum is closed!You cannot add new comments!");
            }
            else
            {
                // var guestDTO=guestService.GetByUserIdDTO(loggedInUserId);
               // bool isHighlighted = IsGuestHighlighted();
                forumCommentService.AddNewForumComment(loggedInUserId, forumDTO.Id, Comment);
                MessageBox.Show("You have successfully added a comment!");
                Comment = string.Empty;
                UpdateComments();

            }
        }
        /*
         * 
            AllForums.Clear(); // Clear the existing forums
            var forums = forumService.GetAll();
            foreach (var forum in forums)
            {
                var comments = new ObservableCollection<ForumCommentDTO>(forumCommentService.GetCommentsByForum(forum.Id));
                GuestDTO guestDTO = guestService.GetByIdDTO(forum.GuestId);
                LocationDTO locationDTO = locationService.GetByIdDTO(forum.LocationId);

                AllForums.Add(new ForumDTO(forum.ToForum()) { ForumComments = comments, Location = locationDTO, Guest = guestDTO,
                    CanDisable = forum.GuestId == guestDTO.Id // Set CanDisable property
                });
            }
            FilterForums();
         * 
         * 
         * 
         * 
         * 
         */
        public void UpdateComments()
        {
            ForumComments.Clear();
            var commentsForForum=forumCommentService.GetCommentsByForum(forumDTO.Id);
            foreach (var comment in commentsForForum)
            {
                comment.IsHighlighted = IsGuestHighlighted(comment.UserId);
                if (guestService.IsGuestsId(comment.UserId))
                {
                    comment.Guest = guestService.GetByUserIdDTO(comment.UserId);

                }
                else
                {
                    comment.Owner = ownerService.GetByIdDTO(comment.UserId) ;

                }
      
                ForumComments.Add(comment);
            }

        }
        public void OnFilterComments(string filter)
        {
            if (filter == "newest")
            {
                ForumComments = new ObservableCollection<ForumCommentDTO>(ForumComments.OrderByDescending(c => c.CreationDate));
            }
            else
            {
                ForumComments = new ObservableCollection<ForumCommentDTO>(ForumComments.OrderBy(c => c.CreationDate));
            }
            OnPropertyChanged(nameof(ForumComments));
        }

        public bool IsGuestHighlighted(int loggedInUserId)
        {
            var pastReservations = accommodationReservationService.GetPastReservations();
            return pastReservations.Any(res => res.GuestId == loggedInUserId && res.Location.Id == forumDTO.Location.Id);
        }

        public void OnExitPage()
        {
            NavigationService.GoBack();
        }







    }
}

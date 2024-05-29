using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AllForumsVM : ViewModelBase
    {
        public NavigationService navigationService { get; set; }
        private ForumService forumService;
        private ForumCommentService forumCommentService;
        private GuestService guestService;
        private LocationService locationService;
        private UserService userService;
        public MyICommand CreateForumCommand { get; set; }
        public MyICommand<ForumDTO> DisableForumCommand { get; set; }
        public MyICommand<ForumDTO> OpenForumCommand { get; set; }
        public ObservableCollection<ForumDTO> AllForums { get; set; }
        public ObservableCollection<ForumDTO> FilteredForums { get; set; }
        public int loggedInUserId { get; set; }
        private bool _showAllForums = true;
        public bool ShowAllForums
        {
            get => _showAllForums;
            set
            {
                if (_showAllForums != value)
                {
                    _showAllForums = value;
                    OnPropertyChanged();
                    FilterForums();
                }
            }
        }

        private bool _showMyForums;
        public bool ShowMyForums
        {
            get => _showMyForums;
            set
            {
                if (_showMyForums != value)
                {
                    _showMyForums = value;
                    OnPropertyChanged();
                    FilterForums();
                }
            }
        }

        public AllForumsVM(NavigationService navigationService, int loggedInUserId)
        {
            forumService = new ForumService(Injector.Injector.CreateInstance<IForumRepository>());
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            forumCommentService = new ForumCommentService(Injector.Injector.CreateInstance<IForumCommentRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            this.navigationService = navigationService;
            this.loggedInUserId = loggedInUserId;
            AllForums = new ObservableCollection<ForumDTO>();
            CreateForumCommand = new MyICommand(OnCreateNewForum);
            OpenForumCommand = new MyICommand<ForumDTO>(OnOpenForum);
            DisableForumCommand = new MyICommand<ForumDTO>(OnDisableForum);
            Update();
        }

        public void Update()
        {
            AllForums.Clear(); // Clear the existing forums
            var forums = forumService.GetAll();
            foreach (var forum in forums)
            {
                var comments = new ObservableCollection<ForumCommentDTO>(forumCommentService.GetCommentsByForum(forum.Id));
                GuestDTO guestDTO = guestService.GetByIdDTO(forum.GuestId);
                LocationDTO locationDTO = locationService.GetByIdDTO(forum.LocationId);

                AllForums.Add(new ForumDTO(forum.ToForum()) { ForumComments = comments, Location = locationDTO, Guest = guestDTO,
                    CanDisable = forum.GuestId == guestDTO.Id , CanBeUseful=IsUseful(forum.Id)
                });
            }
            FilterForums();
        }

        private void FilterForums()
        {
            if (ShowAllForums)
            {
                FilteredForums = new ObservableCollection<ForumDTO>(AllForums);
            }
            else
            {
                var guestDTO = guestService.GetByUserIdDTO(loggedInUserId);
                FilteredForums = new ObservableCollection<ForumDTO>(AllForums.Where(f => f.GuestId == guestDTO.Id));
            }
            OnPropertyChanged("FilteredForums");
        }

        public void OnDisableForum(ForumDTO forumDTO)
        {
            if (forumService.IsDisabled(forumDTO))
            {
                MessageBox.Show("The forum is already closed");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to disable this forum?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    forumService.SetForumStatus(forumDTO);
                    MessageBox.Show("The forum has been disabled. From now on, it will be read-only.", "Forum Disabled", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public bool IsUseful(int forumId)
        {
            var guestUsers = userService.getByRole(UserType.GUEST);
            var ownerUsers=userService.getByRole(UserType.OWNER);
            int guestCommentCount = forumCommentService.GuestCommentCount(forumId,guestUsers);
            int ownerCommentCount = forumCommentService.OwnerCommentCount(forumId,ownerUsers);
            if(guestCommentCount>=2 && ownerCommentCount>=2)
            {
                return true;
            }
            return false;
        }

        public void OnCreateNewForum()
        {
            CreateForum createForum = new CreateForum(navigationService, loggedInUserId);
            navigationService.Navigate(createForum);
        }

        public void OnOpenForum(ForumDTO forumDTO)
        {
            ForumDetails forumDetails = new ForumDetails(navigationService, forumDTO,loggedInUserId);
            navigationService.Navigate(forumDetails);
        }
    }
}

using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }

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

        public int EntityId { get; set; }


        private EntityType entityType;
        public EntityType EntityType
        {

            get { return entityType; }
            set
            {



                if (value != entityType)
                {


                    entityType = value;
                    OnPropertyChanged("EntityType");
                }
            }
        }

        public Image ToImage()
        {


            return new Image(Id, path, EntityId, entityType);
        }

        public ImageDTO() { }


        public ImageDTO(Image image)
        {
            Id = image.Id;
            Path = image.Path;
            EntityId = image.EntityId;
            EntityType = image.EntityType;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}


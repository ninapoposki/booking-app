using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class ImageRepository : IImageRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<Image> serializer;

        private List<Image> images;

        public Subject subject;

        public ImageRepository()
        {
            serializer = new Serializer<Image>();
            images = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<Image> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Image Add(Image image)
        {
            image.Id = NextId();
            images = serializer.FromCSV(FilePath);
            images.Add(image);
            serializer.ToCSV(FilePath, images);
            subject.NotifyObservers();
            return image;
        }

        public int NextId()
        {
            images = serializer.FromCSV(FilePath);
            if (images.Count < 1)
            {
                return 1;
            }
            return images.Max(i => i.Id) + 1;
        }

        public void Delete(Image image)
        {
            images = serializer.FromCSV(FilePath);
            Image founded = images.Find(i => i.Id == image.Id);
            images.Remove(founded);
            serializer.ToCSV(FilePath, images);
            subject.NotifyObservers();
        }

        public Image Update(Image image)
        {
            images = serializer.FromCSV(FilePath);
            Image current = images.Find(i => i.Id == image.Id);
            int index = images.IndexOf(current);
            images.Remove(current);
            images.Insert(index, image);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, images);
            subject.NotifyObservers();
            return image;
        }


        public List<Image> FilterImages()
        {
            images = serializer.FromCSV(FilePath);
            List<Image> filteredImages = new List<Image>();
            foreach (Image image in images)
            {
                if (image.EntityType.ToString().Equals("NONE") && image.EntityId == -1)
                {
                    filteredImages.Add(image);
                }
            }
            return filteredImages;
        }

        public Image FindByPath(string path)
        {
            Image? image = GetAll().FirstOrDefault(i => i.Path == path);
            return image;
        }


        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}

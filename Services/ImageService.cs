using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ImageService
    {
        private IImageRepository imageRepository;
        public ImageService() { 
            imageRepository=Injector.Injector.CreateInstance<IImageRepository>();
        }

        public void Update(ImageDTO image,int tourId)
        {
            image.EntityId = tourId;
            image.EntityType = EntityType.TOUR;
            imageRepository.Update(image.ToImage());
        }
        public void UpdateAccommodation(ImageDTO image, int accommodationId)
        {
            image.EntityId = accommodationId;
            image.EntityType = EntityType.ACCOMMODATION;
            imageRepository.Update(image.ToImage());
        }
        public void UpdateGuestImages(ImageDTO image, int accommodationGradeId)
        {
            image.EntityId = accommodationGradeId;
            image.EntityType = EntityType.GUEST;
            imageRepository.Update(image.ToImage());
        }
        public string FilterImages()
        {
            string filter = "Image files|";//(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            foreach (Domain.Model.Image image in imageRepository.FilterImages())
            {
                filter += image.Path.Split("\\")[5] + ";";
            }
            filter = filter.TrimEnd(';');
            
            return filter;
        }

        public ImageDTO GetByPath(string relativePath) {
            return new ImageDTO(imageRepository.FindByPath(relativePath));
        }       
        public List<Image> GetAllImages()
        {
            return imageRepository.GetAll();
        }

        public List<ImageDTO> GetImagesByAccommodation(int accommodationId, List<ImageDTO> allImages)
        {
            return allImages.Where(img => img.EntityId == accommodationId).ToList();
        }
        
        public List<ImageDTO>GetImagesForEntityType(EntityType entityType)
        {
            return imageRepository.GetAll()
            .Where(img => img.EntityType == entityType)
            .Select(img => new ImageDTO(img)).ToList();
        }
        public List<ImageDTO> GetAll()
        {
            var images = new List<ImageDTO>();
            foreach (Image image in imageRepository.GetAll())
            {
                images.Add(new ImageDTO(image));
            }
            return images;
        }
    }
}

using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ImageService
    {
        private IImageRepository imageRepository;
       //private TourGradeService tourGradeService;
        public ImageService() { 
            imageRepository=Injector.Injector.CreateInstance<IImageRepository>();
            //tourGradeService= new TourGradeService();
        }

        public void Update(ImageDTO image,int tourId)
        {
            image.EntityId = tourId;
            image.EntityType = EntityType.TOUR;
            imageRepository.Update(image.ToImage());
        }


        public void UpdateForGrade(ImageDTO image, int tourGradeId)
        {
            image.EntityId = tourGradeId;
            image.EntityType = EntityType.TOURGRADE;

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
        public string GetFirstPath(int entityId,string type)
        {
            Image? image=imageRepository.GetAll().Find(i => i.EntityId == entityId && i.EntityType.ToString().Equals(type));
            if (image != null) return image.Path;
            return null;
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

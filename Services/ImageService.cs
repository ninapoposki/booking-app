using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
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
    }
}

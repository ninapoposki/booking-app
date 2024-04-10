using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepositories
{
    public interface IImageRepository
    {
        List<Image>GetAll();
        Image Add(Image image);
        int NextId();
        void Delete(Image image);
        Image Update(Image image);
        List<Image> FilterImages();
        Image FindByPath(string path);
        void Subscribe(IObserver observer);
    }
}

using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IVoucherRepository
    {
        List<Voucher> GetAll();
        Voucher Add(Voucher voucher);
        int NextId();
        void Delete(Voucher voucher);
        Voucher Update(Voucher voucher);
        void Subscribe(IObserver observer);
       
    }
}

using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class VoucherRepository:IVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> serializer;

        private List<Voucher> vouchers;

        public Subject subject;

        public VoucherRepository()
        {
            serializer = new Serializer<Voucher>();
            vouchers = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<Voucher> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Voucher Add(Voucher voucher)
        {
            voucher.Id = NextId();
            vouchers = serializer.FromCSV(FilePath);
            vouchers.Add(voucher);
            serializer.ToCSV(FilePath, vouchers);
            subject.NotifyObservers();
            return voucher;
        }

        public int NextId()
        {
            vouchers = serializer.FromCSV(FilePath);
            if (vouchers.Count < 1)
            {
                return 1;
            }
            return vouchers.Max(c => c.Id) + 1;
        }

        public void Delete(Voucher voucher)
        {
            vouchers = serializer.FromCSV(FilePath);
            Voucher founded = vouchers.Find(t => t.Id == voucher.Id);
            vouchers.Remove(founded);
            serializer.ToCSV(FilePath, vouchers);
            subject.NotifyObservers();
        }

        public Voucher Update(Voucher voucher)
        {
            vouchers = serializer.FromCSV(FilePath);
            Voucher current = vouchers.Find(t => t.Id == voucher.Id);
            int index = vouchers.IndexOf(current);
            vouchers.Remove(current);
            vouchers.Insert(index, voucher);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, vouchers);
            subject.NotifyObservers();
            return voucher;
        }
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
        public Voucher? GetById(int id)
        {
            vouchers = serializer.FromCSV(FilePath);
            return vouchers.Find(s => s.Id == id);
        }
    }
}

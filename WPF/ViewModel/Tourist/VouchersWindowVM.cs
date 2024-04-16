using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class VouchersWindowVM:ViewModelBase
    {
        public TourReservationService tourReservationService;
        public ObservableCollection<VoucherDTO> AllVouchers { get; set; }
        public VoucherService voucherService;

        private VoucherDTO selectedVoucher;
        public VoucherDTO SelectedVoucher
        {
            get => selectedVoucher;
            set
            {
                selectedVoucher = value;
                OnPropertyChanged(nameof(SelectedVoucher));
            }
        }

        public VouchersWindowVM()
        {
            AllVouchers = new ObservableCollection<VoucherDTO>();
            voucherService= new VoucherService();
            tourReservationService= new TourReservationService();
            SelectedVoucher = new VoucherDTO();
            Update();
        }

        public void Update() {

            GetVouchers();
        }

            
       public void GetVouchers()
       {

         AllVouchers.Clear();
         List<VoucherDTO> vouchers = voucherService.GetAll().Where(v=>v.Status == Domain.Model.Status.VALID).ToList();
         foreach (VoucherDTO voucher in vouchers)
         {
            AllVouchers.Add(voucher);
         }
       }

      
        public void Apply()
        {
            if (SelectedVoucher == null)
            {
                MessageBox.Show("Molim vas izaberite vaucer.");
                return;
            }
            SelectedVoucher.Status = Status.USED;
            Voucher updatedVoucher = SelectedVoucher.ToVoucher();
            voucherService.UpdateVoucher(updatedVoucher);
            Update();
        }
    }
}

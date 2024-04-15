using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class VoucherService
    {
        private IVoucherRepository voucherRepository;
        private TourReservationService tourReservationService;
        public VoucherService()
        {
            voucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();
            tourReservationService = new TourReservationService();
        }
        public bool GrantVoucher(TourStartDateDTO selectedStartDate, string description)
        {
            foreach (TourReservation tourReservation in tourReservationService.GetReservationsByStartDate(selectedStartDate.Id))
            {
                Voucher voucher = new Voucher(tourReservation.UserId, description);
                voucherRepository.Add(voucher);
                return true;
            }
            return false;

        }

        public List<VoucherDTO> GetAll()
        {
            List<VoucherDTO> voucherDTOs = new List<VoucherDTO>();
            foreach (Voucher voucher in voucherRepository.GetAll())
            {

                VoucherDTO dto = new VoucherDTO(voucher);
                voucherDTOs.Add(dto);
            }
            return voucherDTOs;
        }
        public void UpdateVoucher(Voucher voucher)
        {
            voucherRepository.Update(voucher);
        }
        public void UpdateVoucherFromDTO(VoucherDTO voucherDTO)
        {
            Voucher voucher = voucherDTO.ToVoucher(); 
            UpdateVoucher(voucher); 
        }

    }
    
}

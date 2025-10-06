using ECommerce.CustomExceptions;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;

namespace ECommerce.Services.CMSService
{
    public class CoupenService : ICoupenService
    {
        private readonly ICoupenDetailRepository _coupenDetailRepository;
        private readonly IUserConextService _userConextService;
        private readonly IStaffRepository _staffRepository;
        public CoupenService(ICoupenDetailRepository coupenDetailRepository, IUserConextService userConextService,IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
            _coupenDetailRepository = coupenDetailRepository;
            _userConextService = userConextService;
        }

        public async Task AddCoupenAsync(AddCoupenDetailsDTO addCoupenDetailsDTO)
        {
            CoupenDetail CDetail=await _coupenDetailRepository.FindByCCode(addCoupenDetailsDTO.CoupenCode);
            var StaffId = _userConextService.GetUserId();
            Staff Stf = await _staffRepository.GetByIdAsync(StaffId);
            if (CDetail != null) {
                throw new AlreadyExistException($"coupen with Coupen code {addCoupenDetailsDTO.CoupenCode}");
            }
            else if (Stf == null)
            {
                throw new SecurityTokenException("invalid token");
            }
                CoupenDetail NewCoupen = new CoupenDetail
                {
                    CCode = addCoupenDetailsDTO.CoupenCode,
                    CType = addCoupenDetailsDTO.CoupenType,
                    CValue = addCoupenDetailsDTO.CoupenValue,
                    ExpiryDate = addCoupenDetailsDTO.ExpiryDate,
                    Status = addCoupenDetailsDTO.CoupenStaus,
                    AddedIp = _userConextService.GetUserIp(),
                    AddedOn = DateTime.Now,
                    UpdatedIp = _userConextService.GetUserIp(),
                    UpdatedOn = DateTime.Now,
                };
            await _coupenDetailRepository.AddAsync(NewCoupen);
           
            NewCoupen.Staffs.Add(Stf);
        }
    }
}

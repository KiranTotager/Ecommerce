using ECommerce.CustomExceptions;
using ECommerce.DTOs.CMSDTOs;
using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Models.CMSModels;

namespace ECommerce.Services.CMSService
{
    public class CoupenService : ICoupenService
    {
        private readonly ICoupenDetailRepository _coupenDetailRepository;
        private readonly IUserContextService _userConextService;
        private readonly IStaffRepository _staffRepository;
        public CoupenService(ICoupenDetailRepository coupenDetailRepository, IUserContextService userConextService,IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
            _coupenDetailRepository = coupenDetailRepository;
            _userConextService = userConextService;
        }

        public async Task AddCoupenAsync(AddCoupenDetailsDTO addCoupenDetailsDTO)
        {
            CoupenDetail CDetail=await _coupenDetailRepository.FindByCCodeAsync(addCoupenDetailsDTO.CoupenCode);
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

        public async Task<IEnumerable<CMSCoupenResponseDto>> CMSGetAllCoupensAsync()
        {
            IEnumerable<CoupenDetail> Coupens= await _coupenDetailRepository.GetAllAsync();
            return Coupens.Select(CpnDtl =>
             new CMSCoupenResponseDto
             {
                 CoupenType=CpnDtl.CType,
                 CoupenCode=CpnDtl.CCode,
                 CoupenValue=CpnDtl.CValue,
                 CoupenStatus=CpnDtl.Status,
                 CoupenExpiryDate=CpnDtl.ExpiryDate
             }
            );
        }

        public async Task<IEnumerable<CustomerCoupenDetailsResponseDto>> CustomerGetAllACtiveCoupensAsync()
        {
            IEnumerable<CoupenDetail> Coupens=  await _coupenDetailRepository.GetActiveCoupensAsync();
            return Coupens.Select(CpnDtl =>
            new CustomerCoupenDetailsResponseDto
            {
                CoupenCode= CpnDtl.CCode,
                CoupenValue=CpnDtl.CValue,
            });
        }
    }
}

using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.AdminServices
{
    public class ExpertService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpertService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExpertResponse> GetAllExpert(int page, int pageSize)
        {
            try
            {
                var data = await _unitOfWork.Expert.GetAllExpertsAsync(page, pageSize);

                ExpertResponse response = new ExpertResponse()
                {
                    Experts = data.Item1,
                    TotalCount = data.Item2
                };

                return response;
            }
            catch (Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }

        public async Task<bool> DeleteExpert(int expertId)
        {
            try
            {
                var expert = await _unitOfWork.Expert.GetByIdAsync(expertId);
                if (expert == null)
                {
                    throw new BadRequestException("Không tìm thấy Chuyên gia");
                }

                _unitOfWork.Expert.Delete(expert);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }
    }
}

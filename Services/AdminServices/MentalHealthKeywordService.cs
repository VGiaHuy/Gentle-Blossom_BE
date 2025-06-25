using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.AdminServices
{
    public class MentalHealthKeywordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MentalHealthKeywordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MentalHealthKeywordResponse> GetAllMentalHealthKeyword(int page, int pageSize)
        {
            try
            {
                var data = await _unitOfWork.MentalHealthKeyword.GetAllKeyWordAsync(page, pageSize);

                MentalHealthKeywordResponse response = new MentalHealthKeywordResponse
                {
                    Keywords = data.Item1,
                    TotalCount = data.Item2
                };

                return response;
            }
            catch(Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }

        public async Task<bool> DeleteMentalHealthKeyword(int keywordId)
        {
            try
            {
                var keyword = await _unitOfWork.MentalHealthKeyword.GetByIdAsync(keywordId);
                if(keyword == null)
                {
                    throw new BadRequestException("Không tìm thấy từ khóa");
                }

                _unitOfWork.MentalHealthKeyword.Delete(keyword);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }

        public async Task<bool> UpdateMentalHealthKeyword(MentalHealthKeyword newKeyword)
        {
            var keyword = await _unitOfWork.MentalHealthKeyword.GetByIdAsync(newKeyword.KeywordId);
            if (keyword == null)
            {
                throw new BadRequestException("Từ khóa không hợp lệ");
            }

            try
            {
                keyword.SeverityLevel = newKeyword.SeverityLevel;
                keyword.Keyword = newKeyword.Keyword;
                keyword.Category = newKeyword.Category;
                keyword.UpdatedAt = DateTime.Now;
                keyword.Weight = newKeyword.Weight;
                keyword.IsActive = newKeyword.IsActive;

                _unitOfWork.MentalHealthKeyword.Update(keyword);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }

        public async Task<bool> AddMentalHealthKeyword(MentalHealthKeyword newKeyword)
        {
            try
            {
                await _unitOfWork.MentalHealthKeyword.AddAsync(newKeyword);
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

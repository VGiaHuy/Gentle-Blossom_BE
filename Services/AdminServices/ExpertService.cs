using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Helpers;
using OfficeOpenXml;
using System.Data;

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

        public List<ExpertInfoImportDTO> ProcessExcelFile(Stream excelStream)
        {
            var result = new List<ExpertInfoImportDTO>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            excelStream.Position = 0;

            var dataTable = SetColumnName(new DataTable());
            var (datas, errorMess) = ExcelHelper.ReadExcelTo<ExpertInfoImportDTO>(excelStream, dataTable);
            excelStream.Position = 0;

            using (var package = new ExcelPackage(excelStream))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("File Excel không chứa sheet nào.");
                }

                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension?.Rows ?? 0;

                foreach (var row in datas)
                {
                    result.Add(row);
                }

                if (rowCount == 0)
                {
                    throw new Exception("Sheet trong file Excel không chứa dữ liệu.");
                }
            }

            return result;
        }

        public async Task SaveToTempTable(List<ExpertInfoImportDTO> data)
        {
            try
            {
                foreach(ExpertInfoImportDTO profileDTO in data)
                {
                    UserProfile user = new UserProfile()
                    {
                        FullName = profileDTO.FullName,
                        UserTypeId = 2,
                        Gender = profileDTO.Gender,
                        BirthDate = profileDTO.BirthDate,
                        Email = profileDTO.Email,
                        PhoneNumber = profileDTO.PhoneNumber,
                    };
                    await _unitOfWork.UserProfile.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    Expert expert = new Expert()
                    {
                        UserId = user.UserId,
                        AcademicTitle = profileDTO.AcademicTitle,
                        Description = profileDTO.Description,
                        Organization = profileDTO.Organization,
                        Specialization = profileDTO.Specialization,
                        Position = profileDTO.Position,
                    };
                    await _unitOfWork.Expert.AddAsync(expert);
                    await _unitOfWork.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        public DataTable SetColumnName(DataTable dt)
        {
            dt.Columns.Add("fullName");
            dt.Columns.Add("gender");
            dt.Columns.Add("phoneNumber");
            dt.Columns.Add("email");
            dt.Columns.Add("birthDate");
            dt.Columns.Add("academicTitle");
            dt.Columns.Add("position");
            dt.Columns.Add("specialization");
            dt.Columns.Add("organization");
            dt.Columns.Add("description");

            return dt;
        }
    }
}

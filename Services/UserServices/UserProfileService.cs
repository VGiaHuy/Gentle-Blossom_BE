using AutoMapper;
using GentleBlossom_BE.Data.DTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Services.GoogleService;

namespace GentleBlossom_BE.Services.UserServices
{
    public class UserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly GoogleDriveService _googleDriveService;

        public UserProfileService(IUnitOfWork unitOfWork, IMapper mapper, GoogleDriveService googleDriveService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _googleDriveService = googleDriveService;
        }

        public async Task<UserProfileViewModel> GetAllUserProfile(int id)
        {
            var user = await _unitOfWork.UserProfile.GetByIdWithUserTypeAndExpertAsync(id);

            if (user == null)
            {
                throw new NotFoundException("Người dùng không tồn tại!");
            }

            var userInfo = _mapper.Map<UserProfileDTO>(user);

            userInfo.Position = user.Expert?.Position ?? string.Empty;
            userInfo.AcademicTitle = user.Expert?.AcademicTitle ?? string.Empty;
            userInfo.Specialization = user.Expert?.Specialization ?? string.Empty;
            userInfo.Organization = user.Expert?.Organization ?? string.Empty;
            userInfo.ExpertId = user.Expert?.ExpertId ?? 0;

            var diarys = await _unitOfWork.HealthJourney.GetAllWithDiaryByUserId(id);
            var allDiary = new List<PsychologyDiaryDTO>(diarys.Sum(d => d.PsychologyDiaries.Count));

            foreach (var diary in diarys)
            {
                foreach (var diarysub in diary.PsychologyDiaries)
                {
                    var diaryDTO = _mapper.Map<PsychologyDiaryDTO>(diary);

                    diaryDTO.TreatmentName = diary.Treatment.TreatmentName;
                    diaryDTO.UserName = diary.User.FullName;

                    diaryDTO.DiaryId = diarysub.DiaryId;
                    diaryDTO.CreatedDate = diarysub.CreatedDate;
                    diaryDTO.Mood = diarysub.Mood;
                    diaryDTO.Content = diarysub.Content;

                    allDiary.Add(diaryDTO);
                }
            }

            var periodics = await _unitOfWork.HealthJourney.GetAllWithPeriodicByUserId(id);
            var allPeriodic = new List<PeriodicHealthDTO>(periodics.Sum(d => d.PeriodicHealths.Count));

            foreach (var periodic in periodics)
            {
                foreach (var periodicSub in periodic.PeriodicHealths)
                {
                    var periodicDTO = _mapper.Map<PeriodicHealthDTO>(periodic);

                    periodicDTO.TreatmentName = periodic.Treatment.TreatmentName;
                    periodicDTO.UserName = periodic.User.FullName;

                    periodicDTO.HealthId = periodicSub.HealthId;
                    periodicDTO.CreatedDate = periodicSub.CreatedDate;
                    periodicDTO.Mood = periodicSub.Mood;
                    periodicDTO.WeeksPregnant = periodicSub.WeeksPregnant;
                    periodicDTO.BloodPressure = periodicSub.BloodPressure;
                    periodicDTO.GenderBaby = periodicSub.GenderBaby;
                    periodicDTO.WaistCircumference = periodicSub.WaistCircumference;
                    periodicDTO.Notes = periodicSub.Notes;
                    periodicDTO.Weight = periodicSub.Weight;

                    allPeriodic.Add(periodicDTO);
                }
            }

            var data = new UserProfileViewModel();
            data.PeriodicHealths = allPeriodic;
            data.UserProfile = userInfo;
            data.PsychologyDiaries = allDiary;

            return data;
        }

        public async Task<UserProfileDTO> GetUserInfo(int id)
        {
            var user = await _unitOfWork.UserProfile.GetByIdWithUserTypeAndExpertAsync(id);

            if (user == null)
            {
                throw new NotFoundException("Người dùng không tồn tại!");
            }

            return _mapper.Map<UserProfileDTO>(user);
        }

        public async Task<bool> UpdateUserProfile(UpdateUserProfileDTO userProfile)
        {
            var user = await _unitOfWork.UserProfile.GetByIdAsync(userProfile.UserId);

            if (user == null)
            {
                throw new NotFoundException("Người dùng không tồn tại!");
            }

            // Ánh xạ dữ liệu từ DTO sang thực thể
            user.FullName = userProfile.FullName;
            user.BirthDate = userProfile.BirthDate;
            user.PhoneNumber = userProfile.PhoneNumber;
            user.Email = userProfile.Email;
            user.Gender = userProfile.Gender;
            user.UserTypeId = userProfile.UserTypeId;

            if (userProfile.Avatar != null && userProfile.Avatar.Length > 0)
            {
                try
                {
                    // Kiểm tra định dạng file
                    var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "video/mp4", "video/webm" };
                    if (!allowedContentTypes.Contains(userProfile.Avatar.ContentType))
                    {
                        throw new BadRequestException($"Unsupported media type: {userProfile.Avatar.ContentType}. Only JPEG, PNG, GIF images or MP4, WebM videos are allowed.");
                    }

                    // Upload file lên Google Drive
                    var fileUrl = await _googleDriveService.UploadFileAsync(userProfile.Avatar, MediaType.Avatar);

                    // Xác định loại media
                    var mediaType = string.IsNullOrEmpty(userProfile.Avatar.ContentType)
                        ? throw new BadRequestException("Missing ContentType")
                        : userProfile.Avatar.ContentType.StartsWith("image/") ? "Image"
                        : userProfile.Avatar.ContentType.StartsWith("video/") ? "Video"
                        : throw new BadRequestException($"Unsupported media type: {userProfile.Avatar.ContentType}");

                    user.AvatarFileName = userProfile.Avatar.FileName;
                    user.AvatarType = mediaType;
                    user.AvatarUrl = fileUrl;
                }
                catch (Exception ex)
                {
                    throw new InternalServerException($"Lỗi upload file: {userProfile.Avatar.FileName}: {ex.Message}");
                }
            }

            _unitOfWork.UserProfile.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

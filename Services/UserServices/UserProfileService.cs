using AutoMapper;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.UserServices
{
    public class UserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileViewModel> GetAllUserProfile(int id)
        {
            var user = await _unitOfWork.UserProfile.GetByIdWithUserTypeAndExpertAsync(id);

            if (user == null)
            {
                throw new NotFoundException("Người dùng không tồn tại!");
            }

            var userInfo = _mapper.Map<UserProfileDTO>(user);

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
    }
}

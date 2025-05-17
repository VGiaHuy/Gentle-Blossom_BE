using AutoMapper;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.UserServices
{
    public class HealthJourneyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HealthJourneyService(IUnitOfWork unitOfWork, IMapper mapper, GentleBlossomContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PsychologyDiaryDTO>> GetDiaryByUserId(int id)
        {
            try
            {
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

                return allDiary;
            }
            catch (Exception ex) 
            {
                throw new InternalServerException($"Lỗi hệ thống: {ex.Message}");
            }

        }

        public async Task<List<PeriodicHealthDTO>> GetPeriodicByUserId(int id)
        {
            try
            {
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

                return allPeriodic;
            }
            catch (Exception ex) 
            {
                throw new InternalServerException($"Lỗi hệ thống: {ex.Message}");
            }
        }

    }
}

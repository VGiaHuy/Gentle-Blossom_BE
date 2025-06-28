using AutoMapper;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Services.GoogleService;
using GentleBlossom_BE.Services.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Services.UserServices
{
    public class PregnancyCareService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly ChatService _chatService;

        public PregnancyCareService(IUnitOfWork unitOfWork, IMapper mapper, ChatService chatService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _chatService = chatService; 
        }

        public async Task<bool> ConnectMessageAsync(ConnectMessageDTO connectMessage)
        {
            try
            {
                var post = await _unitOfWork.Post.GetByIdAsync(connectMessage.PostId);
                if (post == null)
                {
                    throw new BadRequestException("Không tìm thấy bài viết");
                }

                post.Hidden = true;

                // kiểm tra nếu cuộc trò chuyện đã tồn tại thì không tạo cuộc trò chuyện mới
                var connection = await _unitOfWork.ChatRoomUser.AreParticipantsInPrivateChatRoomAsync(connectMessage.ExpertId, post.PosterId);

                if (!connection)
                {
                    var chatRoomName = "Tư vấn sức khỏe";
                    var chatRoom = await _chatService.CreateChatRoomAsync(chatRoomName, false, connectMessage.ExpertId);
                    await _chatService.AddUserToChatRoomAsync(chatRoom.ChatRoomId, post.PosterId);
                }

                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while connecting the message.", ex);
            }
        }

        public async Task<List<HealthJourneyDTO>> GetHealthJourney(int chatRoomId, int expertId)
        {
            try
            {
                // tìm người dùng không phải là chuyên gia trong phòng chat
                var userId = await _unitOfWork.ChatRoomUser.GetUseIdByChatRoomIdAsync(chatRoomId, expertId);
                if (userId == 0 || userId == null)
                {
                    throw new BadRequestException("Không tìm thấy người dùng trong phòng chat");
                }

                var healthJourneys = await _unitOfWork.HealthJourney.GetAllByUserId(userId);

                if (healthJourneys == null || !healthJourneys.Any())
                {
                    return new List<HealthJourneyDTO>();
                }

                var healthJourneyDtos = _mapper.Map<List<HealthJourneyDTO>>(healthJourneys);

                return healthJourneyDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the health journey.", ex);
            }
        }

        public async Task<List<PeriodicHealthDTO>> GetAllPeriodicByHeathId(int heathId)
        {
            try
            {
                var healthJourneys = await _unitOfWork.PeriodicHealth.GetAllByHealthJourneyId(heathId);

                if (healthJourneys == null || !healthJourneys.Any())
                {
                    return new List<PeriodicHealthDTO>();
                }

                var healthJourneyDtos = _mapper.Map<List<PeriodicHealthDTO>>(healthJourneys);

                return healthJourneyDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the health journey.", ex);
            }
        }

        public async Task<List<PsychologyDiaryDTO>> GetAllPsychologyByHeathId(int heathId)
        {
            try
            {
                var healthJourneys = await _unitOfWork.PsychologyDiary.GetAllByHealthJourneyId(heathId);

                if (healthJourneys == null || !healthJourneys.Any())
                {
                    return new List<PsychologyDiaryDTO>();
                }

                var healthJourneyDtos = _mapper.Map<List<PsychologyDiaryDTO>>(healthJourneys);

                return healthJourneyDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the health journey.", ex);
            }
        }

        public async Task<List<MonitoringFormDTO>> GetAllMonitoringFormByHeathId(int healthJourneyId)
        {
            try
            {
                var monitoringForms = await _unitOfWork.MonitoringForm.GetAllMonitoringFormByHeathId(healthJourneyId);
                if (monitoringForms == null)
                {
                    throw new BadRequestException("Không tìm thấy hành trình sức khỏe");
                }

                var monitoringFormDTOs = _mapper.Map<List<MonitoringFormDTO>>(monitoringForms);

                return monitoringFormDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the health journey by ID.", ex);
            }
        }

        public async Task CreateHealthJourney(CreateHealthJourneyDTO createHealth)
        {
            try
            {
                var userId = await _unitOfWork.ChatRoomUser.GetUseIdByChatRoomIdAsync(createHealth.ChatRoomId, createHealth.ExpertId);
                var expert = await _unitOfWork.Expert.GetExpertByUserIdAsync(createHealth.ExpertId);

                if (expert == null)
                {
                    throw new BadRequestException("Không tìm thấy chuyên gia");
                }
                if (userId == 0)
                {
                    throw new BadRequestException("Không tìm thấy người dùng trong phòng chat");
                }

                HealthJourney healthJourney = new HealthJourney
                {
                    UserId = userId,
                    TreatmentId = createHealth.TreatmentId,
                    JourneyName = createHealth.JourneyName,
                    DueDate = createHealth.DueDate,
                    Status = false
                };
                await _unitOfWork.HealthJourney.AddAsync(healthJourney);
                await _unitOfWork.SaveChangesAsync();

                MonitoringForm monitoringForm = new MonitoringForm
                {
                    Status = createHealth.MonitoringStatus,
                    Notes = createHealth.MonitoringNote,
                    ExpertId = expert.ExpertId,
                    JourneyId = healthJourney.JourneyId,
                };
                await _unitOfWork.MonitoringForm.AddAsync(monitoringForm);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the health journey.", ex);
            }
        }

        public async Task CreateMonitoring(CreateMonitoringDTO createMonitoring)
        {
            try
            {
                var expert = await _unitOfWork.Expert.GetExpertByUserIdAsync(createMonitoring.ExpertId);

                if (expert == null)
                {
                    throw new BadRequestException("Không tìm thấy chuyên gia");
                }

                MonitoringForm monitoringForm = new MonitoringForm
                {
                    Status = createMonitoring.Status,
                    Notes = createMonitoring.Notes,
                    ExpertId = expert.ExpertId,
                    JourneyId = createMonitoring.JourneyId,
                };
                await _unitOfWork.MonitoringForm.AddAsync(monitoringForm);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the health journey.", ex);
            }
        }

        public async Task CreatePeriodic(PeriodicHealthDTO periodicHealth)
        {
            try
            {
                var periodic = new PeriodicHealth()
                {
                    Mood = periodicHealth.Mood,
                    GenderBaby = periodicHealth.GenderBaby,
                    BloodPressure = periodicHealth.BloodPressure,
                    WaistCircumference = periodicHealth.WaistCircumference,
                    Weight = periodicHealth.Weight,
                    WeeksPregnant = periodicHealth.WeeksPregnant,
                    Notes = periodicHealth.Notes,
                    JourneyId = periodicHealth.JourneyId,
                };

                await _unitOfWork.PeriodicHealth.AddAsync(periodic);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the health journey.", ex);
            }
        }

        public async Task CreatePsychologyDiary(PsychologyDiaryDTO psychologyDiary)
        {
            try
            {
                var psychology = new PsychologyDiary()
                {
                    Content = psychologyDiary.Content,
                    Mood = psychologyDiary.Mood,
                    JourneyId = psychologyDiary.JourneyId,
                };

                await _unitOfWork.PsychologyDiary.AddAsync(psychology);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the health journey.", ex);
            }
        }

        public async Task CompleteJourney(int journeyId)
        {
            try
            {
                var healthJourney = await _unitOfWork.HealthJourney.GetByIdAsync(journeyId);

                if (healthJourney == null)
                {
                    throw new BadRequestException("Không tìm thấy hành trình!");
                }

                healthJourney.Status = true;

                _unitOfWork.HealthJourney.Update(healthJourney);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        public async Task CreateNewJourneyWithData(CreateNewJourneyWithDataDTO requets)
        {
            try
            {
                if (requets == null)
                    throw new BadRequestException("Dữ liệu không hợp lệ");

                if (requets.TreatmentId == Treatments.Periodic)
                {
                    var healthJourney = new HealthJourney()
                    {
                        UserId = requets.UserId,
                        TreatmentId = Treatments.Periodic,
                        JourneyName = requets.JourneyName,
                        Status = true,
                    };
                    await _unitOfWork.HealthJourney.AddAsync(healthJourney);
                    await _unitOfWork.SaveChangesAsync();

                    var periodicHealth = _mapper.Map<PeriodicHealth>(requets);
                    periodicHealth.JourneyId = healthJourney.JourneyId;

                    await _unitOfWork.PeriodicHealth.AddAsync(periodicHealth);
                }
                else
                {
                    var healthJourney = new HealthJourney()
                    {
                        UserId = requets.UserId,
                        TreatmentId = Treatments.Periodic,
                        JourneyName = requets.JourneyName,
                        Status = true,
                    };
                    await _unitOfWork.HealthJourney.AddAsync(healthJourney);
                    await _unitOfWork.SaveChangesAsync();

                    var psychologyDiary = _mapper.Map<PsychologyDiary>(requets);
                    psychologyDiary.JourneyId = healthJourney.JourneyId;

                    await _unitOfWork.PsychologyDiary.AddAsync(psychologyDiary);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Models;

public partial class GentleBlossomContext : DbContext
{
    public GentleBlossomContext()
    {
    }

    public GentleBlossomContext(DbContextOptions<GentleBlossomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<ChatRoomUser> ChatRoomUsers { get; set; }

    public virtual DbSet<CommentPost> CommentPosts { get; set; }

    public virtual DbSet<ConnectionMedical> ConnectionMedicals { get; set; }

    public virtual DbSet<Expert> Experts { get; set; }

    public virtual DbSet<HealthJourney> HealthJourneys { get; set; }

    public virtual DbSet<LoginUser> LoginUsers { get; set; }

    public virtual DbSet<MentalHealthKeyword> MentalHealthKeywords { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }

    public virtual DbSet<MonitoringForm> MonitoringForms { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PeriodicHealth> PeriodicHealths { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostAnalysis> PostAnalyses { get; set; }

    public virtual DbSet<PostCategory> PostCategories { get; set; }

    public virtual DbSet<PostLike> PostLikes { get; set; }

    public virtual DbSet<PostMedium> PostMedia { get; set; }

    public virtual DbSet<PsychologyDiary> PsychologyDiaries { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewImage> ReviewImages { get; set; }

    public virtual DbSet<RoleAdmin> RoleAdmins { get; set; }

    public virtual DbSet<Treatment> Treatments { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=VOGIAHUY\\SQLEXPRESS;Initial Catalog=GentleBlossom;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Administ__AD0500A6535FAE03");

            entity.ToTable("Administrator");

            entity.Property(e => e.AdminId).HasColumnName("adminId");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Role).WithMany(p => p.Administrators)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Administr__roleI__2A164134");

            entity.HasOne(d => d.User).WithMany(p => p.Administrators)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Administr__userI__29221CFB");
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.ChatRoomId).HasName("PK__ChatRoom__CB58B492342793F5");

            entity.ToTable("ChatRoom");

            entity.Property(e => e.ChatRoomId).HasColumnName("chatRoomId");
            entity.Property(e => e.ChatRoomName)
                .HasMaxLength(100)
                .HasColumnName("chatRoomName");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsGroup).HasColumnName("isGroup");
        });

        modelBuilder.Entity<ChatRoomUser>(entity =>
        {
            entity.HasKey(e => e.ChatRoomUserId).HasName("PK__ChatRoom__C9D3D5666CED9407");

            entity.ToTable("ChatRoomUser");

            entity.Property(e => e.ChatRoomUserId).HasColumnName("chatRoomUserId");
            entity.Property(e => e.ChatRoomId).HasColumnName("chatRoomId");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("joinedAt");
            entity.Property(e => e.ParticipantId).HasColumnName("participantId");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.ChatRoomUsers)
                .HasForeignKey(d => d.ChatRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatRoomU__chatR__65370702");

            entity.HasOne(d => d.Participant).WithMany(p => p.ChatRoomUsers)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatRoomU__parti__662B2B3B");
        });

        modelBuilder.Entity<CommentPost>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__CommentP__CDDE919DAC3EA0CA");

            entity.ToTable("CommentPost");

            entity.HasIndex(e => e.ParentCommentId, "IDX_Comment_ParentComment");

            entity.HasIndex(e => e.PostId, "IDX_Comment_Post");

            entity.HasIndex(e => e.PosterId, "IDX_Comment_Poster");

            entity.Property(e => e.CommentId).HasColumnName("commentId");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("commentDate");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .HasColumnName("content");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("fileName");
            entity.Property(e => e.MediaType)
                .HasMaxLength(20)
                .HasColumnName("mediaType");
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(1000)
                .HasColumnName("mediaUrl");
            entity.Property(e => e.ParentCommentId).HasColumnName("parentCommentId");
            entity.Property(e => e.PostId).HasColumnName("postId");
            entity.Property(e => e.PosterId).HasColumnName("posterId");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__CommentPo__paren__51300E55");

            entity.HasOne(d => d.Post).WithMany(p => p.CommentPosts)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CommentPo__postI__4F47C5E3");

            entity.HasOne(d => d.Poster).WithMany(p => p.CommentPosts)
                .HasForeignKey(d => d.PosterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CommentPo__poste__503BEA1C");
        });

        modelBuilder.Entity<ConnectionMedical>(entity =>
        {
            entity.HasKey(e => e.ConnectId).HasName("PK__Connecti__3B10F317481405FA");

            entity.ToTable("ConnectionMedical");

            entity.HasIndex(e => e.PostId, "IX_ExpertConnections_PostId");

            entity.Property(e => e.ConnectId).HasColumnName("connectId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.ExpertId).HasColumnName("expertId");
            entity.Property(e => e.JourneyId).HasColumnName("journeyId");
            entity.Property(e => e.PostId).HasColumnName("postId");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Expert).WithMany(p => p.ConnectionMedicals)
                .HasForeignKey(d => d.ExpertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Connectio__exper__66EA454A");

            entity.HasOne(d => d.Journey).WithMany(p => p.ConnectionMedicals)
                .HasForeignKey(d => d.JourneyId)
                .HasConstraintName("FK__Connectio__journ__68D28DBC");

            entity.HasOne(d => d.Post).WithMany(p => p.ConnectionMedicals)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Connectio__postI__6BAEFA67");

            entity.HasOne(d => d.User).WithMany(p => p.ConnectionMedicals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Connectio__userI__67DE6983");
        });

        modelBuilder.Entity<Expert>(entity =>
        {
            entity.HasKey(e => e.ExpertId).HasName("PK__Expert__99DFBAF6E0EBE54E");

            entity.ToTable("Expert");

            entity.HasIndex(e => e.UserId, "UQ_Expert_UserId").IsUnique();

            entity.Property(e => e.ExpertId).HasColumnName("expertId");
            entity.Property(e => e.AcademicTitle)
                .HasMaxLength(50)
                .HasColumnName("academicTitle");
            entity.Property(e => e.Organization)
                .HasMaxLength(200)
                .HasColumnName("organization");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .HasColumnName("position");
            entity.Property(e => e.Specialization)
                .HasMaxLength(100)
                .HasColumnName("specialization");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithOne(p => p.Expert)
                .HasForeignKey<Expert>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expert__userId__245D67DE");
        });

        modelBuilder.Entity<HealthJourney>(entity =>
        {
            entity.HasKey(e => e.JourneyId).HasName("PK__HealthJo__BBECC39F65FA3FDB");

            entity.ToTable("HealthJourney");

            entity.HasIndex(e => e.TreatmentId, "IDX_HealthJourney_Treatment");

            entity.HasIndex(e => e.UserId, "IDX_HealthJourney_User");

            entity.Property(e => e.JourneyId).HasColumnName("journeyId");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("startDate");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TreatmentId).HasColumnName("treatmentId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Treatment).WithMany(p => p.HealthJourneys)
                .HasForeignKey(d => d.TreatmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HealthJou__treat__2FCF1A8A");

            entity.HasOne(d => d.User).WithMany(p => p.HealthJourneys)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HealthJou__userI__2EDAF651");
        });

        modelBuilder.Entity<LoginUser>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__LoginUse__1F5EF4CF18341628");

            entity.ToTable("LoginUser");

            entity.HasIndex(e => e.UserName, "IDX_LoginUser_UserName").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__LoginUse__66DCF95C68D23867").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ__LoginUse__CB9A1CFEDD687F51").IsUnique();

            entity.Property(e => e.LoginId).HasColumnName("loginId");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("userName");

            entity.HasOne(d => d.User).WithOne(p => p.LoginUser)
                .HasForeignKey<LoginUser>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoginUser__userI__2180FB33");
        });

        modelBuilder.Entity<MentalHealthKeyword>(entity =>
        {
            entity.HasKey(e => e.KeywordId).HasName("PK__MentalHe__A6DC9B8A3DD9BF35");

            entity.HasIndex(e => e.IsActive, "IX_Keywords_Active");

            entity.HasIndex(e => e.Category, "IX_Keywords_Category");

            entity.Property(e => e.KeywordId).HasColumnName("keywordId");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Keyword).HasMaxLength(200);
            entity.Property(e => e.SeverityLevel)
                .HasMaxLength(20)
                .HasColumnName("severityLevel");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__4808B9936DCF394E");

            entity.ToTable("Message");

            entity.HasIndex(e => e.SenderId, "IDX_Message_Sender");

            entity.Property(e => e.MessageId).HasColumnName("messageId");
            entity.Property(e => e.ChatRoomId).HasColumnName("chatRoomId");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .HasColumnName("content");
            entity.Property(e => e.HasAttachment)
                .HasDefaultValue(false)
                .HasColumnName("hasAttachment");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("isRead");
            entity.Property(e => e.SenderId).HasColumnName("senderId");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sentAt");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__chatRoo__69FBBC1F");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__senderI__6AEFE058");
        });

        modelBuilder.Entity<MessageAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__MessageA__C417BD81EC712037");

            entity.ToTable("MessageAttachment");

            entity.Property(e => e.AttachmentId).HasColumnName("attachmentId");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("fileName");
            entity.Property(e => e.FilePath)
                .HasMaxLength(1000)
                .HasColumnName("filePath");
            entity.Property(e => e.FileSize).HasColumnName("fileSize");
            entity.Property(e => e.FileType)
                .HasMaxLength(20)
                .HasColumnName("fileType");
            entity.Property(e => e.MessageId).HasColumnName("messageId");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageAttachments)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MessageAt__messa__70A8B9AE");
        });

        modelBuilder.Entity<MonitoringForm>(entity =>
        {
            entity.HasKey(e => e.FormId).HasName("PK__Monitori__51BCB72BCC3C708B");

            entity.ToTable("MonitoringForm");

            entity.Property(e => e.FormId).HasColumnName("formId");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdDate");
            entity.Property(e => e.ExpertId).HasColumnName("expertId");
            entity.Property(e => e.JourneyId).HasColumnName("journeyId");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Expert).WithMany(p => p.MonitoringForms)
                .HasForeignKey(d => d.ExpertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Monitorin__exper__40058253");

            entity.HasOne(d => d.Journey).WithMany(p => p.MonitoringForms)
                .HasForeignKey(d => d.JourneyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Monitorin__journ__40F9A68C");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__4BA5CEA9B7B97A44");

            entity.HasIndex(e => e.IsSeen, "IDX_Notifications_Status");

            entity.HasIndex(e => e.UserId, "IDX_Notifications_User");

            entity.Property(e => e.NotificationId).HasColumnName("notificationId");
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .HasColumnName("content");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.IsSeen)
                .HasDefaultValue(false)
                .HasColumnName("isSeen");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__userI__55009F39");
        });

        modelBuilder.Entity<PeriodicHealth>(entity =>
        {
            entity.HasKey(e => e.HealthId).HasName("PK__Periodic__73703F35D21E0120");

            entity.ToTable("PeriodicHealth");

            entity.HasIndex(e => e.JourneyId, "IDX_PeriodicHealth_Journey");

            entity.HasIndex(e => new { e.JourneyId, e.CreatedDate }, "UQ_PeriodicHealth").IsUnique();

            entity.Property(e => e.HealthId).HasColumnName("healthId");
            entity.Property(e => e.BloodPressure).HasColumnName("bloodPressure");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdDate");
            entity.Property(e => e.GenderBaby).HasColumnName("genderBaby");
            entity.Property(e => e.JourneyId).HasColumnName("journeyId");
            entity.Property(e => e.Mood)
                .HasMaxLength(100)
                .HasColumnName("mood");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.WaistCircumference)
                .HasColumnType("decimal(3, 1)")
                .HasColumnName("waistCircumference");
            entity.Property(e => e.WeeksPregnant).HasColumnName("weeksPregnant");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(3, 1)")
                .HasColumnName("weight");

            entity.HasOne(d => d.Journey).WithMany(p => p.PeriodicHealths)
                .HasForeignKey(d => d.JourneyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PeriodicH__journ__339FAB6E");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__DD0C739A6A2D0D86");

            entity.ToTable("Post");

            entity.HasIndex(e => e.CategoryId, "IDX_Post_Category");

            entity.HasIndex(e => e.CreatedDate, "IDX_Post_CreatedDate");

            entity.HasIndex(e => e.PosterId, "IDX_Post_Poster");

            entity.Property(e => e.PostId).HasColumnName("postId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdDate");
            entity.Property(e => e.NumberOfLike).HasColumnName("numberOfLike");
            entity.Property(e => e.PosterId).HasColumnName("posterId");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__categoryId__47A6A41B");

            entity.HasOne(d => d.Poster).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PosterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__posterId__46B27FE2");
        });

        modelBuilder.Entity<PostAnalysis>(entity =>
        {
            entity.HasKey(e => e.PostAnalysisId).HasName("PK__PostAnal__03905F8419AC32E0");

            entity.ToTable("PostAnalysis");

            entity.HasIndex(e => e.PostId, "IX_PostAnalysis_PostId");

            entity.Property(e => e.PostAnalysisId).HasColumnName("postAnalysisId");
            entity.Property(e => e.AnalysisStatus)
                .HasMaxLength(20)
                .HasColumnName("analysisStatus");
            entity.Property(e => e.PostId).HasColumnName("postId");
            entity.Property(e => e.RiskLevel)
                .HasMaxLength(20)
                .HasColumnName("riskLevel");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.SentimentScore).HasColumnName("sentimentScore");

            entity.HasOne(d => d.Post).WithMany(p => p.PostAnalyses)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostAnaly__postI__7AF13DF7");
        });

        modelBuilder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__PostCate__23CAF1D88F5A6628");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PostId }).HasName("PK_post_likes");

            entity.ToTable("PostLike");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.PostId).HasColumnName("postId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Post).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_post_likes_post");

            entity.HasOne(d => d.User).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_post_likes_user");
        });

        modelBuilder.Entity<PostMedium>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("PK__PostMedi__D271B462122A355B");

            entity.Property(e => e.MediaId).HasColumnName("mediaId");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("fileName");
            entity.Property(e => e.MediaType)
                .HasMaxLength(20)
                .HasColumnName("mediaType");
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(1000)
                .HasColumnName("mediaUrl");
            entity.Property(e => e.PostId).HasColumnName("postId");

            entity.HasOne(d => d.Post).WithMany(p => p.PostMedia)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostMedia__postI__1C873BEC");
        });

        modelBuilder.Entity<PsychologyDiary>(entity =>
        {
            entity.HasKey(e => e.DiaryId).HasName("PK__Psycholo__E98C9C409614E0AE");

            entity.ToTable("PsychologyDiary");

            entity.Property(e => e.DiaryId).HasColumnName("diaryId");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdDate");
            entity.Property(e => e.JourneyId).HasColumnName("journeyId");
            entity.Property(e => e.Mood)
                .HasMaxLength(100)
                .HasColumnName("mood");

            entity.HasOne(d => d.Journey).WithMany(p => p.PsychologyDiaries)
                .HasForeignKey(d => d.JourneyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Psycholog__journ__37703C52");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__2ECD6E047967878B");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.ExpertId).HasColumnName("expertId");
            entity.Property(e => e.Feedback)
                .HasMaxLength(4000)
                .HasColumnName("feedback");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Expert).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ExpertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__expertId__59C55456");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__userId__5AB9788F");
        });

        modelBuilder.Entity<ReviewImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ReviewIm__336E9B557CD494A2");

            entity.ToTable("ReviewImage");

            entity.Property(e => e.ImageId).HasColumnName("imageId");
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .HasColumnName("image");
            entity.Property(e => e.ReviewId).HasColumnName("reviewId");

            entity.HasOne(d => d.Review).WithMany(p => p.ReviewImages)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewIma__revie__5F7E2DAC");
        });

        modelBuilder.Entity<RoleAdmin>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__RoleAdmi__CD98462A960618BD");

            entity.ToTable("RoleAdmin");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__Treatmen__D7AA58E82E6B6857");

            entity.Property(e => e.TreatmentId).HasColumnName("treatmentId");
            entity.Property(e => e.TreatmentName)
                .HasMaxLength(50)
                .HasColumnName("treatmentName");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserProf__CB9A1CFF1AD58C9B");

            entity.HasIndex(e => e.Email, "IDX_UserProfiles_Email");

            entity.HasIndex(e => e.PhoneNumber, "IDX_UserProfiles_Phone");

            entity.HasIndex(e => e.PhoneNumber, "UQ__UserProf__4849DA01A9D5BC27").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__UserProf__AB6E6164F4132049").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.AvatarFileName)
                .HasMaxLength(255)
                .HasColumnName("avatarFileName");
            entity.Property(e => e.AvatarType)
                .HasMaxLength(20)
                .HasColumnName("avatarType");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(1000)
                .HasColumnName("avatarUrl");
            entity.Property(e => e.BirthDate).HasColumnName("birthDate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.UserTypeId).HasColumnName("userTypeId");

            entity.HasOne(d => d.UserType).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserProfi__userT__1CBC4616");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UsertypeId).HasName("PK__UserType__A1CA1C6561E778D8");

            entity.Property(e => e.UsertypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("usertypeId");
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .HasColumnName("typeName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

-- Tạo cơ sở dữ liệu
-- DROP DATABASE GentleBlossom;
CREATE DATABASE GentleBlossom;
GO

USE GentleBlossom;
GO

-- Bảng lưu thông tin tài khoản đăng nhập
CREATE TABLE LoginUser (
    loginId INT IDENTITY PRIMARY KEY,
	typeUser TINYINT NOT NULL,
	userId INT NOT NULL UNIQUE,
    userName NVARCHAR(50) NOT NULL,
    password NVARCHAR(50) NOT NULL
);
GO

-- Bảng lưu thông tin Thai phụ
CREATE TABLE ExpectantMother (
    motherId INT IDENTITY PRIMARY KEY,
    fullName NVARCHAR(50) NOT NULL,
    birthDate DATE NOT NULL CHECK (birthDate < GETDATE()),
    phoneNumber NVARCHAR(10) UNIQUE NOT NULL CHECK (LEN(phoneNumber) = 10),
    email NVARCHAR(100) UNIQUE NOT NULL,
    avatar NVARCHAR(1000) NULL
);
GO

-- Bảng lưu hành trình mang thai
CREATE TABLE PregnancyJourney (
    journeyId INT IDENTITY PRIMARY KEY,
    motherId INT NOT NULL FOREIGN KEY REFERENCES ExpectantMother(motherId),
    dueDate DATE NOT NULL,
    weeksPregnant TINYINT NOT NULL,
    gender BIT NOT NULL
);
GO

-- Bảng lưu thông tin sức khỏe định kỳ
CREATE TABLE PeriodicHealth (
    healthId INT IDENTITY PRIMARY KEY,
    motherId INT NOT NULL FOREIGN KEY REFERENCES ExpectantMother(motherId),
    weeksPregnant TINYINT NOT NULL,			-- số tuần mang thai
    bloodPressure TINYINT NULL,				-- huyết áp
    waistCircumference DECIMAL(3,1) NULL,	-- vòng bụng
    weight DECIMAL(3,1) NULL,				-- cân nặng
    mood NVARCHAR(100) NULL,
    notes NVARCHAR(200) NULL,
    createdDate DATE DEFAULT GETDATE()
);
GO

-- Bảng lưu thông tin chuyên gia
CREATE TABLE Expert (
    expertId INT IDENTITY PRIMARY KEY,
    fullName NVARCHAR(100) NOT NULL,
    birthYear INT NULL,
    gender BIT NOT NULL,						-- 0: Nam; 1: Nữ
    phoneNumber NVARCHAR(10) NOT NULL,
    email NVARCHAR(100) UNIQUE NOT NULL,
    academicTitle NVARCHAR(50) NOT NULL,		-- Học vị
    position NVARCHAR(50) NOT NULL,				-- Chức vụ
    specialization NVARCHAR(100) NOT NULL,		-- Chuyên môn
    organization NVARCHAR(100) NOT NULL,		-- Đơn vị công tác
    avatar NVARCHAR(1000)
);
GO

-- Bảng danh mục loại điều trị
CREATE TABLE Treatments (
	treatmentId INT PRIMARY KEY IDENTITY,
	treatmentName NVARCHAR(50) NOT NULL,		-- Loại điều trị: Tâm lý, Thai sản, Sức khỏe.
);

-- Bảng phiếu theo dõi
CREATE TABLE MonitoringForm (
    formId INT IDENTITY PRIMARY KEY,
    expertId INT NOT NULL FOREIGN KEY REFERENCES Expert(expertId),
    motherId INT NOT NULL FOREIGN KEY REFERENCES ExpectantMother(motherId),
	treatmentId INT NOT NULL FOREIGN KEY REFERENCES Treatments(treatmentId),
    status TINYINT NOT NULL,				-- 1: Ổn định; 2: Cần theo dõi; 3: Nguy cấp
    notes NVARCHAR(500) NULL,
	createdDate DATE DEFAULT GETDATE()
);
GO

-- Bảng danh mục thể loại bài đăng
CREATE TABLE PostCategories (
	categoryId INT PRIMARY KEY IDENTITY,
	categoryName NVARCHAR(50) NOT NULL		-- 1: Dinh dưỡng, 2: tâm lý, 3: tập luyện, 4: Kiến thức phụ sản, 5: Trao đổi, hỏi đáp
);
GO

-- Bảng bài viết
CREATE TABLE Post (
    postId INT IDENTITY PRIMARY KEY,
    posterId INT NOT NULL,
    posterType TINYINT NOT NULL,		-- 1: Chuyên gia, 2: Thai phụ, 3: Quản trị
    categoryId INT FOREIGN KEY REFERENCES PostCategories(categoryId),
    content NVARCHAR(4000) NOT NULL,
    image NVARCHAR(1000) NULL,
    createdDate DATE DEFAULT GETDATE(),
	numberOfLike INT NOT NULL DEFAULT 0,
	numberOfComment INT NOT NULL DEFAULT 0
);
GO

CREATE TABLE PostImage (
    imageId INT PRIMARY KEY IDENTITY,
    postId INT NOT NULL FOREIGN KEY REFERENCES Post(postId),
	image NVARCHAR(1000) NOT NULL
);
GO

-- Bảng bình luận
CREATE TABLE CommentPost (
    commentId INT IDENTITY PRIMARY KEY,
    post INT NOT NULL FOREIGN KEY REFERENCES Post(postId),
    parentCommentId INT NULL,	-- Nếu là trả lời bình luận
    posterId INT NOT NULL,
    posterType INT NOT NULL,				-- 1: Chuyên gia, 2: Thai phụ, 3: Quản trị
    content NVARCHAR(4000) NOT NULL,
    image NVARCHAR(1000) NULL,
    commentDate DATE DEFAULT GETDATE()
);
GO

-- Bảng thông báo
CREATE TABLE Notifications (
	notificationId INT IDENTITY PRIMARY KEY,
	typeUser TINYINT NOT NULL,		-- 1: Chuyên gia, 2: Thai phụ, 3: Quản trị
	userId INT NOT NULL,
	content NVARCHAR(100) NOT NULL,
	createAt DATETIME DEFAULT GETDATE(),		-- Thời gian tạo thông báo
	isSeen BIT DEFAULT 0				-- Thông báo đã được xem hay chưa (TRUE/FALSE)
);
GO

-- Bảng nhật ký tâm lý
CREATE TABLE PsychologyDiary (
    diaryId INT IDENTITY PRIMARY KEY,
    motherId INT NOT NULL FOREIGN KEY REFERENCES ExpectantMother(motherId),
    createdDate DATE DEFAULT GETDATE(),
    mood TINYINT NOT NULL,			-- 1: Tốt, 2: Bình thường, 3: Không tốt
    content NVARCHAR(4000) NOT NULL
);
GO

-- Bảng đánh giá chuyên gia
CREATE TABLE Review (
    reviewId INT IDENTITY PRIMARY KEY,
    expertId INT NOT NULL FOREIGN KEY REFERENCES Expert(expertId),
    motherId INT NOT NULL FOREIGN KEY REFERENCES ExpectantMother(motherId),
    rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    feedback NVARCHAR(4000) NULL,
    createdDate DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE ReviewImage (
    imageId INT PRIMARY KEY IDENTITY,
    reviewId INT NOT NULL FOREIGN KEY REFERENCES Review(reviewId),
	image NVARCHAR(1000) NOT NULL
);
GO

-- Chức năng nhắn tin
CREATE TABLE ChatRoom (
    chatRoomId INT IDENTITY PRIMARY KEY,	-- Mã phòng chat
    chatRoomName NVARCHAR(100) NULL,		-- Tên phòng chat (chỉ dùng cho nhóm)
    isGroup BIT NOT NULL,					-- 0: Nhắn tin riêng, 1: Nhắn tin nhóm
    createdAt DATETIME DEFAULT GETDATE()	-- Thời gian tạo phòng
);
GO

CREATE TABLE ChatRoomUser (
    chatRoomUserId INT IDENTITY PRIMARY KEY,
    chatRoomId INT NOT NULL FOREIGN KEY REFERENCES ChatRoom(chatRoomId),
    participantId INT NOT NULL,                  -- Liên kết với người dùng (Thai phụ, Chuyên gia,...)
    participantType TINYINT NOT NULL,            -- 1: Thai phụ, 2: Chuyên gia, 3: Quản trị
    joinedAt DATETIME DEFAULT GETDATE()			 -- Thời gian tham gia phòng
);
GO

CREATE TABLE Message (
    messageId INT IDENTITY PRIMARY KEY,     -- Mã tin nhắn
    chatRoomId INT NOT NULL FOREIGN KEY REFERENCES ChatRoom(chatRoomId),
    senderId INT NOT NULL,                  -- Mã người gửi
    senderType TINYINT NOT NULL,            -- Loại người gửi (1: Thai phụ, 2: Chuyên gia,...)
    content NVARCHAR(2000) NULL,			-- Nội dung tin nhắn
	hasAttachment BIT DEFAULT 0,			-- Tin nhắn có chứa tệp đính kèm hay không
    sentAt DATETIME DEFAULT GETDATE(),      -- Thời gian gửi tin nhắn
    isRead BIT DEFAULT 0                    -- Trạng thái đã đọc (0: chưa đọc, 1: đã đọc)
);
GO

-- Thêm bảng để lưu tệp tin hoặc liên kết tệp tin
CREATE TABLE MessageAttachment (
    attachmentId INT IDENTITY PRIMARY KEY,
    messageId INT NOT NULL FOREIGN KEY REFERENCES Message(messageId),
    fileName NVARCHAR(255),
    filePath NVARCHAR(1000),		-- Đường dẫn lưu file
    fileType NVARCHAR(50)			 -- Loại tệp (vd: image/png, video/mp4)
);
GO

--------------------------------------- ADD INDEX ------------------------------------------------------
-- Bảng Message
CREATE INDEX IX_Message_ChatRoomId_SentAt ON Message(chatRoomId, sentAt);
CREATE INDEX IX_Message_SenderId_IsRead ON Message(senderId, isRead);

-- Bảng ChatRoomUser
CREATE INDEX IX_ChatRoomUser_participantId ON ChatRoomUser(participantId);

-- Bảng LoginUser
CREATE INDEX IX_LoginUser_userName ON LoginUser(userName);

-- Bảng ThaiPhu
CREATE INDEX IX_ExpectantMother_phoneNumber ON ExpectantMother(phoneNumber);
CREATE INDEX IX_ExpectantMother_email ON ExpectantMother(email);

-- Bảng HanhTrinhMangThai
CREATE INDEX IX_PregnancyJourney_motherId ON PregnancyJourney(motherId);

-- Bảng SucKhoeDinhKy
CREATE INDEX IX_PeriodicHealth_motherId ON PeriodicHealth(motherId);
CREATE INDEX IX_PeriodicHealth_weeksPregnant ON PeriodicHealth(weeksPregnant);

-- Bảng ChuyenGia
CREATE INDEX IX_Expert_email ON Expert(email);
CREATE INDEX IX_Expert_specialization ON Expert(specialization);

-- Bảng PhieuTheoDoi
CREATE INDEX IX_MonitoringForm_motherId ON MonitoringForm(motherId);
CREATE INDEX IX_MonitoringForm_expertId ON MonitoringForm(expertId);
CREATE INDEX IX_MonitoringForm_status ON MonitoringForm(status);

-- Bảng Post
CREATE INDEX IX_Post_categoryId ON Post(categoryId);
CREATE INDEX IX_Post_posterId_posterType ON Post(posterId, posterType);
CREATE INDEX IX_Post_createdDate ON Post(createdDate);

-- Bảng CommentPost
CREATE INDEX IX_CommentPost_post ON CommentPost(post);
CREATE INDEX IX_CommentPost_parentCommentId ON CommentPost(parentCommentId);
CREATE INDEX IX_CommentPost_posterId_posterType ON CommentPost(posterId, posterType);

-- Bảng Notifications
CREATE INDEX IX_Notifications_userId_typeUser ON Notifications(userId, typeUser);
CREATE INDEX IX_Notifications_isSeen ON Notifications(isSeen);
CREATE INDEX IX_Notifications_createAt ON Notifications(createAt);

-- Bảng NhatKyTamLy
CREATE INDEX IX_PsychologyDiary_motherId ON PsychologyDiary(motherId);
CREATE INDEX IX_PsychologyDiary_createdDate ON PsychologyDiary(createdDate);

-- Bảng Review
CREATE INDEX IX_Review_expertId ON Review(expertId);
CREATE INDEX IX_Review_motherId ON Review(motherId);
CREATE INDEX IX_Review_createdDate ON Review(createdDate);

-- Bảng ChatRoomUser
CREATE INDEX IX_ChatRoomUser_chatRoomId ON ChatRoomUser(chatRoomId);
CREATE INDEX IX_ChatRoomUser_participantId_participantType ON ChatRoomUser(participantId, participantType);

-- Bảng Message
CREATE INDEX IX_Message_chatRoomId ON Message(chatRoomId);
CREATE INDEX IX_Message_senderId_senderType ON Message(senderId, senderType);
CREATE INDEX IX_Message_isRead ON Message(isRead);

-- Bảng MessageAttachment
CREATE INDEX IX_MessageAttachment_messageId ON MessageAttachment(messageId);
CREATE INDEX IX_MessageAttachment_fileType ON MessageAttachment(fileType);
﻿-- Tạo cơ sở dữ liệu
-- DROP DATABASE GentleBlossom;
CREATE DATABASE GentleBlossom;
GO

USE GentleBlossom;
GO

-- Bảng danh mục loại người dùng
CREATE TABLE UserTypes (
	usertypeId TINYINT PRIMARY KEY IDENTITY,
	typeName NVARCHAR(50) NOT NULL			-- 1: Admin, 2: Chuyên gia, 3: Người dùng
);
GO

-- Bảng lưu thông tin người dùng (3 loại người dùng: Admin, Chuyên gia, Thai phụ)
CREATE TABLE UserProfiles (
    userId INT IDENTITY PRIMARY KEY,
    fullName NVARCHAR(100) NOT NULL,
    birthDate DATE NOT NULL CHECK (birthDate < GETDATE()),
    phoneNumber NVARCHAR(10) UNIQUE NOT NULL CHECK (LEN(phoneNumber) = 10),
    email NVARCHAR(100) UNIQUE NOT NULL,
    avatar NVARCHAR(1000) NULL,
	gender BIT NOT NULL,	-- 0 là nữ, 1 là nam
	userTypeId TINYINT NOT NULL FOREIGN KEY REFERENCES UserTypes(usertypeId),
);
GO

-- Bảng lưu thông tin tài khoản đăng nhập
CREATE TABLE LoginUser (
    loginId INT IDENTITY PRIMARY KEY,
	userId INT UNIQUE NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    userName NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(100) NOT NULL
);
GO

-- Bảng lưu thông tin dành riêng cho chuyên gia
CREATE TABLE Expert (
    expertId INT IDENTITY PRIMARY KEY,
	userId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    academicTitle NVARCHAR(50) NOT NULL,		-- Học vị
    position NVARCHAR(50) NOT NULL,				-- Chức vụ
    specialization NVARCHAR(100) NOT NULL,		-- Chuyên môn
    organization NVARCHAR(200) NOT NULL,		-- Đơn vị công tác
);
GO

-- Bảng lưu chức vụ của Đội ngũ quản trị
CREATE TABLE RoleAdmin (
    roleId TINYINT IDENTITY PRIMARY KEY,
    roleName NVARCHAR(100) NOT NULL
);
GO

-- Bảng lưu chức vụ của Đội ngũ quản trị
CREATE TABLE Administrator (
    adminId INT IDENTITY PRIMARY KEY,
	userId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    roleId TINYINT NOT NULL FOREIGN KEY REFERENCES RoleAdmin(roleId),
);
GO

-- Bảng danh mục loại điều trị
CREATE TABLE Treatments (
	treatmentId INT PRIMARY KEY IDENTITY,
	treatmentName NVARCHAR(50) NOT NULL,		-- Loại điều trị: Tâm lý, Thai sản,...
);
GO

-- Bảng lưu hành trình sức khỏe
CREATE TABLE HealthJourney (
    journeyId INT IDENTITY PRIMARY KEY,
    userId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    treatmentId INT NOT NULL FOREIGN KEY REFERENCES Treatments(treatmentId),
    startDate DATE NOT NULL DEFAULT GETDATE(),
    dueDate DATE NULL,		-- Chỉ dành cho thai phụ
    endDate DATE NULL,		-- Ngày kết thúc điều trị
    status BIT NOT NULL		-- 0: Đang điều trị, 1: Đã điều trị
);
GO

-- Bảng lưu thông tin theo dõi sức khỏe thai kỳ định kỳ 
CREATE TABLE PeriodicHealth (
    healthId INT IDENTITY PRIMARY KEY,
    journeyId INT NOT NULL FOREIGN KEY REFERENCES HealthJourney(journeyId),
    weeksPregnant TINYINT NOT NULL,			-- số tuần mang thai
    bloodPressure TINYINT NULL,				-- huyết áp
    waistCircumference DECIMAL(3,1) NULL,	-- vòng bụng
    weight DECIMAL(3,1) NULL,				-- cân nặng
    mood NVARCHAR(100) NULL,				-- Tâm trạng
	genderBaby BIT NULL,
    notes NVARCHAR(500) NULL,
    createdDate DATE DEFAULT GETDATE()
);
GO

-- Bảng nhật ký tâm lý của thai phụ điều trị tâm lý
CREATE TABLE PsychologyDiary (
    diaryId INT IDENTITY PRIMARY KEY,
    journeyId INT NOT NULL FOREIGN KEY REFERENCES HealthJourney(journeyId),
    createdDate DATE DEFAULT GETDATE(),
    mood NVARCHAR(100) NOT NULL,
    content NVARCHAR(4000) NOT NULL
);
GO

-- Bảng thông tin kết nối giữa Người dùng và chuyên gia
CREATE TABLE ConnectionMedical (
    connectId INT IDENTITY PRIMARY KEY,
    expertId INT NOT NULL FOREIGN KEY REFERENCES Expert(expertId),
    userId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    journeyId INT NOT NULL FOREIGN KEY REFERENCES HealthJourney(journeyId),
	status TINYINT NOT NULL		-- 0: Chờ, 1: Đang tư vấn, 2: Hoàn thành, 3: Hủy bỏ
);
GO

-- Bảng phiếu theo dõi
CREATE TABLE MonitoringForm (
    formId INT IDENTITY PRIMARY KEY,
    expertId INT NOT NULL FOREIGN KEY REFERENCES Expert(expertId),
    journeyId INT NOT NULL FOREIGN KEY REFERENCES HealthJourney(journeyId),
    status TINYINT NOT NULL,				-- 1: Ổn định; 2: Cần theo dõi; 3: Nguy cấp
    notes NVARCHAR(500) NULL,
	createdDate DATE DEFAULT GETDATE()
);
GO

-- Bảng danh mục thể loại bài đăng
CREATE TABLE PostCategories (
	categoryId INT PRIMARY KEY IDENTITY,
	categoryName NVARCHAR(100) NOT NULL		-- 1: Dinh dưỡng, 2: tâm lý, 3: tập luyện, 4: Kiến thức phụ sản, 5: Trao đổi, hỏi đáp,...
);
GO

-- Bảng bài viết
CREATE TABLE Post (
    postId INT IDENTITY PRIMARY KEY,
    posterId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    categoryId INT NOT NULL FOREIGN KEY REFERENCES PostCategories(categoryId),
    content NVARCHAR(4000) NOT NULL,
    createdDate DATE DEFAULT GETDATE(),
	numberOfLike INT NOT NULL DEFAULT 0
);
GO

-- Bảng hình ảnh của bài viết
CREATE TABLE PostImage (
    imageId INT PRIMARY KEY IDENTITY,
    postId INT NOT NULL FOREIGN KEY REFERENCES Post(postId),
	image NVARCHAR(1000) NOT NULL
);
GO

-- Bảng bình luận
CREATE TABLE CommentPost (
    commentId INT IDENTITY PRIMARY KEY,
    postId INT NOT NULL FOREIGN KEY REFERENCES Post(postId),
	posterId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    parentCommentId INT NULL FOREIGN KEY REFERENCES CommentPost(commentId),		-- Nếu là một bình luận trả lời 1 bình luận khác
    content NVARCHAR(4000) NOT NULL,
    image NVARCHAR(1000) NULL,
    commentDate DATE DEFAULT GETDATE()
);
GO

-- Bảng thông báo
CREATE TABLE Notifications (
	notificationId INT IDENTITY PRIMARY KEY,
	userId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
	content NVARCHAR(200) NOT NULL,
	createAt DATETIME DEFAULT GETDATE(),		-- Thời gian tạo thông báo
	isSeen BIT DEFAULT 0				-- Thông báo đã được xem hay chưa (TRUE/FALSE)
);
GO

-- Bảng đánh giá chuyên gia
CREATE TABLE Review (
    reviewId INT IDENTITY PRIMARY KEY,
    expertId INT NOT NULL FOREIGN KEY REFERENCES Expert(expertId),
    userId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    feedback NVARCHAR(4000) NULL,
    createdDate DATETIME DEFAULT GETDATE()
);
GO

-- Bảng hình ảnh của các đánh giá
CREATE TABLE ReviewImage (
    imageId INT PRIMARY KEY IDENTITY,
    reviewId INT NOT NULL FOREIGN KEY REFERENCES Review(reviewId),
	image NVARCHAR(1000) NOT NULL
);
GO

-- Bảng thông tin phòng chat
CREATE TABLE ChatRoom (
    chatRoomId INT IDENTITY PRIMARY KEY,	-- Mã phòng chat
    chatRoomName NVARCHAR(100) NULL,		-- Tên phòng chat (chỉ dùng cho nhóm)
    isGroup BIT NOT NULL,					-- 0: Nhắn tin riêng, 1: Nhắn tin nhóm
    createdAt DATETIME DEFAULT GETDATE()	-- Thời gian tạo phòng
);
GO

-- Bảng người dùng trong phòng chat
CREATE TABLE ChatRoomUser (
    chatRoomUserId INT IDENTITY PRIMARY KEY,
    chatRoomId INT NOT NULL FOREIGN KEY REFERENCES ChatRoom(chatRoomId),
    participantId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    joinedAt DATETIME DEFAULT GETDATE()			 -- Thời gian tham gia phòng
);
GO

-- Bảng thông tin của một tin nhắn
CREATE TABLE Message (
    messageId INT IDENTITY PRIMARY KEY,     -- Mã tin nhắn
    chatRoomId INT NOT NULL FOREIGN KEY REFERENCES ChatRoom(chatRoomId),
    senderId INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(userId),
    content NVARCHAR(2000) NULL,			-- Nội dung tin nhắn
	hasAttachment BIT DEFAULT 0,			-- Tin nhắn có chứa tệp đính kèm hay không
    sentAt DATETIME DEFAULT GETDATE(),      -- Thời gian gửi tin nhắn
    isRead BIT DEFAULT 0                    -- Trạng thái đã đọc (0: chưa đọc, 1: đã đọc)
);
GO

-- Bảng lưu tệp tin hoặc liên kết tệp tin cho tin nhắn 
CREATE TABLE MessageAttachment (
    attachmentId INT IDENTITY PRIMARY KEY,
    messageId INT NOT NULL FOREIGN KEY REFERENCES Message(messageId),
    fileName NVARCHAR(255),
    filePath NVARCHAR(1000),		-- Đường dẫn lưu file
    fileType NVARCHAR(20),		 -- Loại tệp (vd: image/png, video/mp4)
	fileSize INT NULL  -- Kích thước file (tính bằng KB)
);
GO
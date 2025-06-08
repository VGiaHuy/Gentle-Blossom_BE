USE GentleBlossom;
GO

CREATE UNIQUE INDEX IDX_LoginUser_UserName ON LoginUser(userName);

CREATE INDEX IDX_UserProfiles_Phone ON UserProfiles(phoneNumber);
CREATE INDEX IDX_UserProfiles_Email ON UserProfiles(email);

CREATE INDEX IDX_HealthJourney_User ON HealthJourney(userId);
CREATE INDEX IDX_HealthJourney_Treatment ON HealthJourney(treatmentId);

CREATE INDEX IDX_PeriodicHealth_Journey ON PeriodicHealth(journeyId);

CREATE INDEX IDX_ConnectionMedical_User ON ConnectionMedical(userId);
CREATE INDEX IDX_ConnectionMedical_Expert ON ConnectionMedical(expertId);

CREATE INDEX IDX_Post_Category ON Post(categoryId);
CREATE INDEX IDX_Post_Poster ON Post(posterId);
CREATE INDEX IDX_Post_CreatedDate ON Post(createdDate);

CREATE INDEX IDX_Comment_Post ON CommentPost(postId);
CREATE INDEX IDX_Comment_Poster ON CommentPost(posterId);
CREATE INDEX IDX_Comment_ParentComment ON CommentPost(parentCommentId);

CREATE INDEX IDX_Message_Sender ON Message(senderId);

CREATE INDEX IDX_Notifications_User ON Notifications(userId);
CREATE INDEX IDX_Notifications_Status ON Notifications(isSeen);

CREATE INDEX IX_Message_ChatRoomId ON Message(chatRoomId);
CREATE INDEX IX_ChatRoomUser_ChatRoomId ON ChatRoomUser(chatRoomId);
CREATE INDEX IX_ChatRoomUser_ParticipantId ON ChatRoomUser(participantId);

USE GentleBlossom;
GO

ALTER TABLE LoginUser
ADD CONSTRAINT CK_LoginUser_typeUser CHECK (typeUser IN (1, 2, 3));

ALTER TABLE MonitoringForm
ADD CONSTRAINT CK_MonitoringForm_Status CHECK (Status IN (1, 2, 3));

ALTER TABLE CommentPost
ADD CONSTRAINT CK_CommentPost_posterType CHECK (posterType IN (1, 2, 3));

ALTER TABLE Notifications
ADD CONSTRAINT CK_Notifications_typeUser CHECK (typeUser IN (1, 2, 3));

ALTER TABLE PsychologyDiary
ADD CONSTRAINT CK_PsychologyDiary_mood CHECK (mood IN (1, 2, 3));



-- Tự động xóa tệp đính kèm khi xóa tin nhắn
ALTER TABLE MessageAttachment
ADD CONSTRAINT FK_MessageAttachment_Message
FOREIGN KEY (MessageId)
REFERENCES Message(MessageId)
ON DELETE CASCADE;	

-- Tự động xóa thành viên khi xóa phòng chat
ALTER TABLE ChatRoomUser
ADD CONSTRAINT FK_ChatRoomUser_ChatRoom
FOREIGN KEY (ChatRoomId)
REFERENCES ChatRoom(ChatRoomId)
ON DELETE CASCADE;

-- Tự động xóa tin nhắn khi xóa phòng chat
ALTER TABLE Message
ADD CONSTRAINT FK_Message_ChatRoom 
FOREIGN KEY (ChatRoomId) 
REFERENCES ChatRoom(ChatRoomId)
ON DELETE CASCADE;

ALTER TABLE PregnancyJourney
ADD CONSTRAINT FK_PregnancyJourney_ExpectantMother
FOREIGN KEY (motherId)
REFERENCES ExpectantMother(motherId)
ON DELETE CASCADE;

ALTER TABLE PeriodicHealth
ADD CONSTRAINT FK_PeriodicHealth_ExpectantMother
FOREIGN KEY (motherId)
REFERENCES ExpectantMother(motherId)
ON DELETE CASCADE;

ALTER TABLE PsychologyDiary
ADD CONSTRAINT FK_PsychologyDiary_ExpectantMother
FOREIGN KEY (motherId)
REFERENCES ExpectantMother(motherId)
ON DELETE CASCADE;

ALTER TABLE MonitoringForm
ADD CONSTRAINT FK_MonitoringForm_ExpectantMother
FOREIGN KEY (motherId)
REFERENCES ExpectantMother(motherId)
ON DELETE CASCADE;

ALTER TABLE Review
ADD CONSTRAINT FK_Review_Expert
FOREIGN KEY (expertId)
REFERENCES Expert(expertId)
ON DELETE CASCADE;

ALTER TABLE Review
ADD CONSTRAINT FK_Review_ExpectantMother
FOREIGN KEY (motherId)
REFERENCES ExpectantMother(motherId)
ON DELETE CASCADE;

ALTER TABLE Post
ADD CONSTRAINT FK_Post_Poster
FOREIGN KEY (posterId)
REFERENCES ExpectantMother(motherId)
ON DELETE CASCADE;

ALTER TABLE CommentPost
ADD CONSTRAINT FK_CommentPost_Post
FOREIGN KEY (post)
REFERENCES Post(PostId)
ON DELETE CASCADE;

ALTER TABLE PostImage
ADD CONSTRAINT FK_PostImage_Post_Cascade
FOREIGN KEY (postId)
REFERENCES Post(PostId)
ON DELETE CASCADE;

ALTER TABLE ReviewImage
ADD CONSTRAINT FK_ReviewImage_Review_Cascade
FOREIGN KEY (reviewId)
REFERENCES Review(reviewId)
ON DELETE CASCADE;

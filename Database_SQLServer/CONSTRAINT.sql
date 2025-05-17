USE GentleBlossom;
GO

ALTER TABLE MonitoringForm
ADD CONSTRAINT CK_MonitoringForm_Status CHECK (Status IN (1, 2, 3));

-- Thêm UNIQUE constraint tránh trùng lặp dữ liệu theo journeyId + createdDate
ALTER TABLE PeriodicHealth ADD CONSTRAINT UQ_PeriodicHealth UNIQUE (journeyId, createdDate);


---- Ràng buộc: Đảm bảo `participantId` tồn tại trong `UserProfiles`
--ALTER TABLE ChatRoomUser 
--ADD CONSTRAINT fk_ChatRoomUser_participant 
--FOREIGN KEY (participantId) REFERENCES UserProfiles(userId) 
--ON DELETE CASCADE;

---- Ràng buộc: Đảm bảo `chatRoomId` tồn tại trong `ChatRoom`
--ALTER TABLE ChatRoomUser 
--ADD CONSTRAINT fk_ChatRoomUser_chatRoom 
--FOREIGN KEY (chatRoomId) REFERENCES ChatRoom(chatRoomId) 
--ON DELETE CASCADE;

---- Ràng buộc: Không cho phép một `participantId` tham gia cùng một `chatRoomId` nhiều lần
--ALTER TABLE ChatRoomUser 
--ADD CONSTRAINT uq_ChatRoomUser UNIQUE (participantId, chatRoomId);

---- Thêm CHECK constraint để đảm bảo tin nhắn phải có nội dung hoặc file đính kèm:
--ALTER TABLE Message ADD CONSTRAINT CHK_Message_ContentOrAttachment 
--CHECK (content IS NOT NULL OR hasAttachment = 1);


---- Tự động xóa tệp đính kèm khi xóa tin nhắn
--ALTER TABLE MessageAttachment
--ADD CONSTRAINT FK_MessageAttachment_Message
--FOREIGN KEY (MessageId)
--REFERENCES Message(MessageId)
--ON DELETE CASCADE;	


---- Tự động xóa tin nhắn khi xóa phòng chat
--ALTER TABLE Message
--ADD CONSTRAINT FK_Message_ChatRoom 
--FOREIGN KEY (ChatRoomId) 
--REFERENCES ChatRoom(ChatRoomId)
--ON DELETE CASCADE;

--ALTER TABLE Review
--ADD CONSTRAINT FK_Review_Expert
--FOREIGN KEY (expertId)
--REFERENCES Expert(expertId)
--ON DELETE CASCADE;

--ALTER TABLE PostImage
--ADD CONSTRAINT FK_PostImage_Post_Cascade
--FOREIGN KEY (postId)
--REFERENCES Post(PostId)
--ON DELETE CASCADE;

--ALTER TABLE ReviewImage
--ADD CONSTRAINT FK_ReviewImage_Review_Cascade
--FOREIGN KEY (reviewId)
--REFERENCES Review(reviewId)
--ON DELETE CASCADE;

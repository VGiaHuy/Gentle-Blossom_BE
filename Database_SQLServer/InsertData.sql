USE GentleBlossom;
GO

-- Bảng danh mục loại người dùng
INSERT INTO UserTypes (typeName) VALUES
(N'Admin'), (N'Chuyên gia'), (N'Người dùng');

-- Bảng thông tin người dùng - Role Người dùng
-- Bảng thông tin người dùng - Role Người dùng
INSERT INTO UserProfiles (fullName, birthDate, phoneNumber, email, avatarUrl, avatarType, avatarFileName, gender, userTypeId) VALUES
(N'Nguyễn Thị Anh', '1990-05-20', '0901234567', 'nguyenthianh@gmail.com', NULL, NULL, NULL, 0, 3),
(N'Đỗ Ngọc Hân', '1995-11-29', '0934455778', 'ngochan@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Hoàng Xuân Mai', '1993-08-12', '0923456789', 'hoangxuanmai@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Lê Thị Ánh Đào', '2000-01-05', '0934567890', 'anhdao@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Phạm Hoàng Xuân', '1982-03-21', '0945678901', 'hoangxuan@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Bùi Thị Mỹ Lê', '1995-07-18', '0956789012', 'myle@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Ngô Tuyết Sang', '1988-06-25', '0967890123', 'tuyetsang@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Võ Thị Hoàng', '1991-04-14', '0978901234', 'thihoang@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Hoàng Thị Mỹ Linh', '1991-07-11', '0912233556', 'mylinh@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Hồ Thị Mai', '1997-09-08', '0990123456', 'thimai@gmail.com',  NULL, NULL, NULL, 0, 3),
(N'Trương Mỹ Tú', '1997-09-08', '0990123451', 'truongmytu@gmail.com',  NULL, NULL, NULL, 0, 3),

-- Bảng thông tin người dùng - Role Chuyên gia
(N'Nguyễn Hoàng Bảo', '1992-11-22', '0901122334', 'hoangbao@gmail.com',  NULL, NULL, NULL, 1, 2),
(N'Phạm Thị Kim Ngân', '1994-03-15', '0912233445', 'kimngan@gmail.com',  NULL, NULL, NULL, 0, 2),
(N'Lê Văn Thành', '1989-06-10', '0923344556', 'vanthanh@gmail.com',  NULL, NULL, NULL, 1, 2),
(N'Trần Thị Thu Hương', '1991-07-18', '0934455667', 'thuhuong@gmail.com',  NULL, NULL, NULL, 0, 2),
(N'Bùi Minh Khang', '1987-02-28', '0945566778', 'minhkhang@gmail.com',  NULL, NULL, NULL, 1, 2),
(N'Đặng Thị Thanh Trúc', '1988-05-05', '0956677889', 'thanhtruc@gmail.com',  NULL, NULL, NULL, 0, 2),
(N'Nguyễn Nhật Hào', '1993-09-12', '0967788990', 'nhathao@gmail.com',  NULL, NULL, NULL, 1, 2),
(N'Võ Thanh Phương', '1985-01-20', '0978899001', 'thanhphuong@gmail.com',  NULL, NULL, NULL, 1, 2),
(N'Trịnh Hoàng Lan', '1990-12-03', '0989900112', 'hoanglan@gmail.com',  NULL, NULL, NULL, 0, 2),
(N'Lý Thị Thanh Mai', '1995-08-21', '0990011223', 'thanhmai@gmail.com',  NULL, NULL, NULL, 0, 2),

-- Bảng thông tin người dùng - Role Admin
(N'Hà Thị Như Quỳnh', '1998-03-22', '0978899112', 'nhuquynh@gmail.com',  NULL, NULL, NULL, 0, 1),
(N'Phạm Hoài Nam', '1984-05-19', '0989900223', 'hoainam@gmail.com',  NULL, NULL, NULL, 1, 1),
(N'Cao Thị Thanh Hà', '1996-10-27', '0990011334', 'thanhha@gmail.com',  NULL, NULL, NULL, 0, 1);

UPDATE LoginUser SET password = '$2a$11$TRwDGWltYO45ocHwH1cezOH0hUY0XRHYoViBN2DUPjGK9kV6V91E2' where userId = 1

-- Bảng lưu thông tin tài khoản đăng nhập
INSERT INTO LoginUser (userId, userName, password) VALUES
(1, '0901234567', '$2a$11$St4t8gfc2ez2KtwFrtlTme2NDPrf9Kdui0usAgI/OAZ/hViq18V6W'),
(2, '0934455778', '$2a$11$0eMa73RBB8EFxfyAYv9I6.rXaz5KSFZyo8t0guoGSyHJBkDo5.g72'),
(3, '0923456789', '$2a$11$Zck4xphDC9gbpsYo7UuMmO00B6W/F2QjjpgKH8Qw55NQzgbnEwLDO'),
(4, '0934567890', '$2a$11$tHOrD0A90x5D.yxMsdeVVeHQFx3z2b2vIuMxsbXNnnxweExyDRDKK'),
(5, '0945678901', '$2a$11$rq09xNlPvY2cN94EocUv9Og3cdeN1yBIKrTETPQe/MntLbUMw51S2'),
(6, '0956789012', '$2a$11$i2pr0U4MYZGl4Z.Ck82JHUBgbokj7A4hrvQSfrjc8.UTnbd2FLc2a'),
(7, '0967890123', '$2a$11$N2tSmMRiUzUZ3nhCg5EK7.s9mfv41Bkp/HWqMe4Bywwz4LmnFMsoK'),
(8, '0978901234', '$2a$11$yTva13PfM5WIFvRUdHqip.yj5CjQML4E5odC9evdnHdQ7M2Ybjiha'),
(9, '0912233556', '$2a$11$aCXtOP7KYMgKfLBCor8U2OaDqQeZ2l7EImfFy5NPEayx5MzzyWrSu'),
(10, '0990123456', '$2a$11$CKuRLs2/N8Ak6osS3hMvveGBnGT7VNdO7gTzzAcUjHqyzZSlcOJhW'),
(11, '0990123451', '$2a$11$pFHP32XvZWWo3DmoXJ.CROD3hG2oYPmZKmFpa4Tm4Z1IaaUcu3wz6'),
(12, '0901122334', '$2a$11$tKrFiDDqumMH1B6sfrL9NOXEu6hC6depyZ.Q5r01sfR2FlIn6PbKq'),
(13, '0912233445', '$2a$11$l2UDIPK2XUQWyMkNwRM79uhwXpJevQeRfB6NDDhYkhxBdmiDR8PAy'),
(14, '0923344556', '$2a$11$RvjauXTijhVW4QozUWTb/OzixBPgzA7/0QIz4Qu/7Y0R8bFZKOKtS'),
(15, '0934455667', '$2a$11$KfsxC2RzxEm7C6jvIrxjmOYohk45E6b.8KTqOQiE4qmb6JeyDkTkK'),
(16, '0945566778', '$2a$11$npCt8lbe3GCvOUHYqbmWeu/PIUhO1n7FLvYmwVOoB/Bd7LDY5b0EO'),
(17, '0956677889', '$2a$11$O3rETbEgn6g7xsb7toSh5OjUciVa3IxyUpSG7vgX2eBHyRVLt0YSS'),
(18, '0967788990', '$2a$11$bbau8fZXJzVZHe7EZyZ0XehKtCQOaCW5NsdJGeATq/mMnPbqdAGg2'),
(19, '0978899001', '$2a$11$z/MZ8ZBFzYqINz1pufyTrOnpQ7WWfQpE0gxGiHcz9AHPRfP/Vz.6a'),
(20, '0989900112', '$2a$11$IrE/hCaZ3aNWLKpZfIxGOeW.y2HZKSkqPaHOqZtwXRRvKy/Zo1IQ6'),
(21, '0990011223', '$2a$11$8mKcMlQzrtfSwMCJ.F92yO//hIT/aY93sOO0v8AfWjLFFZxpIPe8S'),
(22, '0978899112', '$2a$11$82T.hMGdiYIbMcKyGj9.VurRxY18uEHwwGggVSkLRsyc3f7RRnDaW'),
(23, '0989900223', '$2a$11$jHz5ABHRZYyTh6.YWZdLfu3IoQLo.0p1GznoZaP3NGB9jo7tXZaG6'),
(24, '0990011334', '$2a$11$K2UdflCGepnA6UuCgh3ZQeRmEDhaqI4UF5c2qDgs5AZ5mKxhWJMUe');

UPDATE LoginUser SET password = '$2a$11$js9xClDdi.Jyqj2TPXQJve.HBhaR3SQbMR6FP51Bb18QBov4AqQvO' where userId = 3
UPDATE LoginUser SET password = '$2a$11$JQ77z.0Rl71JdOriBt2EL.f4p.NfW50hffLSrCFJ9FJIR4/HGn/1O' where userId = 4
UPDATE LoginUser SET password = '$2a$11$J6Jjz2HnNlwWxS3zaiyOSOmUMeW4C6K3x1vM36gS7lhbFnLpOvvou' where userId =5 
UPDATE LoginUser SET password = '$2a$11$6G.iga4/qIkwAJwVQ7fzZeoXKZh43pL1cj6KxsRJLMRk92YBLwD46' where userId = 6
UPDATE LoginUser SET password = '$2a$11$Nd4SdlXMZ7y..qn4WCmZ3Ou4QTXAAeAa9hXbQGuccWVuRmiOg2JS6' where userId = 7
UPDATE LoginUser SET password = '$2a$11$xucrdVlzOAm5drRA3JZYk.1IjX4bkmoWj//JnYnzTQisFehDmXcwO' where userId = 8
UPDATE LoginUser SET password = '$2a$11$7xjeIknGFYWblw3CSb1JpeAL2IIV9SJIKoyqr.suyClrdTzmQgH2O' where userId = 9
UPDATE LoginUser SET password = '$2a$11$NzR0brkxUu3a2GpqXJca1eLKFyIUCFMfRxvUyI2c8TskL79DRhPWO' where userId = 10
UPDATE LoginUser SET password = '$2a$11$QJYdDeY8vOViuzKiwHx9p.aN/rpTmn8y7ard6OBPgyz8DimijJz36' where userId = 11
UPDATE LoginUser SET password = '$2a$11$eYP8MfPYCnRyvexfYuN6gORk/L6KcaZmVk3H9vxuGknf5Qhy2syfm' where userId = 12
UPDATE LoginUser SET password = '$2a$11$czvfWsqIoPzVcTsyV9LiPumM/wZmR3GqyqPgxWzdIs3UArzUtU34O' where userId = 13
UPDATE LoginUser SET password = '$2a$11$561PW.IFqhmwjqLXAsvq6uYIfgNWM/X.9eRx7zwtknqk46oZOI7Qi' where userId = 14
UPDATE LoginUser SET password = '$2a$11$bFKzd.ek.BplG6tB6uke3uEjJqFSlxWFOFqN4I7BZCuojxJV6JXj.' where userId = 15
UPDATE LoginUser SET password = '$2a$11$Xd7Z5kLaz4gsVHbqICFmI.xBhWQiO37oBDYUs8eudiYB9lK.e1Hse' where userId = 16
UPDATE LoginUser SET password = '$2a$11$W//kchZVKBhXHTntx4DRWu418wAeST04BPvhCmidyLtN1iJxcFrtW' where userId = 17
UPDATE LoginUser SET password = '$2a$11$8qPZDasrRASfDNAeVwNyouP02/ET6iaRt2VYd68Xvp6KgXdFD56mm' where userId = 18
UPDATE LoginUser SET password = '$2a$11$ErcbgUZ6yMH6MTrrRf.W0uqvrVrYqQHPbfE/9K/UoU3obEXkhpNce' where userId = 19
UPDATE LoginUser SET password = '$2a$11$McoxYgzkz47CAFSPTOqQpux0gRyxldYekXa7Odo92d5BkClgGEQ6a' where userId = 20
UPDATE LoginUser SET password = '$2a$11$mTTRq7lXvCw/gUwXpKPWw.R/3LiJbebHrf8KgvQ4yD3Y4y.IPm.kK' where userId = 21
UPDATE LoginUser SET password = '$2a$11$awRYAxkqnmEOFW.6HCKt/ONkbdX8NGAOuJUFpnycViKEyipG/m//u' where userId = 22
UPDATE LoginUser SET password = '$2a$11$hO2TowD33kLkxZIj2efpQe0OmFrn2un0lwku1h5rwHBxx2DSqRASe' where userId = 23
UPDATE LoginUser SET password = '$2a$11$jcbJJL.8h8AUZAPjPvdD5uEK8PoXykXGUm2kACrbCCw8mB5ewlPlu' where userId = 24


-- Bảng thông tin chuyên gia
INSERT INTO Expert (userId, academicTitle, position, specialization, organization) VALUES
(12, N'Tiến sĩ', N'Phó trưởng khoa', N'Sản khoa', N'Bệnh viện Gaya Việt Hàn'),
(13, N'Phó Giáo sư', N'Trưởng khoa', N'Tâm lý học', N'Đại học Y Hà Nội'),
(14, N'Tiến sĩ', N'Bác sĩ chuyên khoa', N'Nhi khoa', N'Bệnh viện Nhi TW'),
(15, N'Bác sĩ', N'Bác sĩ chuyên khoa', N'Sản khoa', N'Bệnh viện Phụ sản quốc tế Sài Gòn'),
(16, N'Thạc sĩ', N'Bác sĩ chuyên khoa', N'Nhi khoa', N'Bệnh viện Đại học Y Dược'),
(17, N'Bác sĩ', N'Bác sĩ chuyên khoa', N'Sản khoa', N'Bệnh viện Nhi TW'),
(18, N'Thạc sĩ', N'Bác sĩ chuyên khoa', N'Tâm lý học', N'Bệnh viện Vinmec'),
(19, N'Bác sĩ', N'Bác sĩ chuyên khoa', N'Sản khoa', N'Bệnh viện Quốc tế Hạnh Phúc'),
(20, N'Tiến sĩ', N'Phó trưởng khoa', N'Nhi khoa', N'Bệnh viện phụ sản Trung Ương'),
(21, N'Thạc sĩ', N'Bác sĩ chuyên khoa', N'Tâm lý học', N'Bệnh viện phụ sản Hà Nội');


-- Bảng chức vụ của đội ngũ quản trị
INSERT INTO RoleAdmin (roleName) VALUES
(N'Kỹ thuật viên'), (N'Quản trị hệ thống');


-- Bảng thông tin quản trị viên
INSERT INTO Administrator (userId, roleId) VALUES
(22, 1),
(23, 1),
(24, 2);


-- Bảng danh mục loại điều trị
INSERT INTO Treatments (treatmentName) VALUES
(N'Tâm lý'), (N'Thai sản'), (N'Dinh dưỡng'), (N'Vật lý trị liệu'), (N'Chăm sóc hậu sản');


-- Bảng hành trình sức khỏe
INSERT INTO HealthJourney (userId, treatmentId, startDate, dueDate, endDate, status) VALUES
(1, 2, '2024-10-01', '2025-07-30', NULL, 0), -- Thai phụ, chưa sinh
(2, 1, '2025-01-05', NULL, '2025-02-10', 1), -- Đã điều trị xong
(3, 3, '2025-01-10', NULL, NULL, 0), -- Đang điều trị
(4, 4, '2024-11-15', NULL, NULL, 0), -- Đang điều trị
(5, 2, '2024-10-03', '2025-07-15', NULL, 0), -- Thai phụ, chưa sinh
(6, 1, '2024-04-25', NULL, '2025-02-20', 1), -- Đã điều trị xong
(7, 1, '2024-12-01', NULL, NULL, 0), -- Đang điều trị
(8, 3, '2024-02-05', NULL, '2025-03-01', 1), -- Đã điều trị xong
(9, 5, '2025-02-10', NULL, NULL, 0), -- Đang điều trị
(10, 2, '2024-10-15', '2025-06-20', NULL, 0); -- Thai phụ, chưa sinh


-- Bảng theo dõi sức khỏe thai kỳ
INSERT INTO PeriodicHealth (journeyId, weeksPregnant, bloodPressure, waistCircumference, weight, mood, genderBaby, notes, createdDate) VALUES
(1, 4, 110, 85.5, 58.2, N'Tốt', NULL, N'Sức khỏe cảm giác tốt hơn', '2024-11-01'),
(1, 8, 120, 90.0, 60.0, N'Mệt mỏi', NULL, N'Cảm thấy mệt mỏi, rất khó hoạt động', '2024-12-01'),
(1, 12, 115, 93.0, 62.5, N'Thoải mái', 1, N'Cảm giác dễ chịu hơn và có em bé có vẻ lớn nhanh', '2025-01-01'),
(1, 16, 125, 95.0, 64.7, N'Có chút mệt mỏi', 1, N'Cảm giác cơ thể rất uể oải', '2025-02-01'),

(5, 6, 112, 87.0, 59.0, N'Tốt', NULL, N'Chưa xác định giới tính em bé', '2024-10-03'),
(5, 10, 118, 91.5, 61.2, N'Hơi căng thẳng', NULL, N'Đôi lúc cảm thấy lo lắng', '2024-11-03'),
(5, 14, 119, 94.0, 63.5, N'Thoải mái', 0, N'Bác sĩ xác nhận bé gái', '2024-12-03'),
(5, 18, 123, 96.5, 66.0, N'Bình thường', 0, N'Không có triệu chứng bất thường', '2025-01-03'),
(5, 22, 126, 99.0, 67.3, N'Hơi mệt', 0, N'Cảm thấy hơi chóng mặt khi đứng dậy', '2025-02-03'),

(10, 5, 114, 86.5, 58.5, N'Tốt', NULL, N'Không có dấu hiệu lạ', '2024-10-15'),
(10, 9, 121, 89.5, 60.7, N'Buồn ngủ nhiều', NULL, N'Cảm thấy thèm ngủ cả ngày', '2024-11-15'),
(10, 13, 117, 92.5, 62.8, N'Khá ổn', NULL, N'Có vẻ em bé đang phát triển tốt', '2024-12-15'),
(10, 17, 124, 95.3, 65.2, N'Khó tiêu', 1, N'Bác sĩ xác nhận bé trai, hơi khó tiêu khi ăn', '2025-01-15'),
(10, 21, 122, 97.5, 67.0, N'Mệt mỏi', 1, N'Thay đổi hormone khiến tâm trạng không ổn định', '2025-02-15');


-- Bảng nhật ký tâm lý
INSERT INTO PsychologyDiary (journeyId, createdDate, mood, content) VALUES
(2, '2025-01-05', N'Căng thẳng', N'Gần đây tôi cảm thấy lo lắng...'),
(2, '2025-01-15', N'Mệt mỏi tinh thần', N'Tôi cảm giác rất mệt mỏi và sụt cân'),
(2, '2025-01-25', N'Tinh thần tốt hơn', N'Cảm giác lo lắng giảm xuống và tôi đã ăn uống ngon miệng hơn'),
(2, '2025-02-10', N'Sức khỏe dần ổn định', N'Tôi cảm giác tinh thần đã tốt hơn và không cần dùng đến thuốc'),

-- Nhật ký tâm lý của journeyId = 6 (đã điều trị xong)
(6, '2024-04-26', N'Lo lắng', N'Mới bắt đầu điều trị, tôi cảm thấy bất an về tình trạng của mình'),
(6, '2024-06-10', N'Căng thẳng', N'Tôi thấy áp lực khi điều trị kéo dài và không thấy kết quả ngay'),
(6, '2024-10-05', N'Cảm giác tốt hơn', N'Tôi bắt đầu thấy có tiến triển và tâm trạng cũng ổn định hơn'),
(6, '2025-02-20', N'Vui mừng', N'Tôi đã hoàn thành điều trị và sức khỏe ổn định, cảm thấy tự tin hơn'),

-- Nhật ký tâm lý của journeyId = 7 (đang điều trị)
(7, '2024-12-02', N'Cảm giác hoang mang', N'Tôi không biết liệu phương pháp điều trị này có hiệu quả không'),
(7, '2025-01-15', N'Tinh thần suy giảm', N'Có những ngày tôi cảm thấy cực kỳ mệt mỏi và mất ngủ'),
(7, '2025-02-10', N'Hơi lạc quan', N'Tôi cảm thấy có chút tiến triển nhưng vẫn cần thời gian để hồi phục');


-- Bảng kết nối chuyên gia và người dùng
INSERT INTO ConnectionMedical (expertId, userId, journeyId, status) VALUES
(1, 1, 1, 1),
(4, 5, 5, 1),
(8, 10, 10, 1),
(2, 2, 2, 2),
(7, 6, 6, 2),
(10, 7, 7, 1);


-- Bảng phiếu theo dõi
INSERT INTO MonitoringForm (expertId, journeyId, status, notes, createdDate) VALUES
(1, 1, 2, N'Cần theo dõi huyết áp', '2024-11-03'),
(1, 1, 1, N'Sức khỏe ổn định', '2024-12-03'),
(1, 1, 1, N'Cần ăn uống cẩn thận hơn', '2025-01-03'),
(1, 1, 2, N'Chú ý hạn chế ăn đồ dầu mỡ', '2025-02-03');


-- Bảng danh mục thể loại bài đăng
INSERT INTO PostCategories (categoryName) VALUES
(N'Dinh dưỡng'), (N'Tâm lý'), (N'Tập luyện'), (N'Kiến thức phụ sản'), (N'Trao đổi, hỏi đáp');


-- Bảng bài viết
INSERT INTO Post (posterId, categoryId, content, createdDate, numberOfLike) VALUES
(12, 1, N'Trong suốt thai kỳ, dinh dưỡng là yếu tố quan trọng giúp thai nhi phát triển khỏe mạnh. Một chế độ ăn đầy đủ dưỡng chất sẽ cung cấp đủ vitamin và khoáng chất cần thiết. Các nhóm thực phẩm như rau xanh, cá, thịt nạc, trứng, sữa rất quan trọng. Ngoài ra, mẹ bầu cũng cần hạn chế các loại thực phẩm chế biến sẵn và đồ uống có cồn để đảm bảo thai nhi phát triển tốt.', '2025-02-01', 10),
(13, 2, N'Giai đoạn mang thai khiến nhiều mẹ bầu cảm thấy căng thẳng và lo lắng. Việc thay đổi hormone có thể ảnh hưởng đến tâm trạng và gây mất ngủ. Để giảm căng thẳng, mẹ bầu nên dành thời gian nghỉ ngơi, tập thiền hoặc yoga. Bên cạnh đó, trò chuyện với gia đình hoặc chuyên gia tâm lý cũng giúp mẹ bầu cảm thấy an tâm hơn.', '2025-02-02', 15),
(14, 3, N'Tập luyện trong thai kỳ giúp mẹ bầu duy trì sức khỏe và giảm nguy cơ mắc các biến chứng. Một số bài tập an toàn như đi bộ nhẹ nhàng, bơi lội hoặc yoga sẽ giúp cơ thể linh hoạt hơn. Tuy nhiên, mẹ bầu nên tránh các bài tập có cường độ cao hoặc tác động mạnh đến vùng bụng.', '2025-02-03', 8),
(15, 4, N'Nhiều mẹ bầu băn khoăn về các dấu hiệu chuyển dạ và cách xử lý khi sắp sinh. Những dấu hiệu thường gặp gồm đau bụng dưới, ra nước ối và xuất hiện cơn gò tử cung. Khi có các dấu hiệu này, mẹ bầu nên đến bệnh viện để được kiểm tra kịp thời. Việc tìm hiểu trước kiến thức về sinh con sẽ giúp mẹ bầu chuẩn bị tốt hơn.', '2025-02-04', 12),
(16, 5, N'Rất nhiều mẹ bầu có thắc mắc về chế độ ăn uống, luyện tập và cách chăm sóc sức khỏe khi mang thai. Một số câu hỏi phổ biến như: "Có nên uống cà phê không?", "Cần bổ sung canxi từ tháng thứ mấy?", "Nên đi khám thai bao nhiêu lần?" đều là những vấn đề quan trọng. Tìm hiểu kỹ thông tin từ bác sĩ sẽ giúp mẹ bầu có quyết định đúng đắn.', '2025-02-05', 20),
(17, 1, N'Chế độ ăn uống khoa học sẽ giúp mẹ bầu và thai nhi luôn khỏe mạnh. Bổ sung thực phẩm giàu protein, sắt và canxi rất quan trọng. Hạn chế thực phẩm chế biến sẵn, ăn nhiều rau xanh và uống đủ nước sẽ giúp thai nhi phát triển tốt hơn.', '2025-02-06', 5),
(18, 2, N'Những thay đổi về hormone khiến tâm lý mẹ bầu dễ bị ảnh hưởng. Một số mẹ có thể vui vẻ, nhưng cũng có những mẹ cảm thấy buồn bã và căng thẳng. Việc tham gia các lớp học tiền sản, trò chuyện với bạn bè và duy trì lối sống lành mạnh sẽ giúp mẹ bầu giữ tinh thần thoải mái.', '2025-02-07', 14),
(19, 3, N'Bài tập Kegel rất quan trọng giúp mẹ bầu cải thiện cơ sàn chậu, hạn chế són tiểu sau sinh. Mẹ bầu có thể tập luyện bằng cách siết chặt cơ sàn chậu trong 5-10 giây rồi thả lỏng. Kiên trì tập luyện mỗi ngày sẽ giúp quá trình sinh nở dễ dàng hơn.', '2025-02-08', 7),
(20, 4, N'Ở tam cá nguyệt thứ ba, mẹ bầu cần chuẩn bị sẵn sàng cho việc sinh nở. Tìm hiểu các dấu hiệu chuyển dạ, chuẩn bị đồ sơ sinh và học cách chăm sóc bé sau khi sinh là những điều quan trọng. Sự chuẩn bị chu đáo giúp mẹ bầu giảm bớt lo lắng trước ngày lâm bồn.', '2025-02-09', 9),
(21, 5, N'Trao đổi kinh nghiệm với những mẹ bầu khác sẽ giúp giảm bớt lo lắng trong thai kỳ. Việc tham gia vào các hội nhóm, diễn đàn trực tuyến là cách tốt để học hỏi và tìm kiếm sự hỗ trợ từ cộng đồng.', '2025-02-10', 11),
(12, 1, N'Một chế độ ăn uống lành mạnh giúp mẹ bầu kiểm soát cân nặng hợp lý, tránh tiểu đường thai kỳ và đảm bảo thai nhi phát triển tốt nhất. Các thực phẩm giàu omega-3 như cá hồi, hạnh nhân rất quan trọng cho sự phát triển trí não của bé.', '2025-02-11', 13),
(13, 2, N'Nhiều mẹ bầu cảm thấy lo lắng về việc sinh con và làm mẹ lần đầu. Điều quan trọng là giữ tinh thần lạc quan, tham gia các lớp học tiền sản và chuẩn bị tốt về mặt tâm lý. Sự hỗ trợ từ gia đình và bạn bè sẽ giúp mẹ bầu tự tin hơn.', '2025-02-12', 16),
(14, 3, N'Việc vận động nhẹ nhàng mỗi ngày giúp mẹ bầu giảm đau lưng, cải thiện tuần hoàn máu và ngủ ngon hơn. Đi bộ, yoga hoặc bơi lội là những bài tập an toàn mà mẹ bầu nên duy trì trong suốt thai kỳ.', '2025-02-13', 9),
(15, 4, N'Tháng cuối thai kỳ là giai đoạn quan trọng, mẹ bầu cần theo dõi chặt chẽ các dấu hiệu của cơ thể. Việc chuẩn bị sẵn sàng túi đồ đi sinh và có kế hoạch di chuyển đến bệnh viện là điều cần thiết.', '2025-02-14', 17),
(16, 5, N'Nhiều mẹ bầu thắc mắc về việc có nên tiêm phòng khi mang thai hay không. Các loại vắc xin như vắc xin cúm, uốn ván được khuyến nghị để bảo vệ sức khỏe cho cả mẹ và bé.', '2025-02-15', 10),
(17, 1, N'Bổ sung canxi từ tháng thứ 4 trở đi là điều quan trọng giúp bé phát triển hệ xương chắc khỏe. Các thực phẩm như sữa, hải sản, rau lá xanh là nguồn cung cấp canxi dồi dào.', '2025-02-16', 12),
(18, 2, N'Tâm lý lo lắng trong giai đoạn mang thai là điều bình thường. Mẹ bầu cần duy trì chế độ sinh hoạt lành mạnh, tránh căng thẳng quá mức và chia sẻ với người thân khi gặp khó khăn.', '2025-02-17', 18),
(19, 3, N'Tập thể dục đều đặn trong thai kỳ giúp mẹ bầu dễ dàng kiểm soát cân nặng, giảm nguy cơ mắc tiểu đường thai kỳ và giúp quá trình sinh nở thuận lợi hơn.', '2025-02-18', 14),
(20, 4, N'Trong ba tháng cuối, thai nhi phát triển rất nhanh. Mẹ bầu cần bổ sung đầy đủ dinh dưỡng, ngủ đủ giấc và chuẩn bị tâm lý sẵn sàng cho việc sinh nở.', '2025-02-19', 15);


-- Bảng media bài viết
INSERT INTO PostMedia (postId, mediaUrl, mediaType)
VALUES 
(1, 'https://drive.google.com/uc?id=1abcDEFghiJKLmnoPQRsTuvWXyz123456', 'image'),
(1, 'https://drive.google.com/uc?id=2defGHIjklMNOpqrSTuvWXYzabc789012', 'video'),
(2, 'https://drive.google.com/uc?id=3ghiJKLmnoPQRsTuvWXyzabcDEF345678', 'image'),
(2, 'https://drive.google.com/uc?id=4jklMNOpqrSTuvWXYzabcDEFghi901234', 'image');



-- Bảng bình luận
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(1, 2, NULL, N'Bài viết rất hay! Mong có thêm nhiều thông tin bổ ích.', NULL, '2025-02-24');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(1, 3, 1, N'Đồng ý! Đọc xong mình có thêm nhiều kiến thức.', NULL, '2025-02-24');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(1, 4, 1, N'Cảm ơn tác giả! Mong chờ bài viết tiếp theo.', NULL, '2025-02-24');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(2, 5, NULL, N'Làm thế nào để giảm căng thẳng khi mang thai?', NULL, '2025-02-24');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(2, 6, 5, N'Thử nghe nhạc thư giãn hoặc tập yoga nhẹ nhàng.', NULL, '2025-02-24');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(2, 7, 5, N'Thiền cũng là một cách rất tốt để giữ tinh thần thoải mái.', NULL, '2025-02-24');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(3, 8, NULL, N'Nên ăn gì để bổ sung canxi khi mang thai?', NULL, '2025-02-25');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(3, 9, 8, N'Cá hồi, sữa tươi và rau xanh đều rất tốt cho mẹ bầu.', NULL, '2025-02-25');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(3, 10, 8, N'Ngoài ra, có thể bổ sung canxi qua viên uống theo chỉ định của bác sĩ.', NULL, '2025-02-25');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(4, 11, NULL, N'Làm thế nào để bé trong bụng phát triển tốt?', NULL, '2025-02-25');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(4, 1, 11, N'Dinh dưỡng đầy đủ và tinh thần thoải mái là yếu tố quan trọng.', NULL, '2025-02-25');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(4, 2, 11, N'Mẹ bầu có thể tập thể dục nhẹ để tăng cường sức khỏe.', NULL, '2025-02-25');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(5, 3, NULL, N'Mình bị đau lưng nhiều khi mang thai, có cách nào giảm không?', NULL, '2025-02-26');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(5, 4, 3, N'Dùng gối ôm khi ngủ hoặc massage lưng nhẹ nhàng sẽ giúp giảm đau.', NULL, '2025-02-26');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(5, 5, 3, N'Tập yoga bầu cũng giúp giảm đau lưng hiệu quả.', NULL, '2025-02-26');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(6, 6, NULL, N'Có nên uống sữa bầu không hay chỉ cần ăn uống đủ chất?', NULL, '2025-02-26');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(6, 7, 6, N'Mình uống sữa bầu vì nó bổ sung đủ dưỡng chất cần thiết.', NULL, '2025-02-26');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(6, 8, 6, N'Chỉ cần ăn uống đủ chất là không nhất thiết phải uống sữa bầu.', NULL, '2025-02-26');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(7, 9, NULL, N'Bài tập nào giúp giảm sưng chân khi mang thai?', NULL, '2025-02-27');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(7, 10, 9, N'Nâng chân cao khi ngủ và massage nhẹ sẽ giúp giảm sưng.', NULL, '2025-02-27');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(7, 11, 9, N'Đi bộ nhẹ nhàng mỗi ngày giúp máu lưu thông tốt hơn.', NULL, '2025-02-27');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(8, 1, NULL, N'Có mẹ nào bị rạn da không? Cách nào giúp giảm rạn hiệu quả?', NULL, '2025-02-27');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(8, 2, 1, N'Dùng dầu dừa hoặc kem dưỡng ẩm từ sớm sẽ hạn chế rạn da.', NULL, '2025-02-27');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(8, 3, 1, N'Uống nhiều nước và bổ sung vitamin E cũng giúp da đàn hồi tốt hơn.', NULL, '2025-02-27');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(9, 4, NULL, N'Bổ sung DHA thế nào là hợp lý trong thai kỳ?', NULL, '2025-02-28');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(9, 5, 4, N'Nên ăn cá hồi và hạnh nhân, rất tốt cho sự phát triển của bé.', NULL, '2025-02-28');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(9, 6, 4, N'Mình uống viên DHA theo chỉ định của bác sĩ.', NULL, '2025-02-28');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(10, 7, NULL, N'Có mẹ nào bị mất ngủ khi mang thai không?', NULL, '2025-02-28');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(10, 8, 7, N'Mình cũng vậy, uống sữa ấm trước khi ngủ giúp mình dễ ngủ hơn.', NULL, '2025-02-28');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(10, 9, 7, N'Thử tập thể dục nhẹ vào buổi tối, giúp thư giãn cơ thể.', NULL, '2025-02-28');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(11, 10, NULL, N'Có mẹ nào biết thực đơn ăn uống mỗi ngày sao cho khoa học không?', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(11, 11, 10, N'Mình hay ăn sáng với sữa chua, ngũ cốc và trái cây.', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(11, 1, 10, N'Mỗi bữa mình luôn bổ sung rau xanh và đạm từ cá.', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(12, 2, NULL, N'Thông tin về lịch tiêm vắc-xin khi mang thai rất hữu ích!', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(12, 3, 2, N'Mình đã tiêm đầy đủ theo lịch bác sĩ hướng dẫn.', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(12, 4, 2, N'Nên tiêm vắc-xin ho gà, uốn ván để bảo vệ cả mẹ và bé.', NULL, '2025-03-01');
-- postId 13: Chế độ ăn uống khi mang thai
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(13, 5, NULL, N'Mình nên ăn bao nhiêu bữa mỗi ngày để đảm bảo dinh dưỡng?', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(13, 6, 5, N'Nên ăn 5-6 bữa nhỏ trong ngày thay vì 3 bữa lớn.', NULL, '2025-03-01');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(13, 7, 5, N'Bổ sung protein từ cá và thịt nạc sẽ tốt hơn cho bé.', NULL, '2025-03-01');
-- postId 14: Lợi ích của yoga cho mẹ bầu
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(14, 8, NULL, N'Yoga giúp giảm đau lưng và thư giãn rất tốt.', NULL, '2025-03-02');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(14, 9, 8, N'Mình tập yoga từ tháng thứ 4 và thấy cơ thể khỏe hơn.', NULL, '2025-03-02');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(14, 10, 8, N'Nên chọn bài tập phù hợp, tránh các động tác quá sức.', NULL, '2025-03-02');
-- postId 15: Triệu chứng thường gặp khi mang thai
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(15, 11, NULL, N'Mình hay bị đau đầu nhẹ, có mẹ nào bị giống vậy không?', NULL, '2025-03-03');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(15, 1, 11, N'Uống nhiều nước và nghỉ ngơi sẽ giúp giảm đau đầu.', NULL, '2025-03-03');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(15, 2, 11, N'Có thể do thiếu máu, nên bổ sung thêm sắt.', NULL, '2025-03-03');
-- postId 16: Kiểm tra sức khỏe định kỳ cho mẹ bầu
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(16, 3, NULL, N'Bao lâu nên đi khám thai một lần nhỉ?', NULL, '2025-03-04');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(16, 4, 3, N'Mỗi tháng nên khám một lần, 3 tháng cuối thì khám thường xuyên hơn.', NULL, '2025-03-04');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(16, 5, 3, N'Tuần 12, 22 và 32 là những mốc quan trọng cần kiểm tra kỹ.', NULL, '2025-03-04');
-- postId 17: Cách giảm stress khi mang thai
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(17, 6, NULL, N'Mình hay lo lắng về sinh nở, làm sao để giảm căng thẳng?', NULL, '2025-03-05');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(17, 7, 6, N'Thử tập thở sâu, nghe nhạc nhẹ nhàng và đi dạo.', NULL, '2025-03-05');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(17, 8, 6, N'Nói chuyện với người thân hoặc chuyên gia tâm lý cũng giúp ích.', NULL, '2025-03-05');
-- postId 18: Chuẩn bị đồ dùng cho mẹ và bé trước khi sinh
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(18, 9, NULL, N'Mình nên chuẩn bị đồ đi sinh từ tháng thứ mấy?', NULL, '2025-03-06');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(18, 10, 9, N'Tháng thứ 7 hoặc 8 là thời điểm tốt để chuẩn bị.', NULL, '2025-03-06');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(18, 11, 9, N'Nhớ mang theo đầy đủ giấy tờ và đồ dùng cho bé.', NULL, '2025-03-06');
-- postId 19: Cách chăm sóc làn da cho mẹ bầu
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(19, 1, NULL, N'Có ai bị nám da khi mang thai không? Làm sao để giảm?', NULL, '2025-03-07');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(19, 2, 1, N'Dùng kem chống nắng mỗi ngày và uống đủ nước rất quan trọng.', NULL, '2025-03-07');
INSERT INTO CommentPost (postId, posterId, parentCommentId, content, image, commentDate) VALUES
(19, 3, 1, N'Bổ sung vitamin C từ cam, chanh giúp da sáng hơn.', NULL, '2025-03-07');
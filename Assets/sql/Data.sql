-- Active: 1709197271422@@127.0.0.1@1433@Android@dbo
use android;
INSERT INTO [Categories] (Name, Description)
VALUES
('Rock', 'Nhạc rock là một thể loại nhạc có nguồn gốc từ Hoa Kỳ vào cuối những năm 1940 và đầu những năm 1950. Nhạc rock thường được kết hợp với các nhạc cụ điện tử như guitar điện, bass điện và trống điện.'),
('Pop', 'Nhạc pop là một thể loại nhạc phổ biến với giai điệu dễ nghe và lời bài hát dễ nhớ. Nhạc pop thường được kết hợp với các nhạc cụ như guitar, trống, piano và keyboard.'),
('Hip hop', 'Nhạc hip hop là một thể loại nhạc có nguồn gốc từ Hoa Kỳ vào cuối những năm 1970. Nhạc hip hop thường được kết hợp với các nhạc cụ như trống, DJ và rap.'),
('R&B', 'Nhạc R&B là một thể loại nhạc kết hợp giữa nhạc pop và nhạc blues. Nhạc R&B thường được kết hợp với các nhạc cụ như guitar, piano, trống và giọng hát.'),
('Country', 'Nhạc đồng quê là một thể loại nhạc có nguồn gốc từ Hoa Kỳ vào cuối những năm 1800. Nhạc đồng quê thường được kết hợp với các nhạc cụ như guitar, banjo, mandolin và fiddle.'),
('Classical', 'Nhạc cổ điển là một thể loại nhạc có nguồn gốc từ châu Âu vào thế kỷ 17. Nhạc cổ điển thường được kết hợp với các nhạc cụ như violin, cello, piano và dàn nhạc giao hưởng.'),
('Jazz', 'Nhạc jazz là một thể loại nhạc có nguồn gốc từ Hoa Kỳ vào đầu những năm 1900. Nhạc jazz thường được kết hợp với các nhạc cụ như kèn saxophone, kèn trumpet, piano và trống.'),
('Latin', 'Nhạc Latin là một thể loại nhạc có nguồn gốc từ châu Mỹ Latin. Nhạc Latin thường được kết hợp với các nhạc cụ như guitar, bongos, timbales và maracas.'),
('Electronic', 'Nhạc điện tử là một thể loại nhạc sử dụng các nhạc cụ điện tử. Nhạc điện tử thường được kết hợp với các nhạc cụ như synthesizer, drum machine và sequencer.'),
('Metal', 'Nhạc metal là một thể loại nhạc có nguồn gốc từ Hoa Kỳ vào cuối những năm 1960. Nhạc metal thường được kết hợp với các nhạc cụ như guitar điện, bass điện, trống và giọng hát mạnh mẽ.'),
('Folk', 'Nhạc folk là một thể loại nhạc truyền thống thường được hát với guitar hoặc các nhạc cụ dân gian khác. Nhạc folk thường được kết hợp với các bài hát về tình yêu, thiên nhiên và cuộc sống.'),
('Opera', 'Opera là một thể loại nhạc kịch có nguồn gốc từ Ý vào thế kỷ 16. Opera thường được kết hợp với các ca sĩ, vũ công và dàn nhạc.'),
('Musical', 'Musical là một thể loại nhạc kịch có nguồn gốc từ Hoa Kỳ vào thế kỷ 19. Musical thường được kết hợp với các ca sĩ, vũ công, dàn nhạc và kịch bản.'),
('Soundtrack', 'Soundtrack là một album nhạc được sáng tác cho một bộ phim hoặc chương trình truyền hình. Soundtrack thường được kết hợp với các bài hát gốc và các bài hát cover.'),
('World music', 'World music là một thể loại nhạc bao gồm các thể loại nhạc từ khắp nơi trên thế giới. World music thường được kết hợp với các nhạc cụ và phong cách hát từ các nền văn hóa khác nhau.');

INSERT INTO [Users] ([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
VALUES ('android','android', 'android@gmail.com', 'android@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0),
       ('superandroid', 'superandroid', 'superandroid@gmail.com', 'superandroid@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0),
       ('hiep', 'hiep', 'hiep8am@gmail.com', 'hiep8am@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0),
       ('duy', 'duy', 'codedaovoiduy@gmail.com', 'codedaovoiduy@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0),
       ('quang', 'quang', 'mail1@gmail.com', 'mail1@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0),
       ('youzo', 'quan', 'mail2@gmail.com', 'mail2@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0),
       ('chien', 'chien','mail3@gmail.com', 'mail3@gmail.com', 'True', 'security_stamp', 'concurrency_stamp', 'False', 'False', 'False', 0);
INSERT INTO Tracks (Name, FileName, Description, Artwork, AuthorId, UploadAt, IsPrivate, ListenCount, LikeCount, CommentCount)
VALUES
    (N'7 Years', N'7_Years.mp3', 'Lukas Graham', N'7-Years.jpg', 4, '2023-10-10 09:10:10', 'true', 0, 0, 2),
    (N'Buồn thì cứ khóc đi', N'Buon_Thi_Cu_Khoc_Di.mp3', 'Lynk Lee', N'Buon-Thi-Cu-Khoc-Di.jpg', 4, '2023-10-10 09:11:01', 'false', 0, 1, 2),
    (N'Đã lỡ yêu em nhiều', N'Da_Lo_Yeu_Em_Nhieu.mp3', 'JustaTee', N'Da-Lo-Yeu-Em-Nhieu.jpg', 4, '2023-10-10 09:12:12', 'true', 0, 0, 0),
    (N'Nandemonaiya ', N'Nandemonaiya.mp3', '1012', N'default-artwork.jpg', 4, '2023-10-10 09:13:32', 'false', 0, 0, 0),
    (N'Rap chậm thôi', N'Rap_Cham_Thoi.mp3', 'MCK', N'Rap-Cham-Thoi.jpg', 7, '2023-06-21 10:14:12', 'false', 0, 0, 0),
    (N'Thủ Đô Cypher', N'Thu_Do_Cypher.mp3', 'MCK, LowG', N'Thu-Do-Cypher.jpg', 4, '2023-10-10 09:40:12', 'false', 0, 0, 0);

INSERT INTO Tracks_Categories (TrackId, CategoryId)
VALUES
    (1, 7),
    (2, 1),
    (3, 6),
    (3, 4),
    (4, 5),
    (5, 3),
    (6, 3);


INSERT INTO LikeTrack ( [UserId], [TrackId])
VALUES (1, 1),
        (1, 2),
        (2, 1),
        (2, 3),
        (3, 2),
        (4, 1);

INSERT INTO Comments ([Content], [CommentAt], [IsEdited], [TrackId], [UserId], [IsReported]) 
VALUES ('Hay quá', '2023-10-10 09:10:12', 'true', 2, 1, 0),
('Tuyệt vời ạ', '2023-10-10 09:11:42', 'false', 2, 1, 0),
('This song is make my childhoods back. Thank you sir!', '2023-10-10 10:24:20', 'false', 2, 2, 0),
('Bài này hay quá, yeah yeah', '2023-10-10 10:11:20', 'false', 1, 1, 0),
('Nhạc này còn hơi kén người nghe quá bro', '2023-10-10 19:50:12', 'true', 2 , 3, 0),
('như c', '2023-10-10 10:11:20', 'false', 1, 3, 1),
('report tôi đi', '2023-10-10 19:50:12', 'true', 2 , 3, 1);

INSERT INTO [Roles] ([Name], [NormalizedName])
VALUES ('Admin', 'admin'),
       ('SuperAdmin', 'superadmin'),
       ('User', 'user');

INSERT INTO [UserRoles] ([UserId], [RoleId])
VALUES (1, 1),
       (2, 2),
       (3, 3),
       (4, 3),
       (5, 3),
       (6, 3),
       (7, 3);
INSERT INTO [Playlists] (Name, CreatedAt, IsPrivate, AuthorId, Description, ArtWork, Tags, LikeCount, RepostCount, TrackCount)
VALUES 
        ('TopBXH', '2023-10-11 10:11:00', 'false', 1, 'Playlist thịnh hành', 'default-artwork.jpg', '#BXH, #Top', 0, 0, 1),
        ('TopMoiNhat', '2023-10-11 10:11:00', 'false', 1, 'Playlist  mới nhất', 'default-artwork.jpg', '#Top, #MoiNhat', 0, 10, 1),
        ('NhacCuaHiep', '2023-10-11 10:11:00', 'true', 3, 'Playlist theo gu hiep', 'default-artwork.jpg', NULL, 1, 1, 0),
        ('NhacCuaDuy', '2023-10-11 10:11:00', 'false', 4, 'Playlist theo gu duy', 'default-artwork.jpg', NULL, 1, 10, 0),
        ('NhacCuaQuang', '2023-10-11 10:11:00', 'true', 5, 'Playlist theo gu quang', 'default-artwork.jpg', NULL,1, 0, 0),
        ('NhacCuaYouzo', '2023-10-11 10:11:00', 'true', 6, 'Playlist theo gu youzo', 'default-artwork.jpg', NULL, 0, 0, 0),
        ('NhacCuaChien', '2023-10-11 10:11:00', 'true', 7, 'Playlist theo gu chien', 'default-artwork.jpg', NULL, 1, 2, 0);
INSERT INTO LikePlaylist (UserId, PlaylistId)
VALUES 
        (1, 3),
        (1, 2),
        (1, 3),
        (1, 4),
        (1, 5),
        (1, 6),
        (1, 4),
        (2, 4),
        (2, 3),
        (2, 2),
        (2, 5),
        (2, 6),
        (2, 5),
        (3, 5),
        (3, 2),
        (3, 3),
        (3, 6),
        (4, 3),
        (4, 4),
        (4, 3),
        (4, 2),
        (4, 5),
        (4, 6),
        (5, 2),
        (5, 2),
        (5, 5),
        (5, 6),
        (5, 3),
        (6, 4),
        (6, 5),
        (6, 2),
        (6, 3),
        (6, 6),
        (7, 2),
        (7, 4),
        (7, 2),
        (7, 6),
        (7, 5);
INSERT INTO [Tracks_Playlists] (PlaylistId, TrackId)
VALUES 
        (1, 1),
        (1, 2),
        (1, 5),
        (1, 6),
        (2, 1),
        (2, 2),
        (2, 3),
        (3, 2),
        (3, 3),
        (4, 2),
        (4, 3),
        (4, 4),
        (4, 6),
        (5, 6),
        (6, 5),
        (7, 2),
        (7, 6); 
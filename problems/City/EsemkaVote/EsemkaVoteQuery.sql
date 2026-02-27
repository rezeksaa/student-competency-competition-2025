create database [EsemkaVote]
go
use [EsemkaVote]
go

create table [Division](
	[Id]				int				primary key	identity		NOT NULL,
	[Name]			varchar(50)		NOT NULL,
)

create table [Employee](
	[Id]				int				primary key	identity		NOT NULL,
	[Name]				varchar(50)		NOT NULL,
	[Email]				varchar(50)		NOT NULL,
	[Password]			varchar(50)		NOT NULL,
	[Photo]				varchar(50),
	[DivisionId]		int				NOT NULL,
	foreign key([DivisionId])				references [Division]([id])
)

create table [VotingHeader](
	[Id]				int				primary key	identity		NOT NULL,
	[Name]				varchar(50)		NOT NULL,
	[Description]		varchar(200)	NOT NULL,
	[StartDate]			date			NOT NULL,
	[EndDate]			date			NOT NULL,
)

create table [VotingCandidate] (
	[Id]				int				primary key	identity		NOT NULL,
	[VotingHeaderId]	int				NOT NULL,
	[EmployeeId]		int				NOT NULL,
	foreign key([VotingHeaderId])			references [VotingHeader]([id]),
	foreign key([EmployeeId])				references [Employee]([id]),
)

create table [VotingDetail](
	[Id]				int				primary key	identity		NOT NULL,
	[VotedCandidateId]	int				NOT NULL,
	[EmployeeId]		int				NOT NULL,
	[Reason]			varchar(255),
	foreign key([VotedCandidateId])			references [VotingCandidate]([id]),
	foreign key([EmployeeId])				references [Employee]([id]),
)


insert into [Division] ([Name]) values ('Frontend');
insert into [Division] ([Name]) values ('Backend');
insert into [Division] ([Name]) values ('UI/UX');
insert into [Division] ([Name]) values ('Marketing');

insert into [Employee] ([Name], [Email], [Password], [Photo], [DivisionId]) 
values
('John Doe', 'john.doe@example.com', 'password1', NULL, 1),
('Jane Smith', 'jane.smith@example.com', 'password2', NULL, 2),
('Michael Johnson', 'michael.johnson@example.com', 'password3', NULL, 3),
('Emily Davis', 'emily.davis@example.com', 'password4', 'Emily.jpg', 1),
('William Wilson', 'william.wilson@example.com', 'password5', 'William.jpg', 2),
('Olivia Martinez', 'olivia.martinez@example.com', 'password6', NULL, 3),
('James Anderson', 'james.anderson@example.com', 'password7', NULL, 1),
('Sophia Taylor', 'sophia.taylor@example.com', 'password8', 'Sophia.jpg', 2),
('Benjamin Thomas', 'benjamin.thomas@example.com', 'password9', 'Benjamin.jpg', 3),
('Emma Hernandez', 'emma.hernandez@example.com', 'password10', NULL, 1),
('Alexander Moore', 'alexander.moore@example.com', 'password11', 'Alexander.jpg', 2),
('Mia Lee', 'mia.lee@example.com', 'password12', NULL, 3),
('Ethan Walker', 'ethan.walker@example.com', 'password13', 'Ethan.jpg', 1),
('Ava Perez', 'ava.perez@example.com', 'password14', NULL, 2),
('Michael Hill', 'michael.hill@example.com', 'password15', 'Michael.jpg', 3),
('Isabella Scott', 'isabella.scott@example.com', 'password16', NULL, 1),
('Daniel Green', 'daniel.green@example.com', 'password17', NULL, 2),
('Camila Adams', 'camila.adams@example.com', 'password18', NULL, 3),
('Matthew Baker', 'matthew.baker@example.com', 'password19', NULL, 1),
('Charlotte Campbell', 'charlotte.campbell@example.com', 'password20', NULL, 2),
('Aiden Rivera', 'aiden.rivera@example.com', 'password21', NULL, 3),
('Luna Nguyen', 'luna.nguyen@example.com', 'password22', NULL, 1),
('David Mitchell', 'david.mitchell@example.com', 'password23', NULL, 2),
('Ella Gray', 'ella.gray@example.com', 'password24', NULL, 3),
('Logan Carter', 'logan.carter@example.com', 'password25', NULL, 1),
('Avery James', 'avery.james@example.com', 'password26', NULL, 2),
('Jackson Perez', 'jackson.perez@example.com', 'password27', NULL, 3),
('Amelia Evans', 'amelia.evans@example.com', 'password28', NULL, 1),
('Joseph Torres', 'joseph.torres@example.com', 'password29', NULL, 2),
('Harper Diaz', 'harper.diaz@example.com', 'password30', NULL, 3),
('Samuel Wright', 'samuel.wright@example.com', 'password31', NULL, 1),
('Evelyn Lopez', 'evelyn.lopez@example.com', 'password32', NULL, 2),
('Gabriel Hill', 'gabriel.hill@example.com', 'password33', NULL, 3),
('Sofia Murphy', 'sofia.murphy@example.com', 'password34', NULL, 1),
('Dylan Nelson', 'dylan.nelson@example.com', 'password35', NULL, 2),
('Victoria King', 'victoria.king@example.com', 'password36', NULL, 3),
('Carter Ward', 'carter.ward@example.com', 'password37', NULL, 1),
('Madison Powell', 'madison.powell@example.com', 'password38', NULL, 2),
('Ryan Wright', 'ryan.wright@example.com', 'password39', NULL, 3),
('Chloe Wood', 'chloe.wood@example.com', 'password40', NULL, 1),
('Nathan Price', 'nathan.price@example.com', 'password41', NULL, 2),
('Zoe Cooper', 'zoe.cooper@example.com', 'password42', NULL, 3),
('Christian Rivera', 'christian.rivera@example.com', 'password43', NULL, 1),
('Penelope Ward', 'penelope.ward@example.com', 'password44', NULL, 2),
('Isaac Bell', 'isaac.bell@example.com', 'password45', NULL, 3),
('Hannah Diaz', 'hannah.diaz@example.com', 'password46', NULL, 1),
('Liam Bailey', 'liam.bailey@example.com', 'password47', NULL, 2),
('Aria Rivera', 'aria.rivera@example.com', 'password48', NULL, 3),
('Lucas Parker', 'lucas.parker@example.com', 'password49', NULL, 1),
('Grace Kelly', 'grace.kelly@example.com', 'password50', NULL, 2),
('Mason Murphy', 'mason.murphy@example.com', 'password51', NULL, 3),
('Aubrey Powell', 'aubrey.powell@example.com', 'password52', NULL, 1),
('Henry Wright', 'henry.wright@example.com', 'password53', NULL, 2),
('Stella Evans', 'stella.evans@example.com', 'password54', NULL, 3),
('Sebastian Turner', 'sebastian.turner@example.com', 'password55', NULL, 1),
('Aurora Baker', 'aurora.baker@example.com', 'password56', NULL, 2),
('Wyatt Reed', 'wyatt.reed@example.com', 'password57', NULL, 3),
('Leah Cook', 'leah.cook@example.com', 'password58', NULL, 1),
('Hunter Bell', 'hunter.bell@example.com', 'password59', NULL, 2),
('Nova Garcia', 'nova.garcia@example.com', 'password60', NULL, 3);


insert into [VotingHeader] ([Name], [Description], [StartDate], [EndDate]) values
('Best Employee 2022', 'Welcome to the Employee of the Year 2022 voting! Celebrate outstanding dedication and achievement by casting your vote for the most deserving nominee.', '2022-01-01', '2023-01-01'),
('Best Employee 2023', 'Welcome to the Employee of the Year 2023 voting! Celebrate outstanding dedication and achievement by casting your vote for the most deserving nominee.', '2023-01-01', '2024-01-01'),
('Best Employee 2024', 'Welcome to the Employee of the Year 2024 voting! Celebrate outstanding dedication and achievement by casting your vote for the most deserving nominee.', '2024-01-01', '2025-01-01'),
('Best Employee 2025', 'Welcome to the Employee of the Year 2025 voting! Celebrate outstanding dedication and achievement by casting your vote for the most deserving nominee.', '2025-01-01', '2026-01-01');

insert into [VotingCandidate] ([VotingHeaderId], [EmployeeId]) values
(1,4),
(1,5),
(1,8),
(1,9),
(2,11),
(2,13),
(2,15),
(3,20),
(3,22);

insert into [VotingDetail]([VotedCandidateId], [EmployeeId], [Reason]) values
(1, 1, 'She dedication to their work is unparalleled, consistently going above and beyond what is expected.'),
(1, 3, 'She demonstrate exceptional leadership skills, inspiring and guiding their colleagues to achieve their best.'),
(1, 5, 'Her strong work ethic serves as a shining example to the entire team.'),
(1, 7, 'She possess excellent problem-solving abilities, always finding innovative solutions to complex challenges.'),
(1, 9, 'Her attention to detail ensures that every task they undertake is completed to the highest standard.'),
(1, 11, 'She exhibit outstanding time management skills, effectively prioritizing tasks and meeting deadlines.'),
(1, 13, 'Her positive attitude creates a motivating and uplifting atmosphere in the workplace.'),
(1, 15, 'She is highly adaptable, thriving in fast-paced and ever-changing environments.'),
(1, 17, 'She consistently seek feedback and strive for self-improvement, showing a commitment to personal and professional growth.'),
(1, 19, 'She is dependable and reliable, colleagues can always count on them to deliver results.'),
(1, 21, 'She communicate effectively with both colleagues and clients, fostering strong relationships built on trust and respect.'),
(1, 23, 'She demonstrate a willingness to take on new challenges and responsibilities.'),
(1, 25, NULL),
(1, 25, NULL),
(2, 4, 'He excel at collaboration, working seamlessly with others to achieve common goals.'),
(2, 8, 'He take initiative and are proactive in identifying areas for improvement.'),
(2, 12, NULL),
(2, 16, NULL),
(2, 20, NULL),
(2, 24, NULL),
(3, 2, 'She exhibit a high level of professionalism in all aspects of their work.'),
(3, 6, 'She handle pressure with grace and composure, maintaining productivity even in challenging situations.'),
(3, 10, NULL),
(3, 14, NULL),
(3, 18, NULL),
(3, 22, NULL),
(4, 26, NULL),
(4, 27, NULL),
(4, 28, NULL),

(5, 1, 'He possess excellent decision-making skills, consistently making sound judgments based on thorough analysis.'),
(5, 3, 'He is highly knowledgeable in their field, continuously staying updated on industry trends and best practices.'),
(5, 5, 'He is passionate about their work, bringing enthusiasm and energy to everything they do.'),
(5, 7, 'He demonstrate empathy and understanding towards colleagues, creating a supportive and inclusive environment.'),
(5, 9, 'He is resourceful, finding creative solutions to overcome obstacles.'),
(5, 11, 'He show integrity and honesty in their actions, earning the trust and respect of those around them.'),
(5, 13, 'He is results-oriented, focused on achieving measurable outcomes.'),
(5, 15, 'He is resilient, bouncing back from setbacks with determination and perseverance.'),
(5, 17, NULL),
(5, 19, NULL),
(5, 21, NULL),
(5, 23, NULL),
(5, 25, NULL),
(5, 25, NULL),
(5, 4, NULL),
(5, 8, NULL),
(5, 12, NULL),
(5, 16, NULL),
(5, 20, NULL),
(5, 24, NULL),
(6, 2, 'He is excellent mentors, providing guidance and support to junior members of the team.'),
(6, 6, 'He show humility and are open to learning from others.'),
(6, 10, 'He exhibit strong leadership potential, capable of taking on greater responsibilities in the future.'),
(6, 14, 'He prioritize teamwork and cooperation, recognizing the value of collective effort.'),
(6, 18, NULL),
(6, 22, NULL),
(6, 26, NULL),
(6, 27, NULL),
(6, 28, NULL),
(6, 30, NULL),
(6, 31, NULL),
(6, 32, NULL),
(6, 33, NULL),
(6, 34, NULL),
(6, 35, NULL),
(6, 36, NULL),
(7, 37, NULL),
(7, 38, NULL),
(7, 39, NULL),
(7, 40, NULL),
(7, 41, NULL),
(7, 42, NULL),
(7, 43, NULL),
(7, 44, NULL),
(7, 45, NULL),

(8, 1, NULL),
(8, 2, NULL),
(8, 3, NULL),
(8, 4, NULL),
(8, 5, NULL);
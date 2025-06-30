Create table Teams(
	ID serial primary key,
	Team varchar(32)
	
);

create table Roles(
	id serial primary key,
	Role varchar(32)

);

create table Users(
	ID serial primary key,
	Username varchar(32) unique not null,
	Password_hash varchar(255) not null,
	FName varchar(32) not null,
	MName varchar(32),
	LName varchar(32),
	Role_id int references Roles(id),
	Team_id int references Teams(id),
	Email varchar(255),
	is_Active boolean default false

);

create table statuses(
	id serial primary key,
	name varchar(32)
);

create table projects(
	id serial primary key,
	name varchar(64) not null,
	start_date date,
	deadline date,
	status_id integer references statuses(id)
);

create table tasks(
	id serial primary key,
	name varchar(64) not null,
	status_id integer references statuses(id),
	priority int2 default 3 check(priority > 0 and priority < 10)
);

create table users_to_projects(
	id_user int references users(id),
	id_project int references projects(id)
);

create table json_user_works(
	id serial primary key,
	user_id int references users(id),
	jsonwork json

);


-- Заполнение таблицы статусов
INSERT INTO statuses (id, name) VALUES
(1, 'Новая'),
(2, 'В работе'),
(3, 'На проверке'),
(4, 'Завершена'),
(5, 'Отложена'),
(6, 'Отменена'),
(7, 'Требуется информация');


INSERT INTO roles (id, role) VALUES
(1, 'Администратор'),
(2, 'Менеджер'),
(3, 'Разработчик'),
(4, 'Дизайнер'),
(5, 'Тестировщик'),
(6, 'Аналитик'),
(7, 'Гость');

-- Заполнение таблицы teams
INSERT INTO teams (id, team) VALUES
(1, 'Разработка'),
(2, 'Дизайн'),
(3, 'Маркетинг'),
(4, 'Тестирование'),
(5, 'Менеджмент');

-- Заполнение таблицы users
INSERT INTO users (id, username, password_hash, fname, mname, lname, role_id, team_id, email, is_active) VALUES
(1, 'ivanov', '1212', 'Иванов', 'Иван', 'Иванович', 1, 1, 'ivanov@example.com', false),
(2, 'petrova', '1111', 'Петрова', 'Анна', 'Сергеевна', 2, 2, 'petrova@example.com', false),
(3, 'sidorov', '1212', 'Сидоров', 'Алексей', 'Петрович', 3, 1, 'sidorov@example.com', false),
(4, 'smirnova', '1231', 'Смирнова', 'Елена', 'Владимировна', 2, 3, 'smirnova@example.com', false),
(5, 'kuznetsov', '1231', 'Кузнецов', 'Дмитрий', 'Александрович', 1, 4, 'kuznetsov@example.com', false);

-- Заполнение таблицы projects
INSERT INTO projects (id, name, start_date, deadline, status_id) VALUES
(1, 'Разработка мобильного приложения', '2024-01-15', '2025-10-30', 1),
(2, 'Редизайн корпоративного сайта', '2024-02-01', '2025-09-15', 2),
(3, 'Маркетинговая кампания Q2', '2024-03-01', '2026-04-30', 3),
(4, 'Тестирование новой платформы', '2024-03-15', '2025-07-31', 1),
(5, 'Внутренний портал сотрудников', '2024-01-10', '2025-09-30', 1);

-- Заполнение таблицы users_to_project
INSERT INTO users_to_projects (id_user, id_project) VALUES
(1, 1),
(1, 5),
(2, 2),
(3, 1),
(3, 4),
(4, 3),
(5, 4),
(2, 5),
(4, 2);

-- Заполнение таблицы tasks
INSERT INTO tasks (id, name, status_id, priority) VALUES
(1, 'Разработка API для авторизации', 1, 1),
(2, 'Создание макетов главной страницы', 2, 2),
(3, 'Настройка рекламных кампаний', 3, 3),
(4, 'Тестирование модуля платежей', 4, 1),
(5, 'Интеграция с CRM системой', 1, 2),
(6, 'Разработка личного кабинета', 1, 1),
(7, 'Анализ конкурентов', 3, 3),
(8, 'Тестирование мобильной версии', 4, 2),
(9, 'Подготовка презентации', 5, 3),
(10, 'Оптимизация базы данных', 1, 1);
// Чтение конфигурации
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Context;
using Server.Context.Tables;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Пример использования
using (var db = new AppDbContext())
{
    // Проверка подключения
    Console.WriteLine("Проверяем подключение к БД...");
    Console.WriteLine($"БД: {db.Database.GetDbConnection().Database}");
    Console.WriteLine($"Сервер: {db.Database.GetDbConnection().DataSource}");

    // Простой запрос
    var userCount = db.Users.Count();
    Console.WriteLine($"Количество записей в БД: {userCount}");
    var roleCount = db.Roles.Count();
    Console.WriteLine($"Количество записей в БД: {roleCount}");
    var teamCount = db.Teams.Count();
    Console.WriteLine($"Количество записей в БД: {teamCount}");
    var taskCount = db.Tasks.Count();
    Console.WriteLine($"Количество записей в БД: {taskCount}");
    var statCount = db.Statuses.Count();
    Console.WriteLine($"Количество записей в БД: {statCount}");
    var prjctCount = db.Projects.Count();
    Console.WriteLine($"Количество записей в БД: {prjctCount}");

    db.SaveChanges();

}
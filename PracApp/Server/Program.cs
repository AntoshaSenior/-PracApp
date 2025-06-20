// Чтение конфигурации
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Context;
using Server.Context.Tables;
using System.Net;
using System.Net.Sockets;


namespace Server
{
    public class Program
    {
        public static void Main()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 5433);
            using Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(30);
            Console.WriteLine(ipPoint.Address);
            Console.WriteLine("Сервер запущен. Ожидание подключений...");


            ServerListener(socket);



        }
        public async static void ServerListener(Socket socket)
        {
            using Socket client = await socket.AcceptAsync();

            Console.WriteLine($"Адрес подключенного клиента: {client.RemoteEndPoint}");
        }

        public void dbUsing()
        {
            // Пример использования
            using (var db = new AppDbContext())
            {
                // Проверка подключения
                Console.WriteLine("Проверяем подключение к БД...");
                Console.WriteLine($"БД: {db.Database.GetDbConnection().Database}");
                Console.WriteLine($"Сервер БД: {db.Database.GetDbConnection().DataSource}");
                
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
        }
    }
}
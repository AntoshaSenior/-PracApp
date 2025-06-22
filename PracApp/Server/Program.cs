// Чтение конфигурации
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ContextLib;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ContextLib.Context;
using ContextLib.Context.Tables;


namespace Server
{
    public class Program
    {
        public static void Main()
        {

            TcpListener server = new TcpListener(IPAddress.Any, 6666);
            server.Start();
            Console.WriteLine("server start");
            
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                while (client.Connected)
                {
                    int count = stream.Read(buffer, 0, buffer.Length);
                    string inf = Encoding.Default.GetString(buffer, 0, count);
                    Console.Write(inf);
                    Console.WriteLine();
                    string[] infCom = inf.Split('\n');
                    AppDbContext dbContext = new AppDbContext();


                    var option = new JsonSerializerOptions { WriteIndented = true };

                    List<User> dan = dbContext.Users.Where(u => u.Username == infCom[0] & u.Password == infCom[1]).
                        Select(u => u).ToList();

                    

                    string json = JsonSerializer.Serialize<User>(dan[0], option);


                    User? us = JsonSerializer.Deserialize<User>(json);

                    Console.WriteLine(us.Username);

                    client.Close();
                }

            }

        }
      

        public static void dbUsing()
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
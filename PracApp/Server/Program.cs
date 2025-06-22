// Чтение конфигурации
using ContextLib;
using ContextLib.Context;
using ContextLib.Context.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace Server
{
    public class Program
    {
        public static void Main()
        {

            TcpListener server = new TcpListener(IPAddress.Any, 6666);
            server.Start();
            Console.WriteLine("server start");
            byte[] buffer = new byte[1024];
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                

                while (client.Connected)
                {
                    try
                    {
                        buffer = new byte[1024];
                        int count = stream.Read(buffer, 0, buffer.Length);
                        string inf = Encoding.UTF8.GetString(buffer, 0, count);
                        Console.Write(inf);
                        Console.WriteLine();
                        string[] infCom = inf.Split('\n');
                        AppDbContext dbContext = new AppDbContext();


                        var option = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };

                        List<User> UserInf = dbContext.Users.Where(u => u.Username == infCom[0] & u.Password == infCom[1]).
                            Select(u => u).ToList();

                        List<Role> RoleInf = dbContext.Roles.Where(u => u.ID == UserInf[0].RoleID).Select(u => u).ToList();

                        List<Team> TeamInf = dbContext.Teams.Where(u => u.ID == UserInf[0].TeamID).Select(u => u).ToList();

                        Console.WriteLine("User is Send");
                        string json = JsonSerializer.Serialize<User>(UserInf[0], option);

                        buffer = Encoding.UTF8.GetBytes(json);
                        stream.Write(buffer, 0, buffer.Length);

                        Console.WriteLine("Role is send");


                        json = JsonSerializer.Serialize<Role>(RoleInf[0], option);

                        buffer = Encoding.UTF8.GetBytes(json);
                        stream.Write(buffer, 0, buffer.Length);

                        Console.WriteLine("Teams is send");
                        json = JsonSerializer.Serialize<Team>(TeamInf[0], option);

                        buffer = Encoding.UTF8.GetBytes(json);

                        stream.Write(buffer, 0, buffer.Length);


                        stream.Close();
                        client.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Error");
                        stream.Close();
                        client.Close();
                    }
                    
                }

            }

        }
      

    }
}
using ContextLib;
using ContextLib.Context;
using ContextLib.Context.Tables;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 6666);
            server.Start();
            Console.WriteLine("Сервер запущен и слушает порт 6666");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                _ = System.Threading.Tasks.Task.Run(() => HandleClientAsync(client));
            }
        }

        static async System.Threading.Tasks.Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                string? command = await reader.ReadLineAsync();

                switch (command)
                {
                    case "AUTH":
                        await HandleAuthAsync(reader, writer);
                        break;

                    case "SEND_STATS":
                        await HandleSendStatsAsync(reader, writer);
                        break;

                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке клиента: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        static async System.Threading.Tasks.Task HandleAuthAsync(StreamReader reader, StreamWriter writer)
        {
            string? username = await reader.ReadLineAsync();
            string? password = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await writer.WriteLineAsync("ERROR");
                return;
            }

            using var db = new AppDbContext();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                string json = JsonSerializer.Serialize(user);
                await writer.WriteLineAsync("OK");
                await writer.WriteLineAsync(json);
                Console.WriteLine($"Авторизация: {username} (ID: {user.ID})");
            }
            else
            {
                await writer.WriteLineAsync("ERROR");
                Console.WriteLine($"Авторизация не удалась: {username}");
            }
        }

        static async System.Threading.Tasks.Task HandleSendStatsAsync(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string userIdStr = await reader.ReadLineAsync();
                string contentLengthStr = await reader.ReadLineAsync();

                if (!int.TryParse(userIdStr, out int userId) ||
                    !int.TryParse(contentLengthStr, out int contentLength))
                {
                    await writer.WriteLineAsync("ERROR: Invalid headers");
                    return;
                }

                using var db = new AppDbContext();
                bool userExists = await db.Users.AnyAsync(u => u.ID == userId);
                if (!userExists)
                {
                    await writer.WriteLineAsync("ERROR: Unknown user ID");
                    return;
                }

                char[] buffer = new char[contentLength];
                int totalRead = 0;
                while (totalRead < contentLength)
                {
                    int read = await reader.ReadAsync(buffer, totalRead, contentLength - totalRead);
                    if (read == 0)
                    {
                        await writer.WriteLineAsync("ERROR: Incomplete JSON");
                        return;
                    }
                    totalRead += read;
                }

                string jsonWork = new string(buffer);

                // Опциональная проверка валидности JSON
                try
                {
                    var test = JsonSerializer.Deserialize<List<ProcessInfo>>(jsonWork);
                    if (test == null || test.Count == 0)
                    {
                        await writer.WriteLineAsync("ERROR: Empty JSON");
                        return;
                    }
                }
                catch
                {
                    await writer.WriteLineAsync("ERROR: Invalid JSON");
                    return;
                }

                var record = new JsonUserWork
                {
                    UserID = userId,
                    JsonWork = jsonWork
                };

                await db.JsonUserWorks.AddAsync(record);
                await db.SaveChangesAsync();

                await writer.WriteLineAsync("OK");
                Console.WriteLine($"Сохранено JSON от пользователя {userId}");
            }
            catch (Exception ex)
            {
                string err = ex.InnerException?.Message ?? ex.Message;
                await writer.WriteLineAsync($"ERROR: {err}");
                Console.WriteLine($"Ошибка: {err}");
            }
        }



    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var socket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Loopback, 1234 );
            socket.Bind(endPoint);
            socket.Listen(10);

            try
            {
                Console.WriteLine("Server has started");
                while (true)
                {
                    var client= socket.Accept();
                    var buf = new byte[1024];
                    int bytesRead = client.Receive(buf);
                    string receivedMessage= Encoding.UTF8.GetString(buf, 0, bytesRead);
                    Console.WriteLine($"o {DateTime.Now:HH:mm}" +
                                      $" від {client?.RemoteEndPoint?.ToString()}" +
                                      $" отримано рядок:{receivedMessage}");

                    string responseMessage = "Привіт, клієнте! ";
                    client?.Send(Encoding.UTF8.GetBytes(responseMessage));
                    client?.Shutdown(SocketShutdown.Both);
                    client?.Close();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            socket?.Close();
            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
        
    }
}

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Http.Json;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TCPServerConcurrent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TCP Server concurrent!");

            //concurrent server handles multiple cleients at the same time

            //initialisere et objekt af tcplistner class
            TcpListener listener = new TcpListener(IPAddress.Any, 7);
            //begynd at "lytte" for en connection
            listener.Start();

            //acceptere tcp client objekt
            //AcceptTcpClient - dette returnere et TcpClient object
            TcpClient socket = listener.AcceptTcpClient();
            HandleClient(socket);
            listener.Stop();         
        }

        //handle client 
        public static async Task HandleClient(TcpClient socket)
        {
            //opg 7 - 1. GetAll: returner en liste med alle movies
            using (HttpClient httpClient = new HttpClient())
            { 
                //streams - read and write i connectionen
                NetworkStream networkStream = socket.GetStream();
                //derefter splittes de op i to streams
                StreamReader reader = new StreamReader(networkStream);
                StreamWriter writer = new StreamWriter(networkStream);
                //læs det client sender
                string message = reader.ReadLine();
                Console.WriteLine("Client send this: " + message);
                writer.WriteLine(message);
                //skyl ud, rydde op
                writer.Flush();

                //Opg 7 -  modtager alle filmene
                //kalde GET fra din REST controller movie
                //tilføjede url
                httpClient.GetFromJsonAsync("https://restapimovie.azurewebsites.net/api/movies");

                //luk socket
                socket.Close();
            }
        }
    }
} 

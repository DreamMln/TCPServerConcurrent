using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Http.Json;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TCPServerConcurrent
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("TCP Server concurrent!");

            //concurrent server handles multiple cleients at the same time

            //initialisere et objekt af tcplistner class
            TcpListener listener = new TcpListener(IPAddress.Any, 43214);
            //begynd at "lytte" for en connection
            listener.Start();

            while (true)
            {
                //acceptere tcp client objekt
                //AcceptTcpClient - dette returnere et TcpClient object
                //krav
                TcpClient socket = listener.AcceptTcpClient();

                //CONCURRENT server - Task.Run(() håndtere/starter flere tråde,
                //så vi kan have flere clients samtidig

                //Objekt niveau
                //Der er ikke nogle informationer der bliver gemt i Main i program
                //Program program = new Program();

                //Task.Run(() => program.HandleAClient());

                Task.Run(() => HandleAClient(socket));
            }
        }
        //refactoring - gør det smartere
        public static void HandleAClient(TcpClient socket)
        {
            //streams - read and write i connectionen
            //data strøm frem og tilbage, det er to-vejs kommunikation
            NetworkStream networkStream = socket.GetStream();
            //derefter splittes de op i to streams
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            //opret et nyt movie object
            //kald metoden getall, der returnere alle movies i en JSONString
            //kald getByCountry - der returner en liste med alle movies der indeholder
            //en tekststreng i deres Country property
            MovieManager movieManager = new MovieManager();

            //læs det client anmoder om
            string message = reader.ReadLine();

            if (message == "GetAll")
            {
                List<Movie> getAllMovies = movieManager.GetAll();
                //Lavet om til JSON-format, udveksle data
                //data udveksle en liste af Movie objekter
                string movieSerializeToAJsonString =
                JsonConvert.SerializeObject(getAllMovies);
                Console.WriteLine("The list has returned");
                //reultatet af serialization
                //skriver til client
                writer.WriteLine(movieSerializeToAJsonString);
            }
            else if (message.StartsWith("GetByCountry"))
            {
                //jeg vil kun have en del af stringen, derfor bruger jeg Substring
                //tæller antal karaktere hvor den skal starte med at returnere stringen fra
                //jeg ignorere derfor de første 13 karaktere | alt før 13
                string filterOnCountry = message.Substring(13);
                //yderligere sender et land med, fra clienten
                List<Movie> getByCountry = movieManager.GetByCountry(filterOnCountry);
                string countrySerializeToAJsonString =
                JsonConvert.SerializeObject(getByCountry);
                Console.WriteLine("The list with a filter");
                //skriver til client
                writer.WriteLine(countrySerializeToAJsonString);
            }
            //besked i konsollen
            //Console.WriteLine("Client send this: " + message);

            //skyl ud, rydde op, TCP streams har buffer og cache
            //- tvinge den til at sende pakken her og nu
            writer.Flush();
            //stopper med at ''lytte'' - stopper serveren
            //listener.Stop();
            //luk socket - fordi det er TCP, for at spare på com ressourcer, så lad os afslutte den med det samme
            //spar på ressourcer, ryd ordenligt op
            socket.Close();
        }
    }
} 

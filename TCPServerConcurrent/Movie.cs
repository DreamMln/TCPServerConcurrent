using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace TCPServerConcurrent
{
    public class Movie
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int LengthInMinutes { get; set; }
        public string Country { get; set; }

        public Movie()
        {
            //default
        }

        public Movie(int id, string name, int lengthInMinutes, string city)
        {
            ID = id;
            Name = name;
            LengthInMinutes = lengthInMinutes;
            Country = city;
        }
    }
}

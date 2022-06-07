using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerConcurrent
{
    class MovieManager
    {
        private static List<Movie> _movies = new List<Movie>()
        {
            new Movie(){ ID = 1, Name = "Dune", LengthInMinutes = 155, Country = "USA" },
            new Movie(){ ID = 2, Name = "Atlantis", LengthInMinutes = 132, Country = "New Zealand" },
            new Movie(){ ID = 3, Name = "Go' film", LengthInMinutes = 120, Country = "Danmark" }
        };

        public List<Movie> GetAll()
        {
            List<Movie> result = new List<Movie>(_movies);
            //serializer - get all skal returnere en json string
            //til en string i JSON format
            return result;
        }

        public List<Movie> GetByCountry(string filterOnCountry)
        {
            //List<Movie> result = new List<Movie>(_movies);
            //return result;

            return _movies.FindAll(movie => movie.Country == filterOnCountry);
        }

    }
}

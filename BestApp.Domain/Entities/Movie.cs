using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Domain.Entities
{
    public class Movie : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }

        public static Movie Create(string name, string overview, string posterUrl)
        {
            if(name == null) throw new ArgumentNullException("name");
            if (overview == null) throw new ArgumentNullException("overview");

            return new Movie()
            {                
                Name = name,
                Overview = overview,
                PosterUrl = posterUrl
            };
        }
    }
}

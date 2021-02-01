using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStudioAPI.Models
{
    public class Metadata
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public string Title { get; set; }

        public string Language { get; set; }

        public string Duration { get; set; }

        public int ReleaseYear { get; set; }
    }
}

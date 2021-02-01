using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStudioAPI.DTOs
{
    public class StatsDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public double AverageWatchDurationS { get; set; }
        public int Watches { get; set; }
        public int ReleaseYear { get; set; }
    }
}

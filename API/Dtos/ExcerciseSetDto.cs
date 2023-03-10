using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ExcerciseSetDto
    {
        public int Index { get; set; }
        public string Reps { get; set; }
        public bool IsDone { get; set; }
        public int Duration { get; set; }
        public int ExcerciseId { get; set; }
    }
    public class ExcerciseCSetDto
    {
        public string Reps { get; set; }
        public int Duration { get; set; }
        public int ExcerciseId { get; set; }
    }
}
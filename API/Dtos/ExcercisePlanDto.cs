using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Excercise;

namespace API.Dtos
{
    public class ExcercisePlanDto
    {
        public int Day { get; set; }
        public List<ExcerciseSetDto> ExcerciseList { get; set; }
        
    }
    public class ExcerciseCPlanDto
    {
        public string AppUserId { get; set; }
        public int Day { get; set; }
        public List<ExcerciseCSetDto> ExcerciseList { get; set; }
        
    }
}
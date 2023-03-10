using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MealPlanDto
    {
        public int Day { get; set; }
        public int Total { get; set; }
        public string AppUserId { get; set; }
        public List<MealDto> MealList { get; set; }
    }
    public class MealCPlanDto
    {
        public int Day { get; set; }
        public string AppUserId { get; set; }
        public List<MealCDto> MealList { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Text;

namespace TestBot.Models
{
    class ModelOfUserRate
    {
        public string BarName { get; set; }
        public List<long> Likes { get; set; } = new List<long>();
        public List<long> Dislikes { get; set; } = new List<long>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Tir
    {
        public int coord { get; }
        public bool hit { get; set; }
        public string status { get; set; }

        public Tir(int coord, bool hit, string status)
        {
            this.coord = coord;
            this.hit = hit;
            this.status = status;
        }
    }
}

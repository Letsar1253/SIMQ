using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimQCore.Library.PointProcesses
{
    interface IPointProcess
    {
        public double Generate();
    }
}

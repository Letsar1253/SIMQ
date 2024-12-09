using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticFunction.CompareDists
{
    internal interface ICompareDists
    {
        bool CompareDists(double[] Dist1, double[] Dist2, int N, out double Result);
    }
}

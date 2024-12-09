using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticFunction.CompareDists
{
    //Kolmogorov Distance
    internal class KD: ICompareDists
    {
        public bool CompareDists(double[] Dist1, double[] Dist2, int N, out double Result)
        {
            if (!KolmogorovDistance(Dist1, Dist2, N, out Result))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        protected bool KolmogorovDistance(double[] Empirical, double[] estimatedDist, int N, out double kolmogorovDistance)
        {
            double tempMax = double.MinValue;
            double tempDif;

            for (int i = 0; i < N; i++)
            {
                tempDif = Math.Abs(Empirical[i] - estimatedDist[i]);
                if (tempDif > tempMax)
                {
                    tempMax = tempDif;
                }
            }

            kolmogorovDistance = tempMax; //По формуле у меня Dn

            return true;
        }
    }
}

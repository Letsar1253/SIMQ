using System;

namespace SimQCore.Library.CompareDists {
    //Kolmogorov Distance
    internal class KD : ICompareDists
    {
        public bool CompareDists(double[] Dist1, double[] Dist2, int N, out double Result)
        {
            return KolmogorovDistance( Dist1, Dist2, N, out Result );
        }

        public static bool KolmogorovDistance(double[] Empirical, double[] estimatedDist, int N, out double kolmogorovDistance)
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

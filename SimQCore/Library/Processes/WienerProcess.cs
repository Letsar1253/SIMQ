using SimQCore.Library.Distributions;

namespace SimQCore.Processes
{
    public class WienerProcess : IProcess
    {
        private NormalDistribution _normalDistribution;
        private double _lastValue = 0;
        private double _alpha;
        private double _sigma;

        public WienerProcess(double alpha, double sigma)
        {
            _alpha = alpha;
            _sigma = sigma;
            _normalDistribution = new NormalDistribution(0, 1);
        }

        public double Generate()
        {
            _lastValue += _normalDistribution.Generate() * _sigma + _alpha;
            return _lastValue;
        }
    }
}

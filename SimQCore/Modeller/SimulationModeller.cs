using System;

namespace SimQCore.Modeller
{
    class SimulationModeller
    {
        private readonly Supervisor Supervisor = new();
        public double ModelTimeMax { get; set; } = 50;
        private bool IsDone(double t) => t >= ModelTimeMax;

        public void Simulate(Problem problem)
        {
            Supervisor.Setup(problem);

            Console.WriteLine("Моделирование началось.");

            double T = 0;
            while (!IsDone(T))
            {
                Event nextEvent = Supervisor.GetNextEvent();

                // Для статистических данных.
                //double deltaT = nextEvent.ModelTime - T;

                T = nextEvent.ModelTimeStamp;

                // В данном сегменте кода должен проходить сбор статистических данных.
                //Statistic.SaveState(delta); 

                Supervisor.FireEvent(nextEvent, T);
            }

            Console.WriteLine("Моделирование окончено.");
        }
    }
}

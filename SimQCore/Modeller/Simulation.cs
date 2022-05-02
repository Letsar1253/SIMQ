using SimQCore.Modeller.BaseModels;
using SimQCore.Modeller.Supervisors;
using System;

namespace SimQCore.Modeller.Simulation
{
    class SimulationModeller
    {
        private readonly Supervisor Supervisor = new();
        public double ModelTimeMax { get; set; }
        private bool IsDone(double t) => t >= ModelTimeMax;

        public void Simulate(Problem problem)
        {
            Supervisor.Setup(problem);

            Console.WriteLine("Моделирование началось.");

            double T = 0;
            while (!IsDone(T))
            {
                Event nextEvent = Supervisor.GetNextEvent();
                //double deltaT = nextEvent.ModelTime - T;
                T = nextEvent.ModelTime;

                //Statistic.SaveState(delta);

                Supervisor.Actions[nextEvent.Agent.EventTag](nextEvent.Agent, T);
            }

            Console.WriteLine("Моделирование окончено.");
        }
    }
}

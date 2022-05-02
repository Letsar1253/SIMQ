using SimQCore.Library;
using SimQCore.Modeller;
using SimQCore.Modeller.BaseModels;
using SimQCore.Modeller.CustomModels;
using System.Collections.Generic;

namespace SimQCore
{
    class Program
    {
        static void Main()
        {
            // Часть Миши
            Tests.TestTimeGeneration1();

            // Часть Эмиля
            SimulationModeller SM = new();

            SimpleServiceBlock serviceBlock = new();
            SimpleStackBunker simpleStack = new();
            SimpleSource source1 = new();
            SimpleSource source2 = new();

            serviceBlock.BindBunker(simpleStack);

            List<AgentModel> list = new();
            list.Add(source1);
            list.Add(source2);
            list.Add(serviceBlock);
            list.Add(simpleStack);

            Problem problem = new() {
                Agents = list,
            };

            SM.Simulate(problem);
        }
    }
}
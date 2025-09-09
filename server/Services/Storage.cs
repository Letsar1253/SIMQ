
using server.Models;
using SimQCore.Library.Distributions;
using SimQCore.Modeller;
using SimQCore.Modeller.Models;
using SimQCore.Modeller.Models.Common;
using SimQCore.Statistic;

namespace server.Services {
    /// <summary>
    /// Временный статический класс, представляющий собой единое хранилище данных предметной области.
    /// </summary>
    public static class StorageService {
        /** Метод инициализирует задачи, используемые в качестве примеров. */
        private static Dictionary<string, Problem> InitExampleProblems() {
            Dictionary<string, Problem> examples = [];

            // Общие переменные
            Dictionary<string, List<IModellingAgent>> linkList;
            List<IModellingAgent> agentList;
            List<IModellingAgent> sourcesLinks;
            BaseSource source1, source2, source3, source4;
            BaseServiceBlock serviceBlock1, serviceBlock2;
            QueueBuffer queue1, queue2, queue3;

            //  ----------[[ Задача 1 ]]----------

            source1 = new Source( new ExponentialDistribution( 0.2 ) );
            source2 = new Source( new ExponentialDistribution( 0.4 ) );
            source3 = new Source( new ExponentialDistribution( 0.6 ) );

            queue1 = new( 6 );
            queue2 = new();

            serviceBlock1 = new ServiceBlock( new ExponentialDistribution( 0.3 ) );
            serviceBlock2 = new ServiceBlock( new ExponentialDistribution( 0.7 ) );

            sourcesLinks = [
                serviceBlock1, serviceBlock2
            ];

            agentList = [
                source1, source2, source3,
                queue1, queue2,
                serviceBlock1, serviceBlock2
            ];

            serviceBlock1.BindBuffer( queue1 );
            serviceBlock1.BindBuffer( queue2 );

            serviceBlock2.BindBuffer( queue1 );
            serviceBlock2.BindBuffer( queue2 );

            linkList = new() {
                {
                    source1.Id,
                    sourcesLinks
                },
                {
                    source2.Id,
                    sourcesLinks
                },
                {
                    source3.Id,
                    sourcesLinks
                }
            };

            
            examples.Add( "Example 1", new()
            {
                Agents = agentList,
                Name = "Example 1",
                Links = linkList
            } );

            //  ----------[[ Задача 2 ]]----------

            source1 = new Source( new ExponentialDistribution( 0.2 ) );
            source2 = new Source( new ExponentialDistribution( 0.4 ) );

            Orbit orbit = new( new ExponentialDistribution( 0.5 ) );

            serviceBlock1 = new ServiceBlock( new ExponentialDistribution( 0.3 ) );

            agentList = new() {
                source1, source2,
                orbit,
                serviceBlock1
            };

            sourcesLinks = new() {
                serviceBlock1,
                orbit
            };

            List<IModellingAgent> orbitLinks = [
                serviceBlock1
            ];

            linkList = new() {
                {
                    source1.Id,
                    sourcesLinks
                },
                {
                    source2.Id,
                    sourcesLinks
                },
                {
                    orbit.Id,
                    orbitLinks
                }
            };

            examples.Add( "Example 2", new()
            {
                Agents = agentList,
                Name = "Example 2",
                Links = linkList
            } );

            //  ----------[[ Задача 3 ]]----------

            source1 = new Source( new ExponentialDistribution( 0.2 ) );
            source2 = new Source( new ExponentialDistribution( 0.4 ) );
            source3 = new Source( new ExponentialDistribution( 0.6 ) );
            source4 = new Source( new ExponentialDistribution( 0.8 ) );

            queue1 = new( 3 );
            queue2 = new( 2 );
            queue3 = new( 4 );

            serviceBlock1 = new PollingServiceBlock( new ExponentialDistribution( 0.5 ), 3 );
            serviceBlock2 = new PollingServiceBlock( new ExponentialDistribution( 0.6 ), 3 );

            serviceBlock1.BindBuffer( queue1 );
            serviceBlock1.BindBuffer( queue2 );
            serviceBlock1.BindBuffer( queue3 );

            serviceBlock2.BindBuffer( queue1 );
            serviceBlock2.BindBuffer( queue2 );
            serviceBlock2.BindBuffer( queue3 );

            sourcesLinks = [
                serviceBlock1, serviceBlock2
            ];

            linkList = new() {
                {
                    source1.Id,
                    sourcesLinks
                },
                {
                    source2.Id,
                    sourcesLinks
                },
                {
                    source3.Id,
                    sourcesLinks
                },
                {
                    source4.Id,
                    sourcesLinks
                }
            };

            agentList = [
                source1,
                source2,
                source3,
                source4,
                queue1,
                queue2,
                queue3,
                serviceBlock1,
                serviceBlock2
            ];

            examples.Add( "Example 3", new()
            {
                Agents = agentList,
                Name = "Example 3",
                Links = linkList
            } );

            //  ----------[[ Задача 4 ]]----------

            source1 = new FiniteSource( new ExponentialDistribution( 0.2 ) );
            source2 = new FiniteSource( new ExponentialDistribution( 0.4 ) );
            source3 = new FiniteSource( new ExponentialDistribution( 0.6 ) );

            queue1 = new QueueBuffer( 3 );

            serviceBlock1 = new ServiceBlock( new ExponentialDistribution( 0.5 ) );

            serviceBlock1.BindBuffer( queue1 );

            sourcesLinks = [
                serviceBlock1
            ];

            linkList = new() {
                {
                    source1.Id,
                    sourcesLinks
                },
                {
                    source2.Id,
                    sourcesLinks
                },
                {
                    source3.Id,
                    sourcesLinks
                }
            };

            agentList = [
                source1, source2, source3,
                queue1,
                serviceBlock1
            ];

            examples.Add( "Example 4", new()
            {
                Agents = agentList,
                Name = "Example 4",
                Links = linkList
            } );

            //  ----------[[ Задача 5 ]]----------


            GenerationErrorSettings ges = new()
            {
                GenerationErrorCheckStep = 1000,
                GenerationErrorCheckStepModifier = 2,
                MinGenerationError = 0.001
            };

            Problem problem = SimQCore.Simulation.RunQS.InitProblem( ges );
            examples.Add( problem.Name, problem );

            return examples;
        }

        public readonly static Dictionary<string, Problem> Problems = InitExampleProblems();

        public readonly static Dictionary<string, Dictionary<Guid, Result>> Results = [];

        public readonly static Dictionary<uint, SimulationTask> SimulationTasks = [];
    }

}

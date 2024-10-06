using SimQCore.Library.Distributions;
using SimQCore.Modeller;
using SimQCore.Modeller.Models;
using SimQCore.Modeller.Models.Common;
using SimQCore.Modeller.Models.UserModels;
using SimQCore.Statistic;
using System;
using System.Collections;
using System.Collections.Generic;
using SimQCore.RunQS;

namespace SimQCore {
    public class Program {
        static void Main() {

            Misc.showLogs = false;

            // Задачи упомянутые в ВКР - InitExampleProblems()[Номер_задачи]
            // Задача с бесконечным блоком приборов - InitInfServiceBlockProblem()
            // Задача с конечным блоком приборов - InitFinServiceBlockProblem()


            double La = 2;
            double Mu = 1;
            int S = 1;
            int Q = 0;
            //если S = 2, 3 - то возникает ошибка
            Problem problem = RunQS.RunQS.InitFinServiceBlockProblem(La, Mu, S, Q);
            //Problem problem = RunQS.RunQS.InitInfServiceBlockProblem(0.2, 0.5);

            SimulationModeller modeller = new();
            modeller.Simulate( problem );

            StatesStatistic StatesStat = new(modeller.data);


            Misc.Log($"\nСтатистика по результатам моделирования задачи \"{modeller.problem.Name}\":");

            StatesStat.Print_EmpDist();

            // сохранить эмпирическое распределение в массиве Y
            StatesStat.Get_EmpDist(out double[] Y);
        }

        


        /** Метод инициализирует 4 задачи, используемые в качестве примеров в ВКР. */
        private static List<Problem> InitExampleProblems() {
            List<Problem> examples = new();

            // Общие переменные.
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

            sourcesLinks = new() {
                serviceBlock1, serviceBlock2
            };

            agentList = new() {
                source1, source2, source3,
                queue1, queue2,
                serviceBlock1, serviceBlock2
            };

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

            examples.Add( new() {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example 1.",
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

            List<IModellingAgent> orbitLinks = new() {
                serviceBlock1
            };

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

            examples.Add( new() {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example 2.",
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

            sourcesLinks = new() {
                serviceBlock1, serviceBlock2
            };

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

            agentList = new() {
                source1, source2, source3, source4,
                queue1, queue2, queue3,
                serviceBlock1, serviceBlock2
            };

            examples.Add( new() {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example 3.",
                Links = linkList
            } );

            //  ----------[[ Задача 4 ]]----------

            source1 = new FiniteSource( new ExponentialDistribution( 0.2 ) );
            source2 = new FiniteSource( new ExponentialDistribution( 0.4 ) );
            source3 = new FiniteSource( new ExponentialDistribution( 0.6 ) );

            queue1 = new QueueBuffer( 3 );

            serviceBlock1 = new ServiceBlock( new ExponentialDistribution( 0.5 ) );

            serviceBlock1.BindBuffer( queue1 );

            sourcesLinks = new() {
                serviceBlock1
            };

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

            agentList = new() {
                source1, source2, source3,
                queue1,
                serviceBlock1
            };

            examples.Add( new() {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example 4.",
                Links = linkList
            } );

            return examples;
        }



        
    }
}
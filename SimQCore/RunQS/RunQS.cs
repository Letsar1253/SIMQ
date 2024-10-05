using SimQCore.Library.Distributions;
using SimQCore.Modeller;
using SimQCore.Modeller.Models.Common;
using SimQCore.Modeller.Models.UserModels;
using SimQCore.Modeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimQCore.RunQS
{
    //имитационное моделирование СМО различных типов

    public class RunQS
    {
        public ErrorMessage em = new ErrorMessage();



        /** M/M/n/c */
        // запустить MMSQ и получить ее модель
        public bool Run_MMSQ_GetModel(out SimulationModeller modeller, double La = 1, double Mu = 2, int S = 1, int Q = 0, double MaxSimTime = 10000)
        {
            //если S = 2, 3 - то возникает ошибка
            Problem problem = InitFinServiceBlockProblem(La, Mu, S, Q, MaxSimTime);

            modeller = new();

            try
            {
                modeller.Simulate(problem);
            }
            catch (Exception e)
            {
                em.Add_ErrorMsg($"{e.Message}.");
                return false;
            }

            return true;
        }


        /** M/M/inf/inf */
        // запустить M/M/inf/inf и получить ее модель
        public bool Run_MMinf_GetModel(out SimulationModeller modeller, double La = 0.2, double Mu = 0.5, double MaxSimTime = 100)
        {
            //если S = 2, 3 - то возникает ошибка
            Problem problem = InitInfServiceBlockProblem(La, Mu, MaxSimTime);

            modeller = new();

            try
            {
                modeller.Simulate(problem);
            }
            catch (Exception e)
            {
                em.Add_ErrorMsg($"{e.Message}.");
                return false;
            }

            return true;
        }



        /** Метод инициализирует задачу с бесконечным числом обработчиков. */
        /** M/M/inf/inf */
        internal static Problem InitInfServiceBlockProblem(double La = 0.2, double Mu = 0.5, double MaxSimTime = 100)
        {
            Dictionary<string, List<IModellingAgent>> linkList;
            List<IModellingAgent> agentList;

            var source = new Source(new ExponentialDistribution(La));
            var serviceBlock = new InfServiceBlocks(new ExponentialDistribution(Mu));

            linkList = new() {
                {
                    source.Id,
                    new() {
                        serviceBlock
                    }
                }
            };

            agentList = new() {
                source,
                serviceBlock
            };

            Problem problem = new()
            {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example M={La}/M={Mu}/Inf",
                Links = linkList,
                MaxModelationTime = MaxSimTime
            };

            problem.AddAgentForStatistic(serviceBlock); //добавила
            return problem;
        }

        /**
         * Метод инициализирует задачу с конечным числом обработчиков.
         * Если значение Q не указано (или установлено в null, то очередь не будет использоваться).
         * Если значение Q указано в 0, то очередь не ограничена.
         */
        /** M/M/n/c */
        /* M=Mu / M=La / n=S / c=Q */
        internal static Problem InitFinServiceBlockProblem(double La = 1, double Mu = 2, int S = 1, int? Q = null, double? MaxSimTime = 10000)
        {
            Dictionary<string, List<IModellingAgent>> linkList;
            List<IModellingAgent> agentList;

            var source = new Source(new ExponentialDistribution(La));
            var serviceBlock = new FinServiceBlocks(S, new ExponentialDistribution(Mu));

            linkList = new() {
                {
                    source.Id,
                    new() {
                        serviceBlock
                    }
                }
            };

            agentList = new() {
                source,
                serviceBlock
            };

            if( Q.HasValue ) {
                var queue = new QueueBuffer( Q.Value ); 
                serviceBlock.BindBuffer( queue );
                agentList.Add( queue );
            }

            Problem problem = new()
            {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example M={La}/M={Mu}/n={S}/c={Q}",
                Links = linkList,
                MaxModelationTime = MaxSimTime
            };

            problem.AddAgentForStatistic(serviceBlock);
            return problem;
        }


    }
}

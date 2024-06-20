using HelpFunctions;
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
        public bool Run_MMSQ_GetModel(out SimulationModeller modeller, double Mu = 1, double La = 2, int S = 1, int Q = 0)
        {
            //если S = 2, 3 - то возникает ошибка
            Problem problem = InitFinServiceBlockProblem(Mu, La, S, Q);

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
        public bool Run_MMinf_GetModel(out SimulationModeller modeller, double Mu = 0.2, double La = 0.5)
        {
            //если S = 2, 3 - то возникает ошибка
            Problem problem = InitInfServiceBlockProblem(Mu, La);

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
        internal static Problem InitInfServiceBlockProblem(double Mu = 0.2, double La = 0.5)
        {
            Dictionary<string, List<IModellingAgent>> linkList;
            List<IModellingAgent> agentList;

            var source = new Source(new ExponentialDistribution(Mu));
            var serviceBlock = new InfServiceBlocks(new ExponentialDistribution(La));

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

            return new()
            {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example M={Mu}/M={La}/Inf",
                Links = linkList,
                MaxModelationTime = 100
            };
        }

        /** Метод инициализирует задачу с конечным числом обработчиков. */
        /** M/M/n/c */
        /* M=Mu / M=La / n=S / c=Q */
        internal static Problem InitFinServiceBlockProblem(double Mu = 1, double La = 2, int S = 1, int Q = 0)
        {
            Dictionary<string, List<IModellingAgent>> linkList;
            List<IModellingAgent> agentList;

            var queue = new QueueBuffer(Q);
            var source = new Source(new ExponentialDistribution(Mu));
            var serviceBlock = new FinServiceBlocks(S, new ExponentialDistribution(La));

            serviceBlock.BindBuffer(queue);

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
                queue,
                serviceBlock
            };

            Problem problem = new()
            {
                Agents = agentList,
                Date = DateTime.Now,
                Name = $"Example M={Mu}/M={La}/n={S}/c={Q}",
                Links = linkList,
                MaxModelationTime = 10000
            };

            problem.AddAgentForStatistic(serviceBlock);
            return problem;
        }





    }
}

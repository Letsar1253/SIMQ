using SimQCore.Modeller.BaseModels;
using System;
using System.Collections.Generic;

namespace SimQCore.Modeller
{
    /// <summary>
    /// Класс представляет Диспетчера СМО.
    /// </summary>
    /// <remarks>
    /// Диспетчер обеспечивает связь между объектами СМО.
    /// Передача заявок от агента к агенту осуществляется внутри данного класса.
    /// </remarks>
    class Supervisor
    {
        /// <summary>
        /// Коллекция методов, вызываемых по наступлению событий.
        /// </summary>
        public Dictionary<string, Func<AgentModel, double, bool>> Actions = new();

        /// <summary>
        /// Коллекцию действующих объектов СМО.
        /// Объекты имеют тип <paramref name="AgentModel" />.
        /// </summary>
        public List<AgentModel> AllAgents;

        private List<AgentModel> _activeModels = new();

        private bool SendCallToServices(Call call, double T)
        {
            foreach (var serviceBlock in AllAgents)
            {
                if (serviceBlock.Type == AgentType.ServiceBlock)
                    if (((ServiceBlock)serviceBlock).TakeCall(call, T)) return true;
            }

            return false;
        }

        /// <summary>
        /// Метод подготавливает <paramref name="Диспетчера"/> к дальнейшей работе.
        /// </summary>
        public void Setup(Problem problem)
        {
            AllAgents = problem.Agents;

            foreach (var agent in AllAgents)
            {
                if (agent.IsActive()) _activeModels.Add(agent);
            }

            Actions["SimpleServiceBlock"] = (agent, T) =>
            {
                Call call = agent.DoEvent(T);

                Console.WriteLine("Модельное время: {0}, агент: {1}, заявка {2} обработана.",
                    T, agent.Id, call.Id);

                return true;
            };

            Actions["SimpleSource"] = (agent, T) =>
            {
                Call call = agent.DoEvent(T);

                Console.WriteLine("Модельное время: {0}, агент: {1}, заявка {2} поступила.",
                    T, agent.Id, call.Id);
               
                return SendCallToServices(call, T);
            };

            // Установление ещё каких-либо настроек диспетчера (в зависимости от задачи)
        }

        /// <summary>
        /// Метод возвращает следующее моделируемое событие.
        /// </summary>
        /// <returns>Следующее событие <paramref name="Event"/></returns>
        /// <param name="arg">The argument to the method</param>
        public Event GetNextEvent()
        {
            AgentModel nextAgent = null;
            double minT = double.PositiveInfinity;

            foreach (var agent in _activeModels)
            {
                double agentEventTime = agent.NextEventTime;
                if (agentEventTime <= minT)
                {
                    minT = agentEventTime;
                    nextAgent = agent;
                }
            }

            if (nextAgent == null) throw new NotSupportedException();

            Event newEvent = new() {
                ModelTimeStamp = minT,
                Agent = nextAgent,
            };
            return newEvent;
        }
    }
}

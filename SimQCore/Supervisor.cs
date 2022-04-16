using SimQCore.BaseModels;
using System;
using System.Collections.Generic;

namespace SimQCore.Supervisors
{
    class Supervisor
    {
        public Dictionary<string, Func<AgentModel, double, bool>> Actions = new();

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
                ModelTime = minT,
                Agent = nextAgent,
            };
            return newEvent;
        }
    }
}

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
        /// Коллекция, содержащая всех активных агентов, используемых в моделировании.
        /// </summary>
        private List<AgentModel> _activeModels = new();

        /// <summary>
        /// Коллекция методов, вызываемых по наступлению событий.
        /// </summary>
        public static Dictionary<string, Func<AgentModel, List<AgentModel>, double, bool>> Actions = new();

        /// <summary>
        /// Коллекция связей агентов. Используется при вызове того или иного события.
        /// Позволяет направлять заявки далее выбранному агенту.
        /// </summary>
        public Dictionary<string, List<AgentModel>> Links { get; set; }

        /// <summary>
        /// Коллекцию действующих объектов СМО.
        /// Объекты имеют тип <paramref name="AgentModel" />.
        /// </summary>
        public List<AgentModel> AllAgents;

        /// <summary>
        /// Метод позволяет добавить в коллекцию новое действие, 
        /// совершаемое при наступлении события агента с тэгом EventTag.
        /// </summary>
        /// <param name="EventTag">Тэг агента, использующийся для определения действия, 
        /// выполняемого при наступлении события.
        /// </param>
        /// <param name="Action">Действие, выполняемое при наступлении события.</param>
        public static void AddAction( string EventTag, 
            Func<AgentModel, List<AgentModel>, double, bool> Action)
        {
            if(!Actions.ContainsKey(EventTag))
                Actions.Add(EventTag, Action);
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

            Links = problem.Links;

            // Установление ещё каких-либо настроек диспетчера (в зависимости от задачи)
        }


        /// <summary>
        /// Метод выполняет действие, совершаемое при возникшем событии.
        /// </summary>
        /// <param name="e">Описание происходящего события.</param>
        /// <param name="ModelTime">Текущее модельное время.</param>
        public void FireEvent(Event e, double ModelTime)
        {
            var Agent = e.Agent;
            var AgentLinks = Links.ContainsKey(Agent.Id) ? Links[Agent.Id] : null;

            Actions[Agent.EventTag](Agent, AgentLinks, ModelTime);
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

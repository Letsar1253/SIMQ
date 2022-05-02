using SimQCore.Modeller.BaseModels;
using System.Collections.Generic;

namespace SimQCore.Modeller
{
    struct Event
    {
        /// <summary>
        /// Модельное время возникновения события.
        /// </summary>
        public double ModelTimeStamp;
        /// <summary>
        /// Агент, вызвавший событие.
        /// </summary>
        public AgentModel Agent;
    }
    struct Problem
    {
        /// <summary>
        /// Время, в течение которого будет выполняться моделирование.
        /// </summary>
        public int? MaxRealTime;
        /// <summary>
        /// Предельное количество поступающих заявок, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationCalls;
        /// <summary>
        /// Максимальное количество шагов, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationSteps;
        /// <summary>
        /// Максимальное модельное время, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationTime;
        /// <summary>
        /// Список агентов, участвующих в системе.
        /// </summary>
        public List<AgentModel> Agents;
        /// <summary>
        /// Список связей для всех существующих агентов.
        /// </summary>
        public Dictionary<string, List<AgentModel>> Links;
    }
}

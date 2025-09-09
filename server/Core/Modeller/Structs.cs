using SimQCore.Modeller.Models;
using SimQCore.Statistic;

namespace SimQCore.Modeller {
    struct Event {
        /// <summary>
        /// Модельное время возникшего события.
        /// </summary>
        public double ModelTimeStamp;
        /// <summary>
        /// Агент, вызвавший событие.
        /// </summary>
        public IModellingAgent Agent;
    }
    public class Problem {
        /// <summary>
        /// Дата создания задачи.
        /// </summary>
        [Obsolete("Относится к метаинформации, необходимо перенести.")]
        public DateTime Date;
        /// <summary>
        /// Наименование задачи.
        /// </summary>
        [Obsolete("Относится к метаинформации, необходимо перенести.")]
        public string Name;
        /// <summary>
        /// Реальное время, в течение которого будет выполняться моделирование (в секундах).
        /// </summary>
        [Obsolete("Относится к параметрам запуска моделирования, необходимо перенести.")]
        public int MaxRealTime = 30 * 60;
        /// <summary>
        /// Максимальное количество событий, при достижении которого моделирование будет окончено.
        /// </summary>
        [Obsolete("Относится к параметрам запуска моделирования, необходимо перенести.")]
        public int MaxEventsAmount = 1_000_000;
        /// <summary>
        /// Максимальное модельное время, при достижении которого моделирование будет окончено.
        /// </summary>
        [Obsolete("Относится к параметрам запуска моделирования, необходимо перенести.")]
        public double MaxModelationTime = 1_000;
        /// <summary>
        /// Настройки вычисления ошибки генерации.
        /// </summary>
        public GenerationErrorSettings generationErrorSettings = new();
        /// <summary>
        /// Список агентов, участвующих в системе.
        /// </summary>
        public List<IModellingAgent> Agents;
        /// <summary>
        /// Список связей для всех существующих агентов.
        /// </summary>
        public Dictionary<string, List<IModellingAgent>> Links;
        public readonly List<IModellingAgent> AgentsForStatistic = [];

        public void AddAgentForStatistic( IModellingAgent agent )  
            => AgentsForStatistic.Add( agent );
        
    }
}

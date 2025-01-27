using SimQCore.Statistic;
using System;

namespace SimQCore.Modeller {
    public class SimulationModeller {
        /// <summary>
        /// Флаг определяет, закончено ли моделирование текущей задачи.
        /// </summary>
        private bool isDone =>
            ( DateTime.Now - StartRealTime ).TotalSeconds >= problem.MaxRealTime
                || dataCollector.isDone;

        /// <summary>
        /// Отметка времени начала моделирования.
        /// </summary>
        private DateTime StartRealTime;

        /// <summary>
        /// Моделируемая задача.
        /// </summary>
        private Problem problem;

        /// <summary>
        /// Экземпляр сборщика результатов.
        /// </summary>
        public DataCollector dataCollector;

        public void Simulate( Problem problem ) {
            this.problem = problem;

            Supervisor supervisor = new( problem );
            dataCollector = new( problem );
            
            Misc.Log( $"Моделирование задачи \"{problem.Name}\" началось.", LogStatus.WARNING );

            StartRealTime = DateTime.Now;
            double lastEventModelationTime = 0;

            while( !isDone ) {
                // Получим следующее событие
                Event nextEvent = supervisor.GetNextEvent();

                // Обращение к сборщику результатов
                dataCollector.AddState( nextEvent.ModelTimeStamp - lastEventModelationTime, problem.AgentsForStatistic );

                lastEventModelationTime = nextEvent.ModelTimeStamp;

                // Запустим событие
                supervisor.FireEvent( nextEvent );
            }

            Misc.Log( "Моделирование окончено.", LogStatus.WARNING );

            dataCollector.GetAllCalls( problem.Agents );
        }
    }
}

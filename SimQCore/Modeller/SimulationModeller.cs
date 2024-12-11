using SimQCore.Statistic;
using System;

namespace SimQCore.Modeller {
    public class SimulationModeller {
        /// <summary>
        /// Флаг определяет, закончено ли моделирование текущей задачи.
        /// </summary>
        private bool isDone =>
            dataCollector.CurrentModelationTime >= problem.MaxModelationTime
                || dataCollector.CurrentEventsAmount >= problem.MaxEventsAmount
                || dataCollector.CurrentGenerationError <= problem.MinGenerationError
                || ( DateTime.Now - StartRealTime ).TotalSeconds >= problem.MaxRealTime;

        /// <summary>
        /// Временная точка начала моделирования.
        /// </summary>
        private DateTime StartRealTime;

        /// <summary>
        /// Экземпляр сборщика результатов.
        /// </summary>
        public DataCollector dataCollector;

        /// <summary>
        /// Моделируемая задача.
        /// </summary>
        public Problem problem;

        public void Simulate( Problem problem ) {
            this.problem = problem;

            Supervisor supervisor = new( problem );
            dataCollector = new( problem.AgentsForStatistic );
            
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

using SimQCore.Statistic;

namespace SimQCore.Modeller {
    class SimulationModeller {
        /// <summary>
        /// Метод проверяет, закончено ли моделирование текущей задачи.
        /// </summary>
        /// <param name="t">Текущее модельное время.</param>
        /// <returns>True - в случае, если моделирование окончено, иначе false.</returns>
        private bool IsDone( double t ) => t >= MaxModelationTime;

        /// <summary>
        /// Экземпляр сборщика результатов.
        /// </summary>
        public DataCollector data;

        /// <summary>
        /// Моделируемая задача.
        /// </summary>
        public Problem problem;

        /// <summary>
        /// Максимальное время моделирования. По умолчанию - 30.
        /// </summary>
        public double MaxModelationTime = 30;

        public void Simulate( Problem problem ) {
            this.problem = problem;

            MaxModelationTime = problem.MaxModelationTime ?? MaxModelationTime;

            Supervisor supervisor = new();
            supervisor.Setup( problem );

            data = new();
            data.SetupStates( problem.Agents );

            Misc.Log( $"Моделирование задачи \"{problem.Name}\" началось.", LogStatus.WARNING );

            double T = 0;
            while( !IsDone( T ) ) {
                Event nextEvent = supervisor.GetNextEvent();

                // В данном сегменте кода должен проходить сбор статистических данных.
                data.AddState( nextEvent.ModelTimeStamp - T, problem.Agents );

                T = nextEvent.ModelTimeStamp;
                supervisor.FireEvent( nextEvent );
            }

            Misc.Log( "\nМоделирование окончено.", LogStatus.WARNING );

            data.GetAllCalls( problem.Agents );
        }
    }
}

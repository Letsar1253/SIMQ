namespace SimQCore.Modeller {
    class SimulationModeller {
        /// <summary>
        /// Метод проверяет, закончено ли моделирование текущей задачи.
        /// </summary>
        /// <param name="t">Текущее модельное время.</param>
        /// <returns>True - в случае, если моделирование окончено, иначе false.</returns>
        private bool IsDone( double t ) => t >= MaxModelationTime;

        /// <summary>
        /// Максимальное время моделирования. По умолчанию - 30.
        /// </summary>
        public double MaxModelationTime = 30;

        public void Simulate( Problem problem ) {
            MaxModelationTime = problem.MaxModelationTime ?? MaxModelationTime;

            Supervisor supervisor = new();
            supervisor.Setup( problem );

            Misc.Log( $"Моделирование задачи \"{ problem.Name }\" началось.", LogStatus.WARNING );

            double T = 0;
            while( !IsDone( T ) ) {
                Event nextEvent = supervisor.GetNextEvent();

                // Для статистических данных.
                //double deltaT = nextEvent.ModelTime - T;

                T = nextEvent.ModelTimeStamp;

                // В данном сегменте кода должен проходить сбор статистических данных.
                //Statistic.SaveState(delta); 

                supervisor.FireEvent( nextEvent );
            }

            Misc.Log( "\nМоделирование окончено.", LogStatus.WARNING );
        }
    }
}

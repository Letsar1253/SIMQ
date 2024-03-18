using SimQCore.Statistic;

namespace SimQCore.Modeller {
    class SimulationModeller {
        /// <summary>
        /// Метод проверяет, закончено ли моделирование текущей задачи.
        /// </summary>
        /// <param name="t">Текущее модельное время.</param>
        /// <returns>True - в случае, если моделирование окончено, иначе false.</returns>
        private bool IsDone( double t ) => t >= MaxModelationTime;

        public DataCollector data;

        /// <summary>
        /// Максимальное время моделирования. По умолчанию - 30.
        /// </summary>
        public double MaxModelationTime = 30;

        public void Simulate( Problem problem ) {
            MaxModelationTime = problem.MaxModelationTime ?? MaxModelationTime;

            Supervisor supervisor = new();
            supervisor.Setup( problem );

            data = new();
            data.SetupStates( problem.Agents );

            Misc.Log( $"Моделирование задачи \"{problem.Name}\" началось.", LogStatus.WARNING );

            double T = 0;
            while( !IsDone( T ) ) {
                Event nextEvent = supervisor.GetNextEvent();

                T = nextEvent.ModelTimeStamp;

                // В данном сегменте кода должен проходить сбор статистических данных.
                data.AddState( nextEvent.ModelTimeStamp - T, problem.Agents );

                supervisor.FireEvent( nextEvent );
            }

            Misc.Log( "\nМоделирование окончено.", LogStatus.WARNING );

            Misc.Log( $"\nСтатистика по результатам моделирования задачи \"{problem.Name}\":", LogStatus.INFO );

            data.GetAllCalls( problem.Agents );
            //data.SaveStates();

            StatisticCollector statistic = new(data);
            statistic.GetAverage();
            statistic.GetCovariance();
            statistic.GetHistogram( 10 );
            statistic.GetEmpiricalDistribution();
            statistic.GetVariance();

            statistic.PrintAverage();
            statistic.PrintCovariance();
            statistic.PrintHistogram();
            statistic.PrintEmpiricalDistribution();
            statistic.PrintVariance();

            //statistic.SaveResult();
        }
    }
}

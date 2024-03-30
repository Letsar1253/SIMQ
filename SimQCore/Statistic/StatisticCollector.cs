using SimQCore.Modeller;
using SimQCore.Modeller.Models;
using System;
using System.Collections.Generic;
//using Newtonsoft.Json;

namespace SimQCore.Statistic {

    /** Интерфейс, реализуемый моделями агентов, позволяющий выводить их собственный набор результатов. */
    interface IResultableModel {
        /** Метод выводит результаты агента. */
        public string GetResult();
    }

    internal class StatisticCollector: DataCollector {
        private List<IModellingAgent> agents;

        public int[] average;
        public double[] variance;
        public int[][] hist;
        public double[][] cov;
        public double[][] empDist;

        public StatisticCollector( SimulationModeller modeller ) {
            _id = Guid.NewGuid().ToString( "N" );
            Date = DateTime.Now;
 
            totalTime = modeller.data.totalTime;
            totalCalls = modeller.data.totalCalls;
            totalStates = modeller.data.totalStates;
            states = modeller.data.states;
            agents = modeller.problem.Agents;
        }

        public static int [] GetMaxCallArray( List<int> [] st, int total ) {
            int[] maxCallArray = new int[st.Length];
            int maxCall;
            for( int i = 0; i < st.Length; i++ ) {
                maxCall = 0;
                for( int j = 0; j < total; j++ ) {
                    if( st [i] [j] > maxCall ) {
                        maxCall = st [i] [j];
                    }
                }
                maxCallArray [i] = maxCall;
            }
            return maxCallArray;
        }

        public void GetAverage() {
            int sum;
            average = new int [states.Length];
            for( int i = 0; i < states.Length; i++ ) {
                sum = 0;
                foreach( int el in states [i] ) {
                    sum += el;
                }
                average [i] = sum / totalStates;
            }
        }

        public void GetCovariance() {
            if( average == null ) {
                GetAverage();
            }

            double sum, item;
            int nodes = states.Length;
            cov = new double [nodes] [];
            for( int i = 0; i < nodes; i++ ) {
                cov [i] = new double [nodes];
            }

            for( int i = 0; i < nodes; i++ ) {
                for( int j = i; j < nodes; j++ ) {
                    sum = 0;
                    for( int k = 0; k < totalStates; k++ ) {
                        item = ( states [i] [k] - average [j] ) * ( states [j] [k] - average [j] );
                        sum += item;
                    }
                    cov [i] [j] = sum / ( totalStates - 1 );
                    cov [j] [i] = cov [i] [j];
                }
            }
        }

        public void GetHistogram( int bins ) {
            int[] binsArray = new int[states.Length];
            int[] maxCallArray = GetMaxCallArray(states, totalStates);
            for( int i = 0; i < maxCallArray.Length; i++ ) {
                binsArray [i] = bins > maxCallArray [i] ? maxCallArray [i] : bins;
            }

            int step, currentStep, stepCount;
            hist = new int [states.Length] [];
            for( int i = 0; i < states.Length; i++ ) {
                step = maxCallArray [i] / binsArray [i];
                hist [i] = new int [binsArray [i]];
                for( int j = 0; j < totalStates; j++ ) {
                    currentStep = 0;
                    stepCount = 0;
                    while( states [i] [j] > ( currentStep + step ) ) {
                        currentStep += step;
                        stepCount++;
                    }

                    if( stepCount > binsArray [i] - 1 ) {
                        stepCount = binsArray [i] - 1;
                    }

                    hist [i] [stepCount]++;
                }
            }
        }

        public void GetEmpiricalDistribution() {
            int[] maxCallArray = GetMaxCallArray(states, totalStates);

            double sum;
            empDist = new double [states.Length] [];
            for( int i = 0; i < states.Length; i++ ) {
                empDist [i] = new double [maxCallArray [i] + 1];
                for( int j = 0; j < empDist [i].Length; j++ ) {
                    sum = 0;
                    for( int k = 0; k < totalStates; k++ ) {
                        if( states [i] [k] <= j ) {
                            sum++;
                        }
                    }
                    empDist [i] [j] = sum / totalStates;
                }
            }
        }

        public void GetVariance() {
            if( average == null ) {
                GetAverage();
            }

            double sum;
            variance = new double [states.Length];
            for( int i = 0; i < variance.Length; i++ ) {
                sum = 0;
                foreach( int el in states [i] ) {
                    sum += ( el - average [i] ) * ( el - average [i] );
                }
                variance [i] = sum / ( totalStates - 1 );
            }
        }

        public void PrintAverage() {
            Console.WriteLine();
            if( average == null || average.Length == 0 ) {
                Console.WriteLine( "Средние значения не определены." );
            } else {
                Console.WriteLine( "Средние значения:" );
                for( int i = 0; i < average.Length; i++ ) {
                    Console.Write( average [i] + " " );
                }

                Console.WriteLine();
            }
        }

        public void PrintVariance() {
            Console.WriteLine();
            if( variance == null || variance.Length == 0 ) {
                Console.WriteLine( "Массив дисперсий не определён." );
            } else {
                Console.WriteLine( "Дисперсии:" );
                for( int i = 0; i < variance.Length; i++ ) {
                    Console.Write( string.Format( "{0:0.00000} ", variance [i] ) );
                }

                Console.WriteLine();
            }
        }

        public void PrintHistogram() {
            Console.WriteLine();
            if( hist == null || hist.Length == 0 ) {
                Console.WriteLine( "Данные для гистограммы не определены." );
            } else {
                Console.WriteLine( "Гистограмма:" );
                for( int i = 0; i < hist.Length; i++ ) {
                    for( int j = 0; j < hist [i].Length; j++ ) {
                        Console.Write( hist [i] [j] + " " );
                    }
                    Console.WriteLine();
                }
            }
        }

        public void PrintCovariance() {
            Console.WriteLine();
            if( cov == null || cov.Length == 0 ) {
                Console.WriteLine( "Ковариационная матрица не определена." );
            } else {
                Console.WriteLine( "Ковариационная матрица:" );
                for( int i = 0; i < cov.Length; i++ ) {
                    for( int j = 0; j < cov [i].Length; j++ ) {
                        Console.Write( string.Format( "{0:0.00000} ", cov[i][j] ) );
                    }
                    Console.WriteLine();
                }
            }
        }

        public void PrintEmpiricalDistribution() {
            Console.WriteLine();
            if( empDist == null || empDist.Length == 0 ) {
                Console.WriteLine( "Данные эмпирической функции распределения не определены." );
            } else {
                Console.WriteLine( "Данные эмпирической функции распределения:" );
                for( int i = 0; i < empDist.Length; i++ ) {
                    for( int j = 0; j < empDist [i].Length; j++ ) {
                        Console.Write( string.Format("{0:0.00000} ", empDist[i][j]) );
                    }
                    Console.WriteLine();
                }
            }
        }

        /** Метод выводит собственные результаты каждого агента. */
        public void PrintAgentsResults() {
            foreach( IModellingAgent item in agents ) {
                if( item is IResultableModel ) {
                    Misc.Log( $"\nРезультаты агента {item.Id}:" );
                    Misc.Log( ( item as IResultableModel ).GetResult() );
                }
            }
        }

        /** Метод формирует данные результатов моделирования и выводит их. */
        public void CalcAndPrintAll() {
            GetAverage();
            GetCovariance();
            // GetHistogram( 10 );
            GetEmpiricalDistribution();
            GetVariance();

            PrintAverage();
            PrintCovariance();
            // PrintHistogram();
            PrintEmpiricalDistribution();
            PrintVariance();

            PrintAgentsResults();
        }

        //public static string LoadResultToJson(string id)
        //{
        //    if (Storage.collection.CollectionNamespace.CollectionName != "Result") Storage.SetCurrentCollection("Result");
        //    var doc = Storage.GetDocument(id);
        //    return doc;
        //}

        //public static StatisticCollector LoadResultToObject(string id)
        //{
        //    if (Storage.collection.CollectionNamespace.CollectionName != "Result") Storage.SetCurrentCollection("Result");
        //    var doc = Storage.GetDocument(id);
        //    var result = JsonConvert.DeserializeObject<StatisticCollector>(doc);
        //    return result;
        //}

        //public void SaveResult()
        //{
        //    if (Storage.collection.CollectionNamespace.CollectionName != "Result") Storage.SetCurrentCollection("Result");
        //    var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        //    Storage.CreateDocument(json);
        //}
    }
}
using SimQCore.Modeller.Models;
using System;
using System.Collections.Generic;
//using Newtonsoft.Json;

namespace SimQCore.Statistic {
    class DataCollector {
        public string _id = Guid.NewGuid().ToString("N");
        public DateTime Date = DateTime.Now;
        public string Name;

        public double totalTime = 0;
        public int totalCalls = 0;
        public int totalStates = 0;
        public List<int>[] states;

        public void SetupStates( List<IModellingAgent> agents ) {
            int bufferCount = 0;
            foreach( IModellingAgent agent in agents ) {
                if( agent.Type == AgentType.BUFFER || agent.Type == AgentType.ORBIT ) {
                    bufferCount++;
                }
            }

            states = new List<int> [bufferCount];

            for( int i = 0; i < states.Length; i++ ) {
                states [i] = new List<int>();
            }
        }

        public void AddState( double deltaT, List<IModellingAgent> agents ) {
            totalTime += deltaT;
            totalStates++;
            int i = 0;
            foreach( IModellingAgent agent in agents ) {
                if( agent.Type == AgentType.BUFFER ) {
                    states [i].Add( ( ( BaseBuffer )agent ).CurrentSize );
                    i++;
                } else if( agent.Type == AgentType.ORBIT ) {
                    states [i].Add( ( ( BaseOrbit )agent ).CurrentSize );
                    i++;
                }
            }
        }

        public void GetAllCalls( List<IModellingAgent> agents ) {
            foreach( IModellingAgent agent in agents ) {
                if( agent.Type == AgentType.SOURCE ) {
                    totalCalls += ( ( BaseSource )agent ).CallsCreated;
                }
            }
        }

        //public static string LoadStatesToJson(string id)
        //{
        //    if( Storage.collection.CollectionNamespace.CollectionName != "States" ) {
        //        Storage.SetCurrentCollection( "States" );
        //    }
        //    return Storage.GetDocument( id );
        //}

        //public static StatisticCollector LoadStatesToObject( string id ) {
        //    if( Storage.collection.CollectionNamespace.CollectionName != "States" )
        //        Storage.SetCurrentCollection( "States" );
        //    var doc = Storage.GetDocument(id);
        //    return JsonConvert.DeserializeObject<StatisticCollector>(doc);
        //}

        //public void SaveStates()
        //{
        //    if (Storage.collection.CollectionNamespace.CollectionName != "States") Storage.SetCurrentCollection("States");
        //    var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        //    Storage.CreateDocument(json);
        //}
    }
}
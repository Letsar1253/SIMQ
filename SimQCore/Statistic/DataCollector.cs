using SimQCore.Modeller.Models;
using System;
using System.Collections.Generic;
//using Newtonsoft.Json;

namespace SimQCore.Statistic {
    public interface IAgentStatistic {
        /** Метод возвращает текущее состояние агента моделирования. */
        public int GetCurrentState();
    }

    class DataCollector {
        public string _id = Guid.NewGuid().ToString("N");
        public DateTime Date = DateTime.Now;
        public string Name;

        public double totalTime = 0;
        public int totalCalls = 0;
        //public int totalStates = 0;
        public Dictionary<IModellingAgent,Dictionary<int,double>> agentsStatisticData = [];

        public DataCollector(List<IModellingAgent> agents)
        {
            SetupStates(agents);
        }

        public void SetupStates(List<IModellingAgent> agents) { 
            if (agents != null)
                foreach (IModellingAgent agent in agents)
                    /*if( agent is IAgentStatistic )*/
                    agentsStatisticData.Add(agent, []);
        }
        

        public void AddState( double deltaT, List<IModellingAgent> agents ) {
            totalTime += deltaT;
            //totalStates++;
            foreach( IModellingAgent agent in agents ) {
                if( agentsStatisticData.ContainsKey(agent) ) {
                    var current_state = (agent as IAgentStatistic).GetCurrentState();
                    if (agentsStatisticData[agent].ContainsKey(current_state)) agentsStatisticData[agent][current_state] += deltaT; 
                    else agentsStatisticData[agent].Add(current_state, deltaT);
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
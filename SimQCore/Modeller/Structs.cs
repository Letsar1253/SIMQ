using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using SimQCore.Modeller.Models;
using System;
using System.Collections.Generic;

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
        /// Идентификатор задачи.
        /// </summary>
        [BsonId]
        public ObjectId _id;
        /// <summary>
        /// Дата создания задачи.
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// Наименование задачи.
        /// </summary>
        public string Name;
        /// <summary>
        /// Реальное время, в течение которого будет выполняться моделирование (в секундах).
        /// </summary>
        public int MaxRealTime = 60 * 30;
        /// <summary>
        /// Максимальное количество событий, при достижении которого моделирование будет окончено.
        /// </summary>
        public int MaxEventsAmount = 1_000_000;
        /// <summary>
        /// Максимальное модельное время, при достижении которого моделирование будет окончено.
        /// </summary>
        public double MaxModelationTime = 1_000;
        /// <summary>
        /// Погрешность генерации, при достижении которой моделирование будет окончено.
        /// </summary>
        public double MinGenerationError = 0.00001;
        /// <summary>
        /// Список агентов, участвующих в системе.
        /// </summary>
        public List<IModellingAgent> Agents;
        /// <summary>
        /// Список связей для всех существующих агентов.
        /// </summary>
        public Dictionary<string, List<IModellingAgent>> Links;
        public readonly List<IModellingAgent> AgentsForStatistic = [];

        public static Problem DeserializeBson( string id ) {
            return BsonSerializer.Deserialize<Problem>( Storage.Storage.GetDocument( id ) );
        }

        public void AddAgentForStatistic( IModellingAgent agent )  
            => AgentsForStatistic.Add( agent );
        
    }
}

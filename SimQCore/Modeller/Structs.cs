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
    class Problem {
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
        /// Время, в течение которого будет выполняться моделирование.
        /// </summary>
        public int? MaxRealTime;
        /// <summary>
        /// Предельное количество поступающих заявок, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxIncomingCalls;
        /// <summary>
        /// Максимальное количество шагов, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationSteps;
        /// <summary>
        /// Максимальное модельное время, при достижении которого моделирование будет окончено.
        /// </summary>
        public double? MaxModelationTime;
        /// <summary>
        /// Список агентов, участвующих в системе.
        /// </summary>
        public List<IModellingAgent> Agents;
        /// <summary>
        /// Список связей для всех существующих агентов.
        /// </summary>
        public Dictionary<string, List<IModellingAgent>> Links;

        public static Problem DeserializeBson( string id ) {
            return BsonSerializer.Deserialize<Problem>( Storage.Storage.GetDocument( id ) );
        }
    }
}

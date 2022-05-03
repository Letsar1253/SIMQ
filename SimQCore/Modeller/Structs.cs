using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using SimQCore.DataBase;
using SimQCore.Modeller.BaseModels;
using System;
using System.Collections.Generic;

namespace SimQCore.Modeller
{
    struct Event
    {
        /// <summary>
        /// Модельное время возникновения события.
        /// </summary>
        public double ModelTimeStamp;
        /// <summary>
        /// Агент, вызвавший событие.
        /// </summary>
        public AgentModel Agent;
    }
    class Problem
    {
        [BsonId]
        public ObjectId _id;
        public DateTime Date;
        public string Name;
        /// <summary>
        /// Время, в течение которого будет выполняться моделирование.
        /// </summary>
        public int? MaxRealTime;
        /// <summary>
        /// Предельное количество поступающих заявок, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationCalls;
        /// <summary>
        /// Максимальное количество шагов, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationSteps;
        /// <summary>
        /// Максимальное модельное время, при достижении которого моделирование будет окончено.
        /// </summary>
        public int? MaxModelationTime;
        /// <summary>
        /// Список агентов, участвующих в системе.
        /// </summary>
        public List<AgentModel> Agents;
        /// <summary>
        /// Список связей для всех существующих агентов.
        /// </summary>
        public Dictionary<string, List<AgentModel>> Links;

        public static Problem DeserializeBson(string id)
        {
            var doc = Storage.GetDocument(id);
            return BsonSerializer.Deserialize<Problem>(doc);
        }
    }
}

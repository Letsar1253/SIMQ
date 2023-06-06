using SimQCore.Modeller.CustomModels;
using MongoDB.Bson.Serialization;

namespace SimQCore.BsonHelper
{
    class SerializerHelper
    {
        public static void DeserializeProblem()
        {
            BsonClassMap.RegisterClassMap<Source>();
            BsonClassMap.RegisterClassMap<ServiceBlock>();
            BsonClassMap.RegisterClassMap<StackBuffer>();
            BsonClassMap.RegisterClassMap<Call>();
            BsonClassMap.RegisterClassMap<QueueBuffer>();
        }
    }
}
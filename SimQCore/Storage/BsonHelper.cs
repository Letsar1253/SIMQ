using SimQCore.Modeller.CustomModels;
using MongoDB.Bson.Serialization;

namespace SimQCore.BsonHelper
{
    class SerializerHelper
    {
        public static void DeserializeProblem()
        {
            BsonClassMap.RegisterClassMap<SimpleSource>();
            BsonClassMap.RegisterClassMap<SimpleServiceBlock>();
            BsonClassMap.RegisterClassMap<SimpleStackBunker>();
            BsonClassMap.RegisterClassMap<SimpleCall>();
            BsonClassMap.RegisterClassMap<SimpleQueueBunker>();
        }
    }
}
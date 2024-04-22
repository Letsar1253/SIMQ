using SimQCore.Modeller.CustomModels;
using MongoDB.Bson.Serialization;
using SimQCore.Modeller;
using SimQCore.Modeller.BaseModels;
using SimQCore.Library.Distributions;
using SimQCore.Library;

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
            BsonClassMap.RegisterClassMap<Orbit>();
            BsonClassMap.RegisterClassMap<PollingServiceBlock>();
            BsonClassMap.RegisterClassMap<FiniteSource>();
            BsonClassMap.RegisterClassMap<Supervisor>();
            BsonClassMap.RegisterClassMap<BaseSource>();
            BsonClassMap.RegisterClassMap<ExponentialDistribution>();
            BsonClassMap.RegisterClassMap<BaseSensor>();
            
        }
    }
}
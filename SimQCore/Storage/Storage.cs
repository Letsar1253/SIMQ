using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using SimQCore.BsonHelper;
using System;
using SimQCore.Modeller;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Collections.Generic;
using System.Threading.Tasks;
using SerializerHelper = SimQCore.BsonHelper.SerializerHelper;

namespace SimQCore.DataBase
{   
    class Storage : IStorage<Problem>
    {
        private const string collectionName = "Problems";
        private static IMongoCollection<Problem> dbCollection;

        private FilterDefinitionBuilder<Problem> filterBuilder = Builders<Problem>.Filter;


        private const string local = "mongodb+srv://zaharkinartur75:0DO7IKFUcfWah8Yk@foodforpowdercluster.n5nxdf5.mongodb.net/";


        public Storage()
        {
            var mongoClient = new MongoClient(local);
            var db = mongoClient.GetDatabase("SimQ");
            dbCollection = db.GetCollection<Problem>(collectionName);
            var objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
            BsonSerializer.RegisterSerializer(objectSerializer);
            SerializerHelper.DeserializeProblem();
        }
        /// <summary>
        /// Получить все задачи
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Problem>> GetAllAsync()
        {

            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();

        }
        /// <summary>
        /// Получить задачу по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Problem> GetByIdAsync(Guid id)
        {
            FilterDefinition<Problem> filter = filterBuilder.Eq(x => x._id, id);
            var problem = await dbCollection.Find(filter).FirstOrDefaultAsync();
            Console.WriteLine(problem.ToJson(new JsonWriterSettings() { Indent = true }));
            return problem;

        }
        /// <summary>
        /// Получить задачу по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Problem> GetByNameAsync(string name)
        {

            FilterDefinition<Problem> filter = filterBuilder.Eq(x => x.Name, name);
            var problem = await dbCollection.Find(filter).FirstOrDefaultAsync();
            Console.WriteLine(problem.ToJson(new JsonWriterSettings() { Indent = true }));
            return problem;
        }
        /// <summary>
        /// Записать задачу в бд
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(Problem problem)
        {
            await dbCollection.InsertOneAsync(problem);
            return true;

        }
        /// <summary>
        /// Изменить задачу
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Problem problem)
        {
            try
            {
                FilterDefinition<Problem> filter = filterBuilder.Eq(x => x._id, problem._id);
                await dbCollection.ReplaceOneAsync(filter, problem);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }
        /// <summary>
        /// Удалить задачу по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<Problem> filter = filterBuilder.Eq(x => x._id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}

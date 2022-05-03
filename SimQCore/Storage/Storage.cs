using MongoDB.Bson;
using MongoDB.Driver;
using SimQCore.BsonHelper;
using MongoDB.Bson.IO;
using System;

namespace SimQCore.DataBase
{
    class Storage
    {
        private MongoClient client;
        private static IMongoDatabase database;
        private static IMongoCollection<BsonDocument> collection;
        private string cluster = "mongodb+srv://super_user:super_user@cluster0.kcuq1.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
        private string local = "mongodb://localhost:27017";

        public Storage()
        {
            var settings = MongoClientSettings.FromConnectionString(cluster);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            client = new MongoClient(settings);
            SetCurrentDataBase("SimQ");
            SetCurrentCollection("Tasks");
            SerializerHelper.DeserializeProblem();
        }

        private void SetCurrentDataBase(string name)
        {
            database = client.GetDatabase(name);
        }

        private void SetCurrentCollection(string name)
        {
            collection = database.GetCollection<BsonDocument>(name);
            Console.WriteLine(collection);
        }

        public void ReadDataBases()
        {
            var dbList = client.ListDatabases().ToList();
            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
        }

        public void ReadCollections()
        {
            var collList = database.ListCollections().ToList();
            Console.WriteLine($"The list of collections on {database} is: ");
            foreach (var coll in collList)
            {
                Console.WriteLine(coll["name"]);
            }
        }

        public void ReadAllDocuments()
        {
            var documents = collection
                .Find(Builders<BsonDocument>.Filter.Empty)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Include("Name").Include("Date"))
                .ToList();
            foreach (var doc in documents)
            {
                Console.WriteLine(doc.ToJson(new JsonWriterSettings { Indent = true }));
            }
        }

        public void ReadDocument(string id)
        {
            var document = collection
                .Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id)))
                .ToList()[0];
            Console.WriteLine(document.ToJson(new JsonWriterSettings { Indent = true }));
        }

        public void CreateDocument(BsonDocument doc)
        {
            collection.InsertOne(doc);
        }

        public void UpdateDocument(string id, BsonDocument doc)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<BsonDocument>.Update.Set("Agents", doc["Agents"]).Set("Date", DateTime.Now);
            collection.UpdateOne(filter, update);
        }


        public void DeleteDocument(string id)
        {
            Console.WriteLine("enter id");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.DeleteOne(filter);
        }

        public static BsonDocument GetDocument(string id)
        {
            var doc = collection
                .Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id)))
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude("_id"))
                .ToList()[0];
            return doc;
        }
    }
}

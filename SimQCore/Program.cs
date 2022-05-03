using MongoDB.Bson;
using SimQCore.DataBase;
using SimQCore.Library;
using SimQCore.Modeller;
using SimQCore.Modeller.BaseModels;
using SimQCore.Modeller.CustomModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimQCore
{
    class Program
    {
        static void Main()
        {
            //Данил
            Random random = new Random();
            Storage db = new Storage();
            string id = "62615b68b332c2974ce4628d";
            //db.ReadAllDocuments();
            //db.ReadDocument(id);
            //Problem problem1 = Problem.DeserializeBson(id);


            // Часть Миши
            Tests.TestTimeGeneration1();

            // Часть Эмиля
            SimulationModeller SM = new();

            SimpleServiceBlock serviceBlock = new();
            SimpleStackBunker simpleStack = new();
            SimpleSource source1 = new();
            SimpleSource source2 = new();

            serviceBlock.BindBunker(simpleStack);

            List<AgentModel> list = new();
            list.Add(source1);
            list.Add(source2);
            list.Add(serviceBlock);
            list.Add(simpleStack);

            Problem problem = new() {
                Agents = list,
                Date = DateTime.Now,
                Name = $"rand {random.Next(100)}",
            };
            //Данил
            db.CreateDocument(problem.ToBsonDocument());

            SM.Simulate(problem);
        }
    }
}
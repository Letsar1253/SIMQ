using MongoDB.Bson;
using SimQCore.DataBase;
using SimQCore.Library;
using SimQCore.Modeller;
using SimQCore.Modeller.BaseModels;
using SimQCore.Modeller.CustomModels;
using System;
using System.Collections.Generic;

namespace SimQCore
{
    class Program
    {
        static void Main()
        {
            // Часть Данила
            //Storage db = new Storage();
            //string id = "62615b68b332c2974ce4628d";
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

            List<AgentModel> AgentList = new();
            AgentList.Add(source1);
            AgentList.Add(source2);
            AgentList.Add(serviceBlock);
            AgentList.Add(simpleStack);

            List<AgentModel> sourceLinks = new();
            sourceLinks.Add(serviceBlock);

            Dictionary<string, List<AgentModel>> LinkList = new();
            LinkList.Add(source1.Id, sourceLinks);
            LinkList.Add(source2.Id, sourceLinks);

            Problem problem = new() {
                Agents = AgentList,
                Date = DateTime.Now,
                Name = $"rand {new Random().Next(100)}",
                Links = LinkList,
            };
            // Часть Данила
            //db.CreateDocument(problem.ToBsonDocument());

            SM.Simulate(problem);
        }
    }
}
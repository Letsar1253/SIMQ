using SimQCore.Modeller.BaseModels;
using System.Collections.Generic;

namespace SimQCore.Modeller
{
    struct Event
    {
        public double ModelTime;
        public AgentModel Agent;
    }
    struct Problem
    {
        public int MaxModelTime;
        public List<AgentModel> Agents;
    }
}

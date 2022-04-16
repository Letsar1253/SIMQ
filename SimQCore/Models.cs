using System.Collections.Generic;

namespace SimQCore.BaseModels
{
    enum AgentType
    {
        Source,
        ServiceBlock,
        Buffer,
        Call
    }

    abstract class AgentModel {
        public abstract string Id { get; set; }
        public abstract Call DoEvent(double T);
        public abstract double NextEventTime { get; }
        public abstract string EventTag { get; }
        public abstract AgentType Type { get; }
        public abstract bool IsActive();
    }

    abstract class Call : AgentModel
    {
        public override string EventTag => "Call";
        public override AgentType Type => AgentType.Call;
    }

    abstract class Source : AgentModel
    {
        private static int _objectCounter;
        public Source() => Id = "SRC_" + _objectCounter++;
        public override string EventTag => "Source";
        public override AgentType Type => AgentType.Source;
    }

    abstract class ServiceBlock : AgentModel
    {
        private static int _objectCounter;
        public ServiceBlock() => Id = "SBLOCK_" + _objectCounter++;
        public override string EventTag => "ServiceBlock";
        public override AgentType Type => AgentType.ServiceBlock;
        public abstract Call ProcessCall { get; }
        public abstract void BindBunker(Buffer buffer);
        public abstract bool TakeCall(Call call, double T);
    }

    abstract class Buffer : AgentModel
    {
        private static int _objectCounter;
        public Buffer() => Id = "BUNK_" + _objectCounter++;
        public abstract bool TakeCall(Call call);
        public abstract Call PassCall();
        public abstract bool IsFull { get; }
        public abstract bool IsEmpty { get; }
        public override AgentType Type => AgentType.Buffer;
    }

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
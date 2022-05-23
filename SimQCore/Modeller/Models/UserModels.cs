using MongoDB.Bson.Serialization.Attributes;
using SimQCore.Library.Distributions;
using SimQCore.Modeller.BaseModels;
using System;
using System.Collections.Generic;

namespace SimQCore.Modeller.CustomModels
{
    class SimpleSource : Source
    {
        [BsonElement]
        private IDistribution _distribution;

        [BsonElement]
        private int _callCounter;

        [BsonElement]
        private string _sourceID;

        [BsonElement]
        private double _tau;

        private Func<AgentModel, List<AgentModel>, double, bool> EventAction = (Agent, Links, T) =>
        {
            Call call = Agent.DoEvent(T);

            Console.WriteLine("Модельное время: {0}, агент: {1}, заявка {2} поступила.",
                T, Agent.Id, call.Id);

            foreach (var serviceBlock in Links)
            {
                if (serviceBlock.Type == AgentType.SERVICE_BLOCK)
                    if (((ServiceBlock)serviceBlock).TakeCall(call, T)) return true;
            }

            return false;
        };

        public SimpleSource() : base()
        {
            _distribution = new PoissonDistribution(0.2);
            _tau = CalcNextEventTime(0);

            Supervisor.AddAction(EventTag, EventAction);
        }

        public override string Id { get => _sourceID; set => _sourceID = value; }
        public override string EventTag => "SimpleSource";

        private Call CreateCall()
        {
            SimpleCall call = new() { Id = "CALL_" + Id + "_" + _callCounter++ };
            return call;
        }

        public override Call DoEvent(double T)
        {
            CalcNextEventTime(T);
            return CreateCall();
        }

        public override double NextEventTime => _tau;

        private double CalcNextEventTime(double T)
        {                
            _tau = T + _distribution.Generate();
            return _tau;
        }

        public override bool IsActive() => true;
    }

    class SimpleServiceBlock : ServiceBlock
    {
        [BsonElement]
        private IDistribution _distribution;

        [BsonElement]
        private double _delta = double.PositiveInfinity;

        [BsonElement]
        private Call _processCall;

        [BsonElement]
        List<BaseModels.Buffer> _bindedBuffers = new();

        [BsonElement]
        private string _serverBlockId;

        private Func<AgentModel, List<AgentModel>, double, bool> EventAction = (Agent, _, T) =>
        {
            Call call = Agent.DoEvent(T);

            Console.WriteLine("Модельное время: {0}, агент: {1}, заявка {2} обработана.",
                T, Agent.Id, call.Id);

            return true;
        };

        public SimpleServiceBlock() : base()
        {
            _distribution = new PoissonDistribution(0.3);

            Supervisor.AddAction(EventTag, EventAction);
        }

        public override Call ProcessCall => _processCall;
        public override string Id { get => _serverBlockId; set => _serverBlockId = value; }
        public bool IsFree => _processCall == null;
        public override string EventTag => "SimpleServiceBlock";
        public override double NextEventTime => _delta;
        public override void BindBuffer(BaseModels.Buffer bunker) => _bindedBuffers.Add(bunker);

        private bool AcceptCall(Call call, double T)
        {
            _processCall = call;
            _delta = gS(T);
            return true;
        }

        private double gS(double T) => T + _distribution.Generate();

        private Call EndProcessCall()
        {
            Call temp = _processCall;
            _processCall = null;
            _delta = double.PositiveInfinity;
            return temp;
        }

        private Call GetCallFromBunker()
        {
            foreach (var buffer in _bindedBuffers)
            {
                if (buffer.IsEmpty) continue;
                return buffer.PassCall();
            }
            return null;
        }

        private void TakeNextCall(double T)
        {
            Call nextCall = GetCallFromBunker();
            if (nextCall != null) AcceptCall(nextCall, T);
        }

        public override Call DoEvent(double T)
        {
            Call temp = EndProcessCall();
            TakeNextCall(T);

            return temp;
        }

        private bool SendToBunker(Call model, double _)
        {
            foreach (var buffer in _bindedBuffers)
            {
                if (buffer.TakeCall(model)) return true;       
            }
            return false;
        }

        public override bool TakeCall(Call call, double T)
        {
            return IsFree ? AcceptCall(call, T) : SendToBunker(call, T);
        }

        public override bool IsActive() => true;
    }

    class SimpleStackBunker : BaseModels.Buffer
    {
        [BsonElement]
        private Stack<Call> _calls = new();

        [BsonElement]
        public override string Id { get; set; }

        public override bool IsEmpty => _calls.Count == 0;
        public override bool IsFull => _calls.Count > 200;

        public override Call PassCall() {
            return IsEmpty ? null : _calls.Pop();
        }

        public override bool TakeCall(Call newCall)
        {
            if (IsFull) return false;
            _calls.Push(newCall);
            return true;
        }

        public override Call DoEvent(double T) => throw new NotImplementedException();
        public override bool IsActive() => false;
        public override double NextEventTime => double.PositiveInfinity;
        public override string EventTag => "SimpleStackBunker";
    }

    class SimpleQueueBunker : BaseModels.Buffer
    {
        [BsonElement]
        private Queue<Call> _calls = new();

        [BsonElement]
        public override string Id { get; set; }

        public override bool IsEmpty => _calls.Count == 0;
        public override bool IsFull => _calls.Count > 200;

        public override Call PassCall()
        {
            return IsEmpty ? null : _calls.Dequeue();
        }

        public override bool TakeCall(Call newCall)
        {
            if (IsFull) return false;
            _calls.Enqueue(newCall);
            return true;
        }

        public override Call DoEvent(double T) => throw new NotImplementedException();
        public override bool IsActive() => false;
        public override double NextEventTime => double.PositiveInfinity;
        public override string EventTag => "SimpleQueueBunker";
    }

    class SimpleCall : Call
    {
        [BsonElement]
        public override string Id { get; set; }
        public override string EventTag => "Call";
        public override Call DoEvent(double T) => throw new NotImplementedException();
        public override bool IsActive() => false;
        public override double NextEventTime => double.PositiveInfinity;
        public override AgentType Type => AgentType.CALL;
    }
}
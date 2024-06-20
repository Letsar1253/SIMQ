﻿namespace SimQCore.Modeller.Models {

    public enum AgentType {
        SOURCE,         // Источник входящих заявок
        SERVICE_BLOCK,  // Блок приборов
        BUFFER,         // Очередь
        CALL,           // Заявка
        ORBIT           // Орбита
    }

    public interface IModellingAgent {
        public string Id {
            get;
        }
        public BaseCall DoEvent( double T );
        public double NextEventTime {
            get;
        }
        public string EventTag {
            get;
        }
        public AgentType Type {
            get;
        }
        public bool IsActive();
    }

    public abstract class BaseSource: IModellingAgent {
        private static int idCounter;
        public virtual string Id {
            get; protected set;
        }
        public abstract double NextEventTime {
            get;
        }
        public abstract string EventTag {
            get;
        }
        public AgentType Type { get; } = AgentType.SOURCE;
        public BaseSource() => Id = "Source_" + idCounter++;
        public abstract BaseCall DoEvent( double T );
        public abstract bool IsActive();
        public abstract int CallsCreated {
            get;
        }
    }

    public abstract class BaseServiceBlock: IModellingAgent {
        private static int idCounter;
        public virtual string Id {
            get; protected set;
        }
        public abstract double NextEventTime {
            get;
        }
        public abstract string EventTag {
            get;
        }
        public AgentType Type { get; } = AgentType.SERVICE_BLOCK;
        public abstract BaseCall ProcessCall {
            get;
        }
        public BaseServiceBlock() => Id = "ServiceBlock_" + idCounter++;
        public abstract BaseCall DoEvent( double T );
        public abstract bool IsActive();
        public abstract bool IsFree();
        public abstract void BindBuffer( BaseBuffer buffer );
        public abstract bool TakeCall( BaseCall call, double T );
    }

    public abstract class BaseBuffer: IModellingAgent {
        private static int idCounter;
        public virtual string Id {
            get; protected set;
        }
        public abstract double NextEventTime {
            get;
        }
        public abstract string EventTag {
            get;
        }
        public AgentType Type { get; } = AgentType.BUFFER;
        public abstract bool IsFull {
            get;
        }
        public abstract bool IsEmpty {
            get;
        }
        public BaseBuffer() => Id = "Buffer_" + idCounter++;
        public abstract bool TakeCall( BaseCall call );
        public abstract BaseCall PassCall();
        public abstract BaseCall DoEvent( double T );
        public abstract bool IsActive();
        public abstract int CurrentSize {
            get;
        }
    }

    public abstract class BaseCall: IModellingAgent {
        private static int idCounter;
        public virtual string Id {
            get; set;
        }
        public abstract double NextEventTime {
            get;
        }
        public abstract string EventTag {
            get;
        }
        public AgentType Type { get; } = AgentType.CALL;
        public BaseCall() => Id = "Call_" + idCounter++;
        public abstract BaseCall DoEvent( double T );
        public abstract bool IsActive();
    }

    public abstract class BaseOrbit: IModellingAgent {
        private static int idCounter;
        public virtual string Id {
            get; protected set;
        }
        public abstract double NextEventTime {
            get;
        }
        public abstract string EventTag {
            get;
        }
        public AgentType Type { get; } = AgentType.ORBIT;
        public BaseOrbit() => Id = "Orbit_" + idCounter++;
        public abstract BaseCall DoEvent( double T );
        public abstract bool IsActive();
        public abstract bool TakeCall( BaseCall call, double T );
        public abstract BaseCall PeekNextCall( double T );
        public abstract int CurrentSize {
            get;
        }
    }
}

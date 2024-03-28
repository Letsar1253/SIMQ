﻿namespace SimQCore.Modeller.Models {

    enum AgentType {
        SOURCE,         // Источник входящих заявок
        SERVICE_BLOCK,  // Блок приборов
        BUFFER,         // Очередь
        CALL,           // Заявка
        ORBIT           // Орбита
    }

    interface IModellingAgent {
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

    abstract class BaseSource: IModellingAgent {
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

    abstract class BaseServiceBlock: IModellingAgent {
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

    abstract class BaseBuffer: IModellingAgent {
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

    abstract class BaseCall: IModellingAgent {
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

    abstract class BaseOrbit: IModellingAgent {
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

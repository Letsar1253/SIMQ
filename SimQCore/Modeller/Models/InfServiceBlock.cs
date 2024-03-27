using SimQCore.Library.Distributions;
using SimQCore.Modeller.BaseModels;
using SimQCore.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimQCore.Modeller.CustomModels {
    /** Структура описывает состояние агента блока приборов в конкретный момент времени. */
    struct InfServiceBlockState {
        /** Момент времени, в который было зафикисировано состояние агента. */
        public double time;
        /** Количество заявок, находящихся в системе. */
        public int callsAmount;
    }

    /** Структура описывает состояние агента блока приборов в конкретный момент времени. */
    struct InfServiceBlockProcess {
        /** Окончание времени обработки заявки. */
        public double processEndTime;
        /** Обрабатываемая заявка. */
        public BaseCall processCall;
    }

    internal class InfServiceBlocks: BaseServiceBlock, IResultableModel {
        private readonly List<InfServiceBlockState> _serviceBlockStates = new ();
        private readonly List<InfServiceBlockProcess> _processes = new ();
        private readonly List<BaseBuffer> _bindedBuffers = new();
        private readonly IDistribution _distribution;
        private readonly Func<IModellingAgent, List<IModellingAgent>, double, bool> EventAction = (Agent, Links, T) => {
            BaseCall call = Agent.DoEvent( T );
            call.DoEvent( T );
            return true;
        };

        public InfServiceBlocks( IDistribution distribution ) : base() {
            _distribution = distribution;
            Supervisor.AddAction( EventTag, EventAction );
        }
        public override double NextEventTime => _processes.Count > 0
            ? _processes.Min( service => service.processEndTime )
            : double.PositiveInfinity;
        public override string EventTag => GetType().Name;
        public override BaseCall ProcessCall => _processes.Aggregate( ( selectedElem, nextElem ) =>
            selectedElem.processEndTime > nextElem.processEndTime
                ? nextElem : selectedElem
        ).processCall;
        public override void BindBuffer( BaseBuffer buffer ) => _bindedBuffers.Add( buffer );
        public override BaseCall DoEvent( double T ) {
            var finishingProcess = _processes.Aggregate( ( selectedElem, nextElem ) =>
                selectedElem.processEndTime > nextElem.processEndTime
                    ? nextElem : selectedElem
            );
            finishingProcess.processCall.DoEvent( T );
            _processes.Remove( finishingProcess );

            _serviceBlockStates.Add( new() {
                time = T,
                callsAmount = _processes.Count,
            } );

            Misc.Log( $"\nМодельное время: {T}, агент: {Id}, заявка {finishingProcess.processCall.Id} обработана.", LogStatus.SUCCESS );

            return finishingProcess.processCall;
        }
        public override bool IsActive() => true;
        public override bool IsFree() => true;
        public override bool TakeCall( BaseCall call, double T ) {
            _processes.Add( new() {
                processEndTime = T + _distribution.Generate(),
                processCall = call
            } );

            _serviceBlockStates.Add( new() {
                time = T,
                callsAmount = _processes.Count,
            } );

            return true;
        }

        public string GetResult() {
            string result = "";
            _serviceBlockStates.ForEach(
                state => result += string.Format( "{0,8} - {1}\n", state.time, state.callsAmount )
            );
            return result;
        }
    }
}

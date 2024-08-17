using SimQCore.Library.Distributions;
using SimQCore.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimQCore.Modeller.Models.UserModels {
    /** Структура описывает состояние агента блока приборов в конкретный момент времени. */
    struct ServiceBlockState {
        /** Момент времени, в который было зафикисировано состояние агента. */
        public double time;
        /** Количество заявок, находящихся в системе. */
        public int callsAmount;
    }

    /** Структура описывает состояние агента блока приборов в конкретный момент времени. */
    struct ServiceBlockProcess {
        /** Окончание времени обработки заявки. */
        public double processEndTime { get; set; }
        /** Обрабатываемая заявка. */
        public BaseCall processCall { get; set; }
    }

    /** Класс представляет блок с неограниченным количеством приборов. */
    internal class InfServiceBlocks: BaseServiceBlock, IResultableModel, IAgentStatistic {
        private readonly List<ServiceBlockState> _serviceBlockStates = new();
        private readonly List<ServiceBlockProcess> _processes = new();
        private readonly List<BaseBuffer> _bindedBuffers = new();
        private readonly IDistribution _distribution;
        private readonly Func<IModellingAgent, List<IModellingAgent>, double, bool> EventAction = (Agent, Links, T) =>
        {
            BaseCall call = Agent.DoEvent(T);
            call.DoEvent(T);
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
                state => result += string.Format( "{0,-9} - {1}\n", state.time.ToString("G8"), state.callsAmount )
            );
            return result;
        }

        /** Состоянием является количество заявок, обрабатывающихся в момент обращения к агенту. */
        public int GetCurrentState() => _processes.Count;
    }

    /** Класс представляет блок с ограниченным количеством приборов (задаётся в конструкторе). */
    internal class FinServiceBlocks: BaseServiceBlock, IResultableModel, IAgentStatistic {
        private readonly List<ServiceBlockState> _serviceBlockStates = new();
        private readonly List<ServiceBlockProcess> _processes = [];
        private readonly List<BaseBuffer> _bindedBuffers = [];
        private readonly IDistribution _distribution;
        private ServiceBlockProcess neareastProcess => _processes.Aggregate( ( selectedElem, nextElem ) =>
            double.IsPositiveInfinity( selectedElem.processEndTime )
                || !double.IsPositiveInfinity( nextElem.processEndTime )
                && selectedElem.processEndTime < nextElem.processEndTime
                    ? selectedElem
                    : nextElem
        );
        private int actualCallsAmount =>
            _processes.FindAll( p => p.processCall != null ).Count
                + _bindedBuffers.Sum( buffer => buffer.CurrentSize );

        /** Метод получает и возвращает заявку из связанных буферов (если имеются). */
        private BaseCall GetCallFromBuffer() {
            foreach( BaseBuffer buffer in _bindedBuffers ) {
                if( buffer.IsEmpty ) {
                    continue;
                }

                return buffer.PassCall();
            }
            return null;
        }

        private bool SendToBuffer( BaseCall call, double _ ) {
            foreach( BaseBuffer buffer in _bindedBuffers ) {
                if( buffer.TakeCall( call ) ) {
                    Misc.Log( $"Заявка {call.Id} попала в буфер {buffer.Id}." );
                    return true;
                }
            }
            return false;
        }

        /** Метод заканчивает обработку ближайшей заявки и возвращает её. */
        private BaseCall EndProcessCall() {
            BaseCall finishedCall = neareastProcess.processCall;

            int processInd = _processes.FindIndex( p => p.Equals( neareastProcess ) );
            _processes[processInd] = new() {
                processEndTime = double.PositiveInfinity,
                processCall = null
            };

            return finishedCall;
        }
        private bool AcceptCall( BaseCall call, double T ) {
            int processInd = _processes.FindIndex( p => p.Equals( neareastProcess ) );
            _processes[processInd] = new() {
                processEndTime = T + _distribution.Generate(),
                processCall = call
            };

            Misc.Log( $"Заявка {call.Id} принята в обработку устройством {processInd} в блоке {Id}." );

            _serviceBlockStates.Add( new() {
                time = T,
                callsAmount = actualCallsAmount,
            } );

            return true;
        }
        private readonly Func<IModellingAgent,List<IModellingAgent>, double,
                              bool> EventAction = (Agent, Links, T) => {
            BaseCall call = Agent.DoEvent(T);
            call.DoEvent(T);
            return true;
        };

        public FinServiceBlocks( int servicesAmount, IDistribution distribution ) : base() {
            for (int i = 0; i < servicesAmount; i++) {
                _processes.Add( new() {
                    processEndTime = double.PositiveInfinity
                } );
            }

            _distribution = distribution;
            Supervisor.AddAction( EventTag, EventAction );
        }
        public override double NextEventTime => neareastProcess.processEndTime;
        public override string EventTag => GetType().Name;
        public override BaseCall ProcessCall => neareastProcess.processCall;
        public override void BindBuffer( BaseBuffer buffer ) => _bindedBuffers.Add( buffer );
        public override BaseCall DoEvent( double T ) {
            BaseCall finishedCall = EndProcessCall();

            Misc.Log( $"\nМодельное время: {T}, агент: {Id}, заявка {finishedCall.Id} обработана.", LogStatus.SUCCESS );

            BaseCall nextCall = GetCallFromBuffer();

            if( nextCall != null ) {
                // Состояние будет записано в этом методе
                AcceptCall( nextCall, T );
            } else {
                _serviceBlockStates.Add( new() {
                    time = T,
                    callsAmount = actualCallsAmount,
                } );
            }

            return finishedCall;
        }
        public override bool IsActive() => true;
        public override bool IsFree() => double.IsPositiveInfinity( neareastProcess.processEndTime );
        public override bool TakeCall( BaseCall call, double T ) => IsFree()
            ? AcceptCall( call, T )
            : SendToBuffer( call, T );
        public string GetResult() {
            string result = "";
            _serviceBlockStates.ForEach(
                state => result += string.Format( "{0,-10} - {1}\n", state.time.ToString( "G11" ), state.callsAmount )
            );
            return result;
        }

        /** Состоянием является количество заявок, обрабатывающихся в момент обращения к агенту. */
        // В учёт не берутся заявки, хранящиеся в буферах, связанных с данным агентом.
        // Если необходим и их учёт, заменить на использование поля actualCallsAmount.
        public int GetCurrentState() => actualCallsAmount;// _processes.FindAll( p => p.processCall != null ).Count;
    }
}

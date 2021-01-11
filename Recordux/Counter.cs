using Recordux.Lib;

namespace Recordux
{
    public record CounterState(int Value) : State;
    
    public abstract record CounterAction : Action;

    public record Increment : CounterAction;
    
    public record Decrement : CounterAction;

    public record IncrementByAmount(int Payload) : CounterAction;

    public record CounterReducer
        : Reducer<CounterState, CounterAction>
    {
        public override CounterState Reduce(CounterState State, CounterAction action)
            => action switch
            {
                Increment x
                    => State with {Value = State.Value + 1},
                Decrement x
                    => State with {Value = State.Value - 1},
                IncrementByAmount x
                    => State with {Value = State.Value + x.Payload},
                _
                    => State
            };
    }
}
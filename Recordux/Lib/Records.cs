namespace Recordux.Lib
{
    public abstract record Action;

    public abstract record State;

    public abstract record Reducer<TState, TAction>
        where TState : State
    {
        public abstract TState Reduce(TState state, TAction action);
    };

    public record Init : Action;
}
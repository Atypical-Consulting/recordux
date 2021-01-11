using System;
using Recordux.Lib;

namespace Recordux
{
    class Program
    {
        static void Main(string[] args)
        {
            var store =  new Store<CounterState, CounterAction>(
                reducer: new CounterReducer(),
                preloadedState: new CounterState(Value: 0));
            
            store.OnStateChanged += (sender, eventArgs)
                => Console.WriteLine($"{eventArgs.OldState} + {eventArgs.Action} = {store.State}");
            
            store.Dispatch(new Increment());
            store.Dispatch(new Increment());
            store.Dispatch(new Decrement());
            store.Dispatch(new IncrementByAmount(10));
            store.Dispatch(new Decrement());
        }
    }
}
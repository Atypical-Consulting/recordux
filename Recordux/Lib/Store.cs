using System;
using System.Collections.Generic;

namespace Recordux.Lib
{
    public class Store<TState, TAction>
        where TState : State
        where TAction : Action
    {
    private Reducer<TState, TAction> _currentReducer;
    private TState _currentState;
    private List<EventHandler<StateChangedEventArgs>> _currentListeners;
    private List<EventHandler<StateChangedEventArgs>> _nextListeners;
    private bool _isDispatching;

    public Store(Reducer<TState, TAction> reducer, TState preloadedState)
    {
        _currentReducer = reducer;
        _currentState = preloadedState;
        _currentListeners = new List<EventHandler<StateChangedEventArgs>>();
        _nextListeners = new List<EventHandler<StateChangedEventArgs>>();
        _isDispatching = false;
    }

    public TState State
    {
        get
        {
            if (_isDispatching)
                throw new Exception("Error");

            return _currentState;
        }
    }

    public TAction Dispatch(TAction action)
    {
        if (_isDispatching)
            throw new Exception("Reducers may not dispatch actions.");

        TState oldState = _currentState;

        try
        {
            _isDispatching = true;
            _currentState = _currentReducer.Reduce(_currentState, action);
        }
        finally
        {
            _isDispatching = false;
        }

        _currentListeners = _nextListeners;
        foreach (var listener in _currentListeners)
        {
            listener(this, new StateChangedEventArgs
            {
                OldState = oldState,
                Action = action,
                When = DateTime.Now
            });
        }

        return action;
    }

    public event EventHandler<StateChangedEventArgs> OnStateChanged
    {
        add
        {
            if (_isDispatching)
                throw new Exception(
                    "You may not call Store.Subscribe() while the reducer is executing.");

            _nextListeners.Add(value);
        }
        remove
        {
            if (_isDispatching)
                throw new Exception(
                    "You may not unsubscribe from a store listener while the reducer is executing.");

            var index = _nextListeners.IndexOf(value);
            _nextListeners.RemoveAt(index);
            _currentListeners = null;
        }
    }
    }
    
    // public class StateChangedEventHandler : EventHandler<StateChangedEventArgs> {}

    public class StateChangedEventArgs : EventArgs
    {
        public State OldState { get; init; }
        public Action Action { get; init; }
        public DateTime When { get; init; }
    }
}
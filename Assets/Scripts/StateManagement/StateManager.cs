using Util;

namespace StateManagement
{
    public class StateManager : Singleton<StateManager>
    {
        private bool _inTransition;
        private State _currentState;
        
        public State CurrentState { get => _currentState; set => Transition(value); }
        
        public void ChangeState<T>() where T : State => CurrentState = GetState<T>();
        
        private T GetState<T>() where T : State => GetComponent<T>() ?? gameObject.AddComponent<T>();

        private void Transition(State value)
        {
            if (_inTransition)
                return;

            _inTransition = true;

            if (_currentState != null)
                _currentState.Exit();

            _currentState = value;

            if (_currentState != null)
                _currentState.Enter();

            _inTransition = false;
        }
    }
}
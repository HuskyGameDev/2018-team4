using UnityEngine;


/// <summary>
/// Base state machine that allows for exlcusive operation of a single state with transitions
/// </summary>
public class StateMachine : MonoBehaviour, IStateUpdate {

    public virtual void IStateUpdate() {
         if (CurrentState != null)
			CurrentState.IStateUpdate();
    }
    
    public virtual void IStateLateUpdate() {
        if (CurrentState != null)
			CurrentState.IStateLateUpdate();
    }
    
    public virtual void IStateFixedUpdate() {
        if (CurrentState != null)
			CurrentState.IStateFixedUpdate();
    }



	/// <summary>
	/// The current state of the machine
	/// </summary>
	/// <value></value>
	public virtual State CurrentState {
		get { return _currentState; }
		set { Transition (value); }
	}

	protected State _currentState;
	protected bool _inTransition;

	/// <summary>
	/// Returns the current state of the machine
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public virtual T GetState<T> () where T : State {
		T target = GetComponent<T>();
		if (target == null)
			target = gameObject.AddComponent<T>();
		return target;
	}
	
	/// <summary>
	/// Transitions the StateMachine to a new state
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public virtual void ChangeState<T> () where T : State {
		CurrentState = GetState<T>();
	}


	/// <summary>
	/// Handles the state transitioning
	/// </summary>
	/// <param name="value"></param>
	protected virtual void Transition (State value) {
		if (_currentState == value || _inTransition)
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
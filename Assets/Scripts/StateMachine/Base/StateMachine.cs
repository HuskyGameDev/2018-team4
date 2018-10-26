using UnityEngine;
using System.Collections.Generic;

namespace StateMachineSystem {
	/// <summary>
	/// Base state machine that allows for exlcusive operation of a single state with transitions
	/// </summary>
	public class StateMachine : MonoBehaviour, IStateUpdate {

		public virtual void IStateUpdate() {
			if (CurrentState != null && _inTransition == false)
				CurrentState.IStateUpdate();
		}
		
		public virtual void IStateLateUpdate() {
			if (CurrentState != null && _inTransition == false)
				CurrentState.IStateLateUpdate();
		}
		
		public virtual void IStateFixedUpdate() {
			if (CurrentState != null && _inTransition == false)
				CurrentState.IStateFixedUpdate();
		}



		/// <summary>
		/// The current state of the machine
		/// </summary>
		/// <value></value>
		public virtual State CurrentState {
			get { return _currentState; }
			set { StartCoroutine(Transition(value)); }
		}

        [SerializeField]
		protected State _currentState;
		protected bool _inTransition;

		/// <summary>
		/// Returns the current state of the machine
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual T GetState<T>() where T : State {
			T target = GetComponent<T>();
			if (target == null)
				target = gameObject.AddComponent<T>();
			target.owner = this;
			return target;
		}
		
		/// <summary>
		/// Transitions the StateMachine to a new state
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public virtual void ChangeState<T>() where T : State {
			CurrentState = GetState<T>();
		}


		/// <summary>
		/// Handles the state transitioning
		/// </summary>
		/// <param name="value"></param>
		protected virtual IEnumerator<object> Transition(State value) {
            while (_inTransition)
                yield return null;

			_inTransition = true;
			
			if (_currentState != null)
				yield return StartCoroutine(_currentState.Exit());
				
			_currentState = value;
            Debug.LogWarning("Transition ("+this.gameObject.name+")-> " +_currentState.GetType().Name);
            
            if (_currentState != null)
                yield return StartCoroutine(_currentState.Enter());
			_inTransition = false;
		}

		/// <summary>
		/// Generates a state machine on an empty game object with the parent specified (null is 'scene') and a name
		/// </summary>
		/// <param name="machineParent"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static StateMachine GenerateMachine(Transform machineParent, string name = "StateMachine") {
			GameObject newMachine = new GameObject();
            newMachine.transform.parent = machineParent;
			newMachine.name = name;
            return (StateMachine)newMachine.AddComponent<StateMachine>();
		}
	}
}	
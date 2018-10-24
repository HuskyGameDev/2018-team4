using UnityEngine;

/// <summary>
/// An abstract class for representing a state within a state machine.
/// </summary>
public abstract class State : MonoBehaviour, IStateUpdate {

    public virtual void IStateUpdate() {
        //Do nothing by default
    }
    
    public virtual void IStateLateUpdate() {
        //Do nothing by default
    }
    
    public virtual void IStateFixedUpdate() {
        //Do nothing by default
    }

    /// <summary>
    /// Code to execute when entering this state
    /// </summary>
    public virtual void Enter() {
		AddListeners();
	}
	
    /// <summary>
    /// Code to execute when exiting this state
    /// </summary>
	public virtual void Exit() {
		RemoveListeners();
	}


    /// <summary>
    /// Code to execute when the State  is destroyed
    /// </summary>
	protected virtual void OnDestroy() {
		RemoveListeners();
	}

    /// <summary>
    /// Add event listeners that are relevant to this state 
    /// </summary>
	protected virtual void AddListeners() {

	}
    
	/// <summary>
	/// Remove event listeners relevant to this state
	/// </summary>
	protected virtual void RemoveListeners() {

	}
}

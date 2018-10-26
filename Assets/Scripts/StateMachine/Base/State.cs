using UnityEngine;
using System.Collections.Generic; 

namespace StateMachineSystem {

    /// <summary>
    /// An abstract class for representing a state within a state machine.
    /// </summary>
    public abstract class State : MonoBehaviour, IStateUpdate {

        public delegate object DelayedInstruction();
        /// <summary>
        /// Special instructions for this state to run based on its conext.
        /// </summary>
        public List<DelayedInstruction> delayedInstructions = new List<DelayedInstruction>();

        public StateMachine owner;

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
        public virtual IEnumerator<object> Enter() {
            AddListeners();
            yield return null;
        }
        
        /// <summary>
        /// Code to execute when exiting this state
        /// </summary>
        public virtual IEnumerator<object> Exit() {
            RemoveListeners();
            yield return null;
        }

        public void Reset() {
            Debug.Log("Reset called");
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
}
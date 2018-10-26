using UnityEngine; 
using System.Collections.Generic; 

namespace StateMachineSystem.CreatedStates{
    public class ActionState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Apply an action.");
            Debug.Log("¿Internal State machine ran by CSL execution?");
            owner.ChangeState<ActionSelectionState>();
            //owner.ChangeState<EndOfTurnState>();
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
       }

        protected override void RemoveListeners() {
            base.RemoveListeners();
        }

       
    }
}
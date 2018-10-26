using UnityEngine; 
using System.Collections.Generic;

namespace StateMachineSystem.CreatedStates{
    public class ChangePlayerState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Select Correct player to change to");
            owner.ChangeState<StartOfTurnState>();
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
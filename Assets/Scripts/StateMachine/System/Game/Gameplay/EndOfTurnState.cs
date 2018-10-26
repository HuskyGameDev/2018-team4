using UnityEngine; 
using System.Collections.Generic; 

namespace StateMachineSystem.CreatedStates{
    public class EndOfTurnState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("End of Turn Effects");
            owner.ChangeState<ChangePlayerState>();
        }

        public void UIUpdate(){
        //Updates any UI Info that is needed 
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
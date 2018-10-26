using UnityEngine; 
using System.Collections.Generic; 

namespace StateMachineSystem.CreatedStates{
    public class ActionSelectionState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

       public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Load list of Items/Actions/RoomActions/¿HauntActions?");
            Debug.Log("Wait for player action selection.");
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(ChangeToAction, "Gameplay->ActionSwitch");
            this.AddObserver(ChangeToEndOfTurn,"Gameplay->EndOfTurnSwitch");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(ChangeToAction, "Gameplay->ActionSwitch");
            this.RemoveObserver(ChangeToEndOfTurn,"Gameplay->EndOfTurnSwitch");
        }

        void ChangeToAction(object sender, object args) {
            owner.ChangeState<ActionState>();
        }
        void ChangeToEndOfTurn(object sender, object args){
            owner.ChangeState<EndOfTurnState>();
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using StateMachineSystem;

namespace StateMachineSystem.CreatedStates{
    public class GamePlayState : State {
        public StateMachine internalStateMachine; 
        public override void IStateUpdate() {
            base.IStateUpdate();
            this.PostNotification("UpdateGameUI");
        }


        public override IEnumerator<object> Enter() {
            yield return base.Enter();

            if (internalStateMachine == null) {
                internalStateMachine = StateMachine.GenerateMachine(this.transform, "Gameplay State Machine");
                internalStateMachine.ChangeState<ChangePlayerState>();
            }
            else {
                yield return StartCoroutine(internalStateMachine.CurrentState.Enter());
            }
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
            yield return StartCoroutine(internalStateMachine.CurrentState.Exit());

        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Destroy(internalStateMachine.gameObject);
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(SwitchToPause, "GameSystem->PauseGame");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(SwitchToPause, "GameSystem->PauseGame");
        }

        void SwitchToPause(object sender, object args) {
            owner.ChangeState<PauseGameState>();
        }
    }
}
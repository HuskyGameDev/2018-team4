using UnityEngine;
using System.Collections.Generic;
using StateMachineSystem;

namespace StateMachineSystem.CreatedStates{
    public class GameSystemState : State {

        public StateMachine internalStateMachine = null; 

        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Enable gameplay UI");
            if (internalStateMachine == null) {
                internalStateMachine = StateMachine.GenerateMachine(this.transform, "GameSystem State Machine");
                internalStateMachine.ChangeState<NetworkInitalizationState>();
            }
            else {
                yield return StartCoroutine(internalStateMachine.CurrentState.Enter());
            }
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
            yield return StartCoroutine(internalStateMachine.CurrentState.Exit());
            Destroy(internalStateMachine);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Destroy(internalStateMachine.gameObject);
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(ChangeToMainMenu, "SoftwareSystem->MainMenuSwitch");
        }

        protected override
         void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(ChangeToMainMenu, "SoftwareSystem->MainMenuSwitch");
        }

        void ChangeToMainMenu(object sender, object args) {
            //Make sure the game is in the paused state when we leave it.
            owner.ChangeState<MainMenuState>();
        }

    }
}
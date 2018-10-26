using UnityEngine;
using System.Collections.Generic;

namespace StateMachineSystem.CreatedStates{
    public class OptionsState : State {

        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            //Wait for base
            yield return base.Enter();
            Debug.Log("Enable options menu UI");
        }

        public override IEnumerator<object> Exit() {
            //Wait for base
            yield return base.Exit();
            Debug.Log("Disable options Menu");
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            Debug.Log("AddListener for Back button ");
            this.AddObserver(OnBackButton, "OptionsBackButton");
            Debug.Log("AddListener for Apply changes button");
            //this.AddObserver(OnApplyButton, "OptionsApplyButton");
            Debug.Log("AddListener for Sub Menu buttons");
            //this.AddObserver(OnSubMenuXButton, "OptionSubMenuXButton");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            Debug.Log("RemoveListener for Back button ");
            this.RemoveObserver(OnBackButton, "OptionsBackButton");
            Debug.Log("RemoveListener for Apply changes button");
            //this.RemoveObserver(OnApplyButton, "OptionsApplyButton");
            Debug.Log("RemoveListener for Sub Menu buttons");
            //this.RemoveObserver(OnSubMenuXButton, "OptionSubMenuXButton");
        }

        void OnBackButton(object send, object args) {
            Debug.Log("Transition to Main menu");
            owner.ChangeState<MainMenuState>();
        }
    }
}
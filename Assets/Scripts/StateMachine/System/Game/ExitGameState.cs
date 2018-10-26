using UnityEngine;
using System.Collections.Generic;
using StateMachineSystem;
using UnityEngine.SceneManagement;

namespace StateMachineSystem.CreatedStates{
    public class ExitGameState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Close down internet connection");
            Debug.Log("Open Save Field Dialogue");//this should be a Coroutine that handles this, and we yeild for it.
            
            SceneManager.LoadScene("dev_Richy"); // Switch back to whatever the main menu scene is
            this.PostNotification("SoftwareSystem->MainMenuSwitch");
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
            //Nothing
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
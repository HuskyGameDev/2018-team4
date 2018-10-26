using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace StateMachineSystem.CreatedStates{
    public class NetworkInitalizationState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Change to the gameplay scene");
            SceneManager.LoadScene("dev2_Richy");
            Debug.Log("In theory, we just conected to an online session maybe.");
            owner.ChangeState<CreateGameState>();
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
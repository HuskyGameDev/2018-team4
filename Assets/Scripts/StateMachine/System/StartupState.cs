using UnityEngine;
using System.Collections.Generic;

namespace StateMachineSystem.CreatedStates {
    public class StartupState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            //Debug.Log("Load Files - Saves, Card Files, images");
            //Debug.Log("¿Connect to online server? - ¿Update from server?");
            //Debug.Log("Initialize CSL - Connect to Card Database");
            //Debug.Log("Load Graphics, Audio, and Controls options");
            //Debug.Log("Suposed to Transition to : Main Menu");
            owner.ChangeState<MainMenuState>();
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
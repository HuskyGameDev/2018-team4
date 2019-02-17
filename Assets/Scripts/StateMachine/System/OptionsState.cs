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
            this.PostNotification("Options Menu Enable");
            Debug.Log("Enable options menu UI");
        }

        public override IEnumerator<object> Exit() {
            //Wait for base
            yield return base.Exit();
            this.PostNotification("Options Menu Disable");
            Debug.Log("Disable options Menu");
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(ShowGameplayMenu, "Gameplay Menu Pressed");
            this.AddObserver(ShowAudioMenu, "Audio Menu Pressed");
            this.AddObserver(ShowControlsMenu, "Controls Menu Pressed");
            this.AddObserver(ShowGraphicsMenu, "Graphics Menu Pressed");
            this.AddObserver(DisableAllSubMenus, "Options Menu Pressed");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(ShowGameplayMenu, "Gameplay Menu Pressed");
            this.RemoveObserver(ShowAudioMenu, "Audio Menu Pressed");
            this.RemoveObserver(ShowControlsMenu, "Controls Menu Pressed");
            this.RemoveObserver(ShowGraphicsMenu, "Graphics Menu Pressed");
            this.RemoveObserver(DisableAllSubMenus, "Options Menu Pressed");
        }

        void OnBackButton(object send, object args) {
            Debug.Log("Transition to Main menu");
            owner.ChangeState<MainMenuState>();
        }
        void ShowGameplayMenu(object send,object args)
        {
            this.PostNotification("Options Menu Disable");
            this.PostNotification("Gameplay Menu Enable");
        }
        void ShowAudioMenu(object send, object args)
        {
            this.PostNotification("Options Menu Disable");
            this.PostNotification("Audio Menu Enable");
        }
        void ShowControlsMenu(object send, object args)
        {
            this.PostNotification("Options Menu Disable");
            this.PostNotification("Audio Menu Enable");
        }
        void ShowGraphicsMenu(object send, object args)
        {
            this.PostNotification("Options Menu Disable");
            this.PostNotification("Audio Menu Enable");
        }
        void DisableAllSubMenus(object send, object args)
        {
            this.PostNotification("Options Menu Enable");
            this.PostNotification("Audio Menu Disable");
            this.PostNotification("Gameplay Menu Disable");
            this.PostNotification("Graphics Menu Disable");
            this.PostNotification("Controls Menu Disable");
            this.PostNotification("Pause Menu Disable");
        }

    }
}
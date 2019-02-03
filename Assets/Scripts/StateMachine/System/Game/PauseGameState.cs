using UnityEngine;
using System.Collections.Generic;

namespace StateMachineSystem.CreatedStates{
    public class PauseGameState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();

            //Forward the update calls to the Gameplay state so the game can keep running in the background
            GamePlayState gmpState = this.gameObject.GetComponent<GamePlayState>();
            if (gmpState != null){
                gmpState.IStateUpdate();
            }
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Show Pause Menu UI");
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
            Debug.Log("Hide Pause Menu UI");
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(SwitchToOptions, "OptionsButtonPressed");
            this.AddObserver(SwitchToLoadGame, "LoadGameButtonPressed");
            this.AddObserver(SwitchToSaveGame, "SaveGameButtonPressed");
            this.AddObserver(SwitchToGameplay, "GameplayButtonPressed");
            this.AddObserver(SwitchToExitGame, "MainMenuButtonPressed");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(SwitchToOptions, "OptionsButtonPressed");
            this.RemoveObserver(SwitchToLoadGame, "LoadGameButtonPressed");
            this.RemoveObserver(SwitchToSaveGame, "SaveGameButtonPressed");
            this.RemoveObserver(SwitchToGameplay, "GameplayButtonPressed");
            this.RemoveObserver(SwitchToExitGame, "MainMenuButtonPressed");
        }

        void SwitchToExitGame(object sender, object args) {
            //Notify SoftwareSystem level state machine we want to back out to the main menu.
            owner.ChangeState<ExitGameState>();
        }

        void SwitchToOptions(object sender, object args) {
            this.PostNotification("Options Menu Enable");
            this.PostNotification("Pause Menu Disable");
            Debug.Log("Pause Menu was told to change to the options state, but didnt because we dont quite have the logic set up for that yet.");
        }
        void SwitchToLoadGame(object sender, object args) {
            owner.ChangeState<LoadGameState>();
        }
        void SwitchToSaveGame(object sender, object args) {
            owner.ChangeState<SaveGameState>();
        }
        void SwitchToGameplay(object sender, object args) {
            owner.ChangeState<GamePlayState>();
        }
    }
}
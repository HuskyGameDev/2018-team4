using UnityEngine;
using System.Collections.Generic;

namespace StateMachineSystem.CreatedStates{
    public class MainMenuState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter(){
            yield return base.Enter();
            Debug.Log("Enable Main menu UI");
        }
        public override IEnumerator<object> Exit() {
           yield return base.Exit();
            Debug.Log("Disable Main menu UI");
        }


        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(GetOut, "MainMenuExitGameButton");
            this.AddObserver(OptionsMenu, "MainMenuOptionsMenuButton");
            this.AddObserver(StartGame, "MainMenuStartGameButton");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(GetOut, "MainMenuExitGameButton");
            this.RemoveObserver(OptionsMenu, "MainMenuOptionsMenuButton");
            this.RemoveObserver(StartGame, "MainMenuStartGameButton");
        }

        public void StartGame(object sender, object args){
            //Activate UI Menu for selecting players and things 
            //SceneManager.LoadScene("dev2_Richy");
            owner.ChangeState<GameSystemState>();
        }

        public void OptionsMenu(object sender, object args)
        {
            owner.ChangeState<OptionsState>();
        }

        public void GetOut(object sender, object args)
        {
            Debug.Log("GTFO");
            Debug.Break();
            Application.Quit();
        }
    }
}
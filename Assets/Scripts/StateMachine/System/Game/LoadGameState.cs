using UnityEngine; 
using System.Collections.Generic; 

namespace StateMachineSystem.CreatedStates{
    public class LoadGameState : State {
        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("Show File Menu");
        }

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
            Debug.Log("Close File Menu");
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(OnLoadFile, "LoadFileButton");
            this.AddObserver(Back, "LoadFileBackButton");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(OnLoadFile, "LoadFileButton");
            this.RemoveObserver(Back, "LoadFileBackButton");
        }

        void Back(object sender, object args) {
            //We are done with this menu
            owner.ChangeState<PauseGameState>();
        }

        void OnLoadFile(object sender, object args) {
            Debug.Log("Create File");
            Debug.Log("Serialize Decks, Players, Board, StoryellingEngine to File");
            Back(sender, args);
        }
    }
}
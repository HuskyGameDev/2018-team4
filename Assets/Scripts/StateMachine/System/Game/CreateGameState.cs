using UnityEngine;
using System.Collections.Generic;

namespace StateMachineSystem.CreatedStates{
    public class CreateGameState : State {

        public StateMachine internalStateMachine = null; 


        public override void IStateUpdate() {
            base.IStateUpdate();
        }

        public override IEnumerator<object> Enter() {
            yield return base.Enter();
            Debug.Log("RESET Gameplay Sub Machine");
            Debug.Log("Generation - Decks, players, StoryTellingEngine");
            Debug.Log("Player Character Dialogue");
            Debug.Log("Load starting area");

            //Make a new object to hold the game board prefabs
            GameManager._instance.boardHolder = new GameObject();

            //[FOR TESTING]Add three prefabs 
			GameManager._instance.gameState.gameBoard.CreateRoom(new HexCoordinate(0,0));
			GameManager._instance.gameState.gameBoard.CreateRoom(new HexCoordinate(1,0));
			GameManager._instance.gameState.gameBoard.CreateRoom(new HexCoordinate(2,0));
            //[END TESTING]
		}

        public override IEnumerator<object> Exit() {
            yield return base.Exit();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        protected override void AddListeners() {
            base.AddListeners();
            this.AddObserver(ChangeToGamePlay, "GameSystem->GamePlay");
        }

        protected override void RemoveListeners() {
            base.RemoveListeners();
            this.RemoveObserver(ChangeToGamePlay, "GameSystem->GamePlay");
        }

        void ChangeToGamePlay(object sender, object args) {
            owner.ChangeState<GamePlayState>();
        }
    }
}
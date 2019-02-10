using UnityEngine;
using StateMachineSystem;

public class GameManager : MonoBehaviour {
    public StateMachine systemStateMachine;
    public static GameManager _instance = null;

    public GameInformation gameState;

	public GameObject boardTilePrefab;

	public GameObject boardHolder;

    public void Start() {
        if (_instance != null) {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

		InputManager.instance.LoadKeybinds();

        DontDestroyOnLoad(this.gameObject);
        gameState = new GameInformation();
        //Create the state machine
        systemStateMachine = StateMachineSystem.StateMachine.GenerateMachine(this.transform, "SoftwareSystemMachine");
        //Change it to the startup state
      systemStateMachine.ChangeState<StateMachineSystem.CreatedStates.StartupState>();
    }


	/// <summary>
	/// Creates a board tile prefab and returns it
	/// </summary>
	/// <returns></returns>
	public BoardTile CreateRoomTilePrefab() {
		GameObject obj = Instantiate(boardTilePrefab);
		obj.transform.parent = boardHolder.transform;
		return obj.GetComponent<BoardTile>();
	}
}
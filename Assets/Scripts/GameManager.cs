using UnityEngine;
using StateMachineSystem;

public class GameManager : MonoBehaviour {
    public StateMachine systemStateMachine;
    public static GameManager _instance = null;
    
    public void Start() {
        if (_instance != null) {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(this.gameObject);
        //Create the state machine
       systemStateMachine = StateMachineSystem.StateMachine.GenerateMachine(this.transform, "SoftwareSystemMachine");
        //Change it to the startup state
      systemStateMachine.ChangeState<StateMachineSystem.CreatedStates.StartupState>();
    }
}
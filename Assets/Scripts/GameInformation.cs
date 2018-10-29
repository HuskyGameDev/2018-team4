using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour {
    private int NumberOfPlayers;
    private Dictionary<string, string> Players;

	// Use this for initialization
	void Start () {
        Players = new Dictionary<string, string>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddPlayer(string name, string character){
        Players.Add(name, character);
        NumberOfPlayers = NumberOfPlayers + 1; 
    }
    public Dictionary<string,string> ListPlayers(){
        return Players;
    }
    public int PlayerCount(){
        return NumberOfPlayers;
    }
}

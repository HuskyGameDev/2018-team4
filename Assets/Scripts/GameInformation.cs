using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation {
    private int NumberOfPlayers;
	private CardDeck roomTileDeck;
    private Dictionary<string, string> Players;

	public Board gameBoard;

	// Use this for initialization
	public GameInformation() {
		Players = new Dictionary<string, string>();
		roomTileDeck = new CardDeck();
        gameBoard = new Board();
		//Add generic cards some cards to the deck
		for (int i = 0; i < 50; i++) {
			roomTileDeck.AddCard(new BoardGameScripting.BGSScript());
		}
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

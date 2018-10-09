using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player object. Holds player stats, the number of dice to roll for stats, the player's location, items, artifacts, status effects, etc.
/// </summary>
public class Player : MonoBehaviour {

    public enum StatType {Spd, Str, Int, San};	// stat types
	//public enum CardType {Item, Artifact, Event, Effect}; // Card types

    private int[] playerStatLevel; // Spd:0, Str:1, Int:2, San:3   Holds the level fir each stat
    private int[,] playerStatScaling; // Spd:0, Str:1, Int:2, San:3   Holds the scaling for each stat at each level
    private bool playerDead = false;	// if the player is dead
    private HexCoordinate mapLocation;	// players location on the map
	private List<string> playerFlags;   // flags on player
	private DynamicContainer playerData;	// data about player

	private CardDeck playerItems;	// player's regular items
    private CardDeck playerArtifacts;	// player's artifact items
    private CardDeck playerEvents;	// player's event thingys
	private CardDeck playerEffects;	// player's effects

	private CardDeck playerItemsUsed; // player's regular items, that have been used this turn
	private CardDeck playerArtifactsUsed; // player's artifact items, that have been used this turn
	private CardDeck playerEventsUsed; // player's eventthingys, that have been used this turn
	private CardDeck playerEffectsUsed; // player's effects, that have been used this turn

	private int playerMovement;	// player's movement remaining for their turn
	private int playerAttack;	// player's attack remaining for this turn

	/* TODO:
		Movement (teleport, animation, statemachine will decide what moement is valid)
		-Store/Give Event/Item/Artifact
		-Get Event/Item/Artifact
		-Discard Event/Item/Artifact
		-Get/Set String Flag (List<String>)
	*/
	
	void Start () {
        //playerStats = new int[4];
        //playerStatDice = new int[4, 8];
        //loadPlayerStats

        playerStatLevel = new int[] {3, 3, 2, 2};
        playerStatScaling = new int[,] {
                                    {4, 4, 4, 4, 5, 6, 8, 8},
                                    {2, 2, 3, 3, 4, 4, 6, 7},
                                    {1, 2, 3, 4, 4, 5, 5, 5},
                                    {3, 4, 5, 5, 6, 6, 7, 8}
                                    };
		playerItems = new CardDeck();
		playerArtifacts = new CardDeck();
		playerEvents = new CardDeck();
		playerEffects = new CardDeck();

		playerItemsUsed = new CardDeck();
		playerArtifactsUsed = new CardDeck();
		playerEventsUsed = new CardDeck();
		playerEffectsUsed = new CardDeck();

		playerFlags = new List<string>();
		playerData = new DynamicContainer();
    }


	/// <summary>
    /// Resets player movement, attack, and items/artifacts/events
    /// </summary>
	public void StartTurn() {
		//TODO : potential movement and attack modifications
		playerMovement = GetStatDice(0);
		playerAttack = 1;

		/*
		while (!playerItemsUsed.Empty()) {
			playerItems.add(playerItemsUsed.remove());
		}
		while (!playerArtifactsUsed.Empty()) {
			playerArtifacts.add(playerArtifactsUsed.remove());
		}
		while (!playerEventsUsed.Empty()) {
			playerEvents.add(playerEventsUsed.remove());
		}*/
	}

    /// <summary>
    /// Returns the value of the given stat.
    /// </summary>
    /// <param name="stat">Stat to return the value of: Spd, Str, Int, or San</param>
    /// <returns>The value of the given stat</returns>
    public int GetStat(StatType stat) {
        switch(stat)
        {
            case StatType.Spd: return playerStatLevel[0];
            case StatType.Str: return playerStatLevel[1];
            case StatType.Int: return playerStatLevel[2];
            case StatType.San: return playerStatLevel[3];
            default: return -1;
        }
    }

    /// <summary>
    /// Sets the value of a stat and returns previous value.
    /// </summary>
    /// <param name="stat">The stat to set the value of: Spd, Str, Int, or San</param>
    /// <param name="statValue">The value to set the stat to</param>
    /// <returns>The previous value of the stat</returns>
    public int SetStat(StatType stat, int statValue) {
        int oldValue = 0;
        switch (stat)
        {
            case StatType.Spd:
                oldValue = playerStatLevel[0];
                playerStatLevel[0] = statValue;
                break;
            case StatType.Str:
                oldValue = playerStatLevel[1];
                playerStatLevel[1] = statValue;
                break;
            case StatType.Int:
                oldValue = playerStatLevel[2];
                playerStatLevel[2] = statValue;
                break;
            case StatType.San:
                oldValue = playerStatLevel[3];
                playerStatLevel[3] = statValue;
                break;
        }
        CheckDead();
        return oldValue;
    }

    /// <summary>
    /// Modifies the value of a stat by the given value, returns the previous value.
    /// </summary>
    /// <param name="stat">The stat to set the value of: Spd, Str, Int, or San</param>
    /// <param name="modifyValue">The value to modify the stat by</param>
    /// <returns>The previous value of the stat</returns>
    public int ModifyStat(StatType stat, int modifyValue)
    {
        int oldValue = 0;
        switch (stat)
        {
            case StatType.Spd:
                oldValue = playerStatLevel[0];
                playerStatLevel[0] += modifyValue;
                break;
            case StatType.Str:
                oldValue = playerStatLevel[1];
                playerStatLevel[1] += modifyValue;
                break;
            case StatType.Int:
                oldValue = playerStatLevel[2];
                playerStatLevel[2] += modifyValue;
                break;
            case StatType.San:
                oldValue = playerStatLevel[3];
                playerStatLevel[3] += modifyValue;
                break;
        }
        CheckDead();
        return oldValue;
    }

    /// <summary>
    /// Gets the number of dice that should be rolled for a given stat.
    /// </summary>
    /// <param name="stat">The stat for which to find the number of dice to roll</param>
    /// <returns>The number of dice to roll</returns>
    public int GetStatDice(StatType stat) {
        switch (stat)
        {
            case StatType.Spd: return playerStatScaling[0, playerStatLevel[0]];
            case StatType.Str: return playerStatScaling[1, playerStatLevel[1]];
            case StatType.Int: return playerStatScaling[2, playerStatLevel[2]];
            case StatType.San: return playerStatScaling[3, playerStatLevel[3]];
            default: return 0;
        }
    }

	/// <summary>
	/// Allows the UI Script to get the scaling for the user to see. 
	/// </summary>
	/// <returns></returns>
	public int[,] StatScaling() {
		int[,] statScalingCopy = new int[4,8];
		System.Array.Copy(playerStatScaling, statScalingCopy, playerStatScaling.Length);
		return statScalingCopy;
		//return playerStatScaling;
	}

	/// <summary>
	/// Checks if the player is dead
	/// </summary>
	/// <returns>True is the player is dead, otherwise false</returns>
	public bool IsDead() {
        CheckDead();
        return playerDead;
    }

    /// <summary>
    /// Sets the player's dead status and returns the previous value
    /// </summary>
    /// <param name="dead">Value to set the player's death status to: true for dead, false for alive</param>
    /// <returns>the previous value</returns>
    public bool SetDead(bool dead) {
        bool oldValue = playerDead;
        playerDead = dead;
        return oldValue;
    }

    /// <summary>
    /// Checks if any of the player's stats are below zero and sets them as dead if they are, and clamps stats to 7 if they are higher than 7
    /// </summary>
    private void CheckDead() {
        for (int i = 0; i < 4; i++) {
            if (playerStatLevel[i] < 0) playerDead = true;
        }
        for (int i = 0; i < 4; i++)
        {
            if (playerStatLevel[i] > 7) playerStatLevel[i] = 7;
        }
    }

    /// <summary>
    /// Get the location of the player
    /// </summary>
    /// <returns></returns>
    public object GetLocation() {
        return mapLocation;
    }
	
	/// <summary>
    /// Returns true if the player has movement left
    /// </summary>
    /// <returns></returns>
	public bool CanMove() {
		if (playerMovement > 0) {
			return true;
		} else {
			return false;
		}
	}
	
	/// <summary>
    /// Returns the amount of movement the player has left
    /// </summary>
    /// <returns></returns>
	public int MoveRemaining() {
		return playerMovement;
	}

	/// <summary>
	/// Returns true if the player has attacks left
	/// </summary>
	/// <returns></returns>
	public bool CanAttack() {
		if (playerAttack > 0) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Returns the number of attacks the player has left
	/// </summary>
	/// <returns></returns>
	public int AttackRemaining() {
		return playerAttack;
	}
	
	/// <summary>
    /// Moves player to given room
    /// </summary>
	public void MovePlayer(HexCoordinate pos, int MoveCost = 1) {
		//TODO

	}

	/* old get player inventory stuff
    /// <summary>
    /// Get the player's list of items
    /// </summary>
    /// <returns></returns>
    public object GetPlayerItems() {
        return playerItems;
    }

    /// <summary>
    /// Get the player's list of artifacts
    /// </summary>
    /// <returns></returns>
    public object GetPlayerArtifacts() {
        return playerArtifacts;
    }

    /// <summary>
    /// Get the player's list of effects
    /// </summary>
    /// <returns></returns>
    public object GetPlayerEvents() {
        return playerEvents;
    }
	
	/// <summary>
    /// Get the player's list of used items
    /// </summary>
    /// <returns></returns>
    public object GetPlayerItemsUsed() {
        return playerItemsUsed;
    }

    /// <summary>
    /// Get the player's list of used artifacts
    /// </summary>
    /// <returns></returns>
    public object GetPlayerArtifactsUsed() {
        return playerArtifactsUsed;
    }

    /// <summary>
    /// Get the player's list of used effects
    /// </summary>
    /// <returns></returns>
    public object GetPlayerEventsUsed() {
        return playerEventsUsed;
    }
	*/

	/// <summary>
	/// Get one of the parts of the players inventory
	/// Either used or unused, items, artifacts, events, or effects
	/// </summary>
	/// <param name="type">Which deck to get from: Item, Artifact, Event, or Effect, with "U_" at the begining for used</param>
	/// <param name="used">true if cards should be added to used part of inventory</param>
	/// <returns></returns>
	public CardDeck GetInventory(CSL.ScriptType type, bool used) {
		if (!used) {
			switch (type) {
				case CSL.ScriptType.Item: return playerItems;
				case CSL.ScriptType.Artifact: return playerArtifacts;
				case CSL.ScriptType.Event: return playerEvents;
				case CSL.ScriptType.Effect: return playerEffects;
				default: return null;
			}
		} else {
			switch (type) {
				case CSL.ScriptType.Item: return playerItemsUsed;
				case CSL.ScriptType.Artifact: return playerArtifactsUsed;
				case CSL.ScriptType.Event: return playerEventsUsed;
				case CSL.ScriptType.Effect: return playerEffectsUsed;
				default: return null;
			}
		}
		
		
	}

	/// <summary>
	/// Adds a card to a given part of the player's inventory
	/// </summary>
	/// <param name="type">Part of inventory to add card to</param>
	/// <param name="used">true if cards should be added to used part of inventory</param>
	/// <param name="card">Card to add</param>
	public void AddCard(CSL.ScriptType type, bool used, /*CSL.Script*/ CardDeck.Card card) {
		//TODO: Automatically detect which part of inventory to put card in
		if (!used) {
			switch (type) {
				case CSL.ScriptType.Item:
					playerItems.AddCard(card);
					break;
				case CSL.ScriptType.Artifact:
					playerArtifacts.AddCard(card);
					break;
				case CSL.ScriptType.Event:
					playerEvents.AddCard(card);
					break;
				case CSL.ScriptType.Effect:
					playerEffects.AddCard(card);
					break;
			}
		} else {
			switch (type) {
				case CSL.ScriptType.Item:
					playerItemsUsed.AddCard(card);
					break;
				case CSL.ScriptType.Artifact:
					playerArtifactsUsed.AddCard(card);
					break;
				case CSL.ScriptType.Event:
					playerEventsUsed.AddCard(card);
					break;
				case CSL.ScriptType.Effect:
					playerEffectsUsed.AddCard(card);
					break;
			}
		}
	}

	/// <summary>
	/// Adds a list of cards to a given part of the player's inventory
	/// </summary>
	/// <param name="type">Part of inventory to add cards to</param>
	/// <param name="used">true if cards should be added to used part of inventory</param>
	/// <param name="cards">Cards to add</param>
	public void AddCards(CSL.ScriptType type, bool used, /*List<CSL.Script>*/ List<CardDeck.Card> cards) {
		//TODO: Automatically detect which part of inventory to put cards in
		if (!used) {
			switch (type) {
				case CSL.ScriptType.Item:
					playerItems.AddCards(cards);
					break;
				case CSL.ScriptType.Artifact:
					playerArtifacts.AddCards(cards);
					break;
				case CSL.ScriptType.Event:
					playerEvents.AddCards(cards);
					break;
				case CSL.ScriptType.Effect:
					playerEffects.AddCards(cards);
					break;
			}
		} else {
			switch (type) {
				case CSL.ScriptType.Item:
					playerItemsUsed.AddCards(cards);
					break;
				case CSL.ScriptType.Artifact:
					playerArtifactsUsed.AddCards(cards);
					break;
				case CSL.ScriptType.Event:
					playerEventsUsed.AddCards(cards);
					break;
				case CSL.ScriptType.Effect:
					playerEffectsUsed.AddCards(cards);
					break;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="type"></param>
	/// <param name="used"></param>
	/// <param name="cards"></param>
	public void DiscardCard(CSL.ScriptType type, bool used, /*CSL.Script*/ CardDeck.Card card) {
		// TODO: detect card type
		if (!used) {
			switch (type) {
				case CSL.ScriptType.Item:
					playerItems.TakeCard(card);
					break;
				case CSL.ScriptType.Artifact:
					playerArtifacts.TakeCard(card);
					break;
				case CSL.ScriptType.Event:
					playerEvents.TakeCard(card);
					break;
				case CSL.ScriptType.Effect:
					playerEffects.TakeCard(card);
					break;
			}
		} else {
			switch (type) {
				case CSL.ScriptType.Item:
					playerItemsUsed.TakeCard(card);
					break;
				case CSL.ScriptType.Artifact:
					playerArtifactsUsed.TakeCard(card);
					break;
				case CSL.ScriptType.Event:
					playerEventsUsed.TakeCard(card);
					break;
				case CSL.ScriptType.Effect:
					playerEffectsUsed.TakeCard(card);
					break;
			}
		}
	}

	/// <summary>
	/// Add a flag to the list, unless it is already in the list
	/// </summary>
	/// <param name="flag">Flag to add</param>
	public void AddFlag(string flag) {
		if (!playerFlags.Contains(flag)) {
			playerFlags.Add(flag);
		}
	}

	/// <summary>
	/// Check if a flag is in the player's list of flags
	/// </summary>
	/// <param name="flag">Flag to check for</param>
	/// <returns></returns>
	public bool HasFlag(string flag) {
		return playerFlags.Contains(flag);
	}
		
	/// <summary>
	/// Remove the given flag from the player's list of flags, return true is succesfull
	/// </summary>
	/// <param name="flag">Flag to remove</param>
	/// <returns></returns>
	public bool RemoveFlag(string flag) {
		return playerFlags.Remove(flag);
	}

	/// <summary>
	/// Sets a value into internal storage via a type and key
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public void SetData<T>(string key, T value) {
		playerData.SetData<T>(key, value);
	}

	/// <summary>
	/// Access a value from storage via a type and key
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key"></param>
	/// <param name="output"></param>
	public void GetData<T>(string key, out T output) {
		playerData.GetData<T>(key, out output);
	}
}

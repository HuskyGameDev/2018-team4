using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player object. Holds player stats, the number of dice to roll for stats, the player's location, items, artifacts, status effects, etc.
/// </summary>
public class Player : MonoBehaviour {

    public enum StatType {Spd, Str, Int, San};

    private int[] playerStats = new int[4];
    private int[,] playerStatDice = new int[4,7];
    private bool playerDead = false;
    private object mapLocation;
    private object playerItems;
    private object playerArtifacts;
    private object playerEffects;

	void Start () {
        //playerStats = new int[4];
        //playerStatDice = new int[4, 7];
        //loadPlayerStats

        playerStats = new int[] {3, 3, 2, 2};
        playerStatDice = new int[,] {
                                    {4, 4, 4, 4, 5, 6, 8, 8},
                                    {2, 2, 3, 3, 4, 4, 6, 7},
                                    {1, 2, 3, 4, 4, 5, 5, 5},
                                    {3, 4, 5, 5, 6, 6, 7, 8}
                                    };
    }

    /// <summary>
    /// Returns the value of the given stat.
    /// </summary>
    /// <param name="stat">Stat to return the value of: Spd, Str, Int, or San</param>
    /// <returns>The value of the given stat</returns>
    public int GetStat(StatType stat) {
        switch(stat)
        {
            case StatType.Spd: return playerStats[0];
            case StatType.Str: return playerStats[1];
            case StatType.Int: return playerStats[2];
            case StatType.San: return playerStats[3];
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
                oldValue = playerStats[0];
                playerStats[0] = statValue;
                break;
            case StatType.Str:
                oldValue = playerStats[1];
                playerStats[1] = statValue;
                break;
            case StatType.Int:
                oldValue = playerStats[2];
                playerStats[2] = statValue;
                break;
            case StatType.San:
                oldValue = playerStats[3];
                playerStats[3] = statValue;
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
                oldValue = playerStats[0];
                playerStats[0] += modifyValue;
                break;
            case StatType.Str:
                oldValue = playerStats[1];
                playerStats[1] += modifyValue;
                break;
            case StatType.Int:
                oldValue = playerStats[2];
                playerStats[2] += modifyValue;
                break;
            case StatType.San:
                oldValue = playerStats[3];
                playerStats[3] += modifyValue;
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
            case StatType.Spd: return playerStatDice[0, playerStats[0]];
            case StatType.Str: return playerStatDice[1, playerStats[1]];
            case StatType.Int: return playerStatDice[2, playerStats[2]];
            case StatType.San: return playerStatDice[3, playerStats[3]];
            default: return 0;
        }
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
            if (playerStats[i] < 0) playerDead = true;
        }
        for (int i = 0; i < 4; i++)
        {
            if (playerStats[i] > 7) playerStats[i] = 7;
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
    public object GetPlayerEffects() {
        return playerEffects;
    }
}

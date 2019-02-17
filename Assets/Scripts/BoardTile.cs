using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGameScripting;

public class BoardTile : MonoBehaviour {
	
	/// <summary>
	/// How much this room has been rotated.
	/// </summary>
	public int rotationAmout = 0;

	/// <summary>
	/// Array holding the current status of the doors.
	/// </summary>
	public bool[] doors = new bool[] { true, true, true, true, true, true };

	/// <summary>
	/// The sprite renderer for this tile
	/// </summary>
	public SpriteRenderer sprite;

	/// <summary>
	/// The associated card script for this tile
	/// </summary>
	//public BGSScript cardScript;


}

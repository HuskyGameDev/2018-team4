using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    Dictionary<HexCoordinate, BoardTile> _map = new Dictionary<HexCoordinate, BoardTile>();

    public void AddTile(HexCoordinate hex, BoardTile tile)
    {
        _map.Add(hex, tile);
    }
    //Two methods
        //check if there is a tile adacent to another tiles
        //return all tiles adjacent to a tile

    public bool isNeighbor(HexCoordinate hex1, HexCoordinate hex2) //Are these two tiles adjacent?
    {
        return (hex1 - hex2).Magnitude()<=1;
    }

    public bool canMove(HexCoordinate hex1, HexCoordinate hex2){ //return all the sides that both have doors

        if (isNeighbor(hex1, hex2))
        {
           
            return true;
        }
        else return false;

    }

    public bool hasNeighbor(HexCoordinate hex)
    {
        HexCoordinate temp; 
        foreach (Vector2Int offset in HexCoordinate.offset)
        {
            temp = hex + offset;
            if (_map.ContainsKey(temp))
            {
                return true;
            }
        }
       return false; 
    }

	public bool CanCreateRoom(HexCoordinate location) {
		//If the map does not contain the key, then we can place here
		return _map.ContainsKey(location) == false;
	}


	public bool CreateRoom(HexCoordinate location) {
		//Check if we can make a room here first
		if (!CanCreateRoom(location))
			//Break out if we cannot
			return false;

		BoardTile newTile = GameManager._instance.CreateRoomTilePrefab();

		newTile.gameObject.transform.position = HexCoordinate.GetWorldPositionFromHex(location);

		_map.Add(location, newTile);

		return true;
	}
}

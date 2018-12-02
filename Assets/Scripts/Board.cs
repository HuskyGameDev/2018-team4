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
        return (hex1 - hex2).Magnitude()<=1; //magnitude function is always positive
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
		return !_map.ContainsKey(location);
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
    public bool canMove(HexCoordinate hex1, HexCoordinate hex2)
    { //return if the two tiles are adjacent and have doors in the correct spots for movement
        BoardTile b1 = _map[hex1];
        BoardTile b2 = _map[hex2];
        if (isNeighbor(hex1, hex2)) {
            Vector2Int offset = hex2-hex1;
            int dir = (int)HexCoordinate.GetDirectionFromOffset(offset);

            return b2.doors[dir] && b1.doors[(int)HexCoordinate.inverseDirection[dir]];
          
        }
        else return false;

    }
}

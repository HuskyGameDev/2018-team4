using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public class Tile
    {

    }


    Dictionary<HexCoordinate, Tile> _map = new Dictionary<HexCoordinate, Tile>();

    public void AddTile(HexCoordinate hex,Tile tile)
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



}

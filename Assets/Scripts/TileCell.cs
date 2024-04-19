using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a cell in the grid
public class TileCell : MonoBehaviour
{
    // This is the position of the cell in the grid
    public Vector2Int postition {get; set;}
    public Tile tile {get; set;}
    
    // These properties are used to determine if the cell is empty or has a tile
    public bool empty => tile == null;
    public bool hasTile => tile != null;
}

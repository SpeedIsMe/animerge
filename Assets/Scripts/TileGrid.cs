using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class keeps track of the rows and cells in the grid
public class TileGrid : MonoBehaviour
{
    public TileRow[] rows {get; private set;}
    public TileCell[] cells {get; private set;}
    
    
    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;
    
    public void Awake()
    {
        // Get all the rows and cells in the grid
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    public void Start()
    {
        // Set the position of each cell in the grid
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].postition = new Vector2Int(x, y);
            }
        }
    }
}

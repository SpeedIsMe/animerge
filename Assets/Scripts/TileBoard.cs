using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;

    private TileGrid grid;
    private List<Tile> tiles;
    private bool isMoving;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void CreateTile()
    {
        var tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], @"🐭");
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
        }
    }
    
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for(int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for(int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                var cell = grid.Getcell(x, y);

                if (cell.hasTile)
                {
                   changed |= MoveTile(cell.tile, direction);
                } 
            }
        }
        
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);
        
        while (adjacentCell != null)
        {
            if (adjacentCell.hasTile)
            {
                //TODO: merge tiles
                break;
            }
            
            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }
        
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        
        return false;
    }

    private IEnumerator WaitForChanges()
    {
        isMoving = true;
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
        //TODO: create new tile
        //TODO: check for game over
    }
}
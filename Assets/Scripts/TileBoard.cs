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
                if (CanMerge(tile, adjacentCell.tile))
                {
                    MergeTiles(tile, adjacentCell.tile);
                    return true;
                }
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
    
    private bool CanMerge(Tile a, Tile b)
    {
        return a.emoji == b.emoji && !b.locked;
    }

    private void MergeTiles(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);
        
        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        string emoji = DecideEmoji(b.emoji);
        
        b.SetState(tileStates[index], emoji);
    }

    private string DecideEmoji(string emoji)
    {
        string newEmoji = "";
        
        switch (emoji)
        {
            case "🐭":
                newEmoji = "🐰";
                break;
            case "🐰":
                newEmoji = "🐱";
                break;
            case "🐱":
                newEmoji = "🦝";
                break;
            case "🦝":
                newEmoji = "🐶";
                break;
            case "🐶":
                newEmoji = "🦊";
                break;
            case "🦊":
                newEmoji = "🐺";
                break;
            case "🐺":
                newEmoji = "🐵";
                break;
            case "🐵":
                newEmoji = "🐼";
                break;
            case "🐼":
                newEmoji = "🐻";
                break;
            case "🐻":
                newEmoji = "🐘";
                break;
            default:
                newEmoji = emoji;
                break;
        }
        return newEmoji;
    }
    
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (tileStates[i] == state)
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        isMoving = true;
        yield return new WaitForSeconds(0.1f);
        isMoving = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count < grid.size)
        {
            CreateTile();
        }
        
        //TODO: check for game over
    }
}
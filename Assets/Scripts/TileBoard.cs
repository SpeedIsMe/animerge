using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;
    public Tile tilePrefab;
    public TileState[] tileStates;

    private TileGrid grid;
    private bool isMoving;
    private List<Tile> tiles;

    private void Awake()
    {
        // get the TileGrid component from the children of the TileBoard object
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

    private void Update()
    {
        // this block of code checks for the player input and moves the tiles accordingly
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
        }
    }

    // this function is called to clear the board
    public void ClearBoard()
    {
        foreach (var cell in grid.cells) cell.tile = null;

        foreach (var tile in tiles) Destroy(tile.gameObject);

        tiles.Clear();
    }

    // this function is called to create a new tile
    public void CreateTile()
    {
        var tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], @"🐸");
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    // this function is called to move all tiles in a direction and pace of the game
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        var changed = false;
        for (var x = startX; x >= 0 && x < grid.width; x += incrementX)
        for (var y = startY; y >= 0 && y < grid.height; y += incrementY)
        {
            var cell = grid.Getcell(x, y);

            if (cell.hasTile) changed |= MoveTile(cell.tile, direction);
        }

        if (changed) StartCoroutine(WaitForChanges());
    }
    
    // this function is handles the movement of an individual tile and logic for merging tiles
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        var adjacentCell = grid.GetAdjacentCell(tile.cell, direction);
        
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

    // this function is called to check if two tiles can be merged
    private bool CanMerge(Tile a, Tile b)
    {
        return a.emoji == b.emoji && !b.locked;
    }

    // this function is called to merge two tiles
    private void MergeTiles(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        var index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        var emoji = DecideEmoji(b.emoji);

        b.SetState(tileStates[index], emoji);
    }

    // this function is called to decide the emoji of the new tile
    private string DecideEmoji(string emoji)
    {
        var newEmoji = "";

        switch (emoji)
        {
            case "🐸":
                newEmoji = "🐭";
                break;
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

    // this function is called to get the index of a tile state
    private int IndexOf(TileState state)
    {
        for (var i = 0; i < tileStates.Length; i++)
            if (tileStates[i] == state)
                return i;

        return -1;
    }

    // this function is called to wait for the tiles to finish moving and check for game over or win
    private IEnumerator WaitForChanges()
    {
        isMoving = true;
        yield return new WaitForSeconds(0.1f);
        isMoving = false;

        foreach (var tile in tiles) tile.locked = false;

        if (tiles.Count < grid.size) CreateTile();

        if (CheckForGameOver()) gameManager.GameOver();

        if (CheckForWin()) gameManager.Winner();
    }

    // this function handles the logic for checking if the player has won the game
    private bool CheckForWin()
    {
        foreach (var tile in tiles)
            if (tile.emoji == "🐘")
                return true;

        return false;
    }

    // this function handles the logic for checking if the player has lost the game
    private bool CheckForGameOver()
    {
        if (tiles.Count != grid.size) return false;

        foreach (var tile in tiles)
        {
            var up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            var down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            var left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            var right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile)) return false;
            if (down != null && CanMerge(tile, down.tile)) return false;
            if (left != null && CanMerge(tile, left.tile)) return false;
            if (right != null && CanMerge(tile, right.tile)) return false;
        }

        return true;
    }
}
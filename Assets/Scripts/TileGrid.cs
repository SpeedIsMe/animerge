using UnityEngine;

// This class keeps track of the rows and cells in the grid
public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }


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
        for (var y = 0; y < rows.Length; y++)
        for (var x = 0; x < rows[y].cells.Length; x++)
            rows[y].cells[x].postition = new Vector2Int(x, y);
    }

    public TileCell GetRandomEmptyCell()
    {
        var index = Random.Range(0, size);
        var startIndex = index;

        while (cells[index].hasTile)
        {
            index++;

            if (index >= size) index = 0;

            if (index == startIndex) return null;
        }

        return cells[index];
    }
}
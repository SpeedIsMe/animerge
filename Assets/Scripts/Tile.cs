using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this class represents the tile object in the game
public class Tile : MonoBehaviour
{
    private Image background;
    private TextMeshProUGUI text;
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }
    // this property is used to store the emoji of the tile
    public string emoji { get; private set; }
    // this property is used to lock the movement of the tile when it is animating
    public bool locked { get; set; }

    public void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    // this function is called to set the state of the tile
    public void SetState(TileState state, string emoji)
    {
        this.state = state;
        this.emoji = emoji;

        background.color = state.backgroundColor;
        text.text = emoji;
    }

    // this function is called to spawn the tile in a cell
    public void Spawn(TileCell cell)
    {
        if (this.cell != null) this.cell.tile = null;


        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    // this function is called to move the tile to a cell
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null) this.cell.tile = null;

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    // this function is called to merge the tile with another tile
    public void Merge(TileCell cell)
    {
        if (this.cell != null) this.cell.tile = null;

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

    // this function is called to animate the tile
    public IEnumerator Animate(Vector3 to, bool merging)
    {
        var elapsedTime = 0f;
        var duration = 0.1f;

        var from = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging) Destroy(gameObject);
    }
}
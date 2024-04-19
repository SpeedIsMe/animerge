using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private Image background;
    private TextMeshProUGUI text;
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }
    public string emoji { get; private set; }
    public bool locked { get; set; }

    public void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state, string emoji)
    {
        this.state = state;
        this.emoji = emoji;

        background.color = state.backgroundColor;
        text.text = emoji;
    }

    public void Spawn(TileCell cell)
    {
        if (this.cell != null) this.cell.tile = null;


        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null) this.cell.tile = null;

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.cell != null) this.cell.tile = null;

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

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
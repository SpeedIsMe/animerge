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
}
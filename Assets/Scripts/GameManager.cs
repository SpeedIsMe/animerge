using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameOver;
    public CanvasGroup winner;

    private void Start()
    {
        NewGame();
    }
    
    public void NewGame()
    {
        gameOver.alpha = 0f;
        gameOver.interactable = false;
        gameOver.gameObject.SetActive(true);
        winner.alpha = 0f;
        winner.interactable = false;

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 0f, 1f, 1f));
    }

    public void Winner()
    {
        board.enabled = false;
        winner.interactable = true;
        gameOver.gameObject.SetActive(false);

        StartCoroutine(Fade(winner, 0f, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        var time = 0f;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}
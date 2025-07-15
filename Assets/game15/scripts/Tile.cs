using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public int number;
    public Vector2Int position;

    private PuzzleManager manager;

    public void Init(int num, Vector2Int pos, PuzzleManager mgr, Sprite image)
    {
        number = num;
        position = pos;
        manager = mgr;

        GetComponent<Image>().sprite = image;
    }

    public void OnClick()
    {
        manager.TryMove(this);
    }
}
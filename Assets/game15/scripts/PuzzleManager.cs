using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    public GridLayoutGroup glg;
    public TMP_Text timetext;

    public Sprite[] tileImages; // 15 ta rasm bo‘lagi (Inspector’dan ulanadi)

    private Tile[,] tiles;
    private Vector2Int emptyPos = new Vector2Int(3, 3);

    public int alar;
    public int blar;
    Transform tt;
    public int level;

    bool boshshlandi;
    int sx,sy;
    void Start()
    {
        if(PlayerPrefs.GetInt("ekran")==1){
            sx=Screen.width;sy=Screen.height;
        }else{
            sx=1400;sy=1050;
        }
        if(PlayerPrefs.HasKey("level")){
            level=PlayerPrefs.GetInt("level");
            level--;
        }else{
            level=1;PlayerPrefs.SetInt("level",2);
        }
        alar=tileImages.Length/blar;
        glg.constraintCount=alar;
        glg.cellSize=new Vector2Int(sx/alar,sy/blar);
        tiles = new Tile[alar, blar];
        InitTiles();
        Shuffle();
        StartCoroutine(sanoqi());
    }

    void InitTiles()
    {
        int count = 1;

        for (int y = 0; y < blar; y++)
        {
            for (int x = 0; x < alar; x++)
            {
                if (count <= alar*blar-1)
                {
                    GameObject tileGO = Instantiate(tilePrefab, gridParent);
                    Tile tile = tileGO.AddComponent<Tile>();
                    tile.Init(count, new Vector2Int(x, y), this, tileImages[count - 1]);
                    Button btn = tileGO.GetComponent<Button>();
                    btn.onClick.AddListener(tile.OnClick);

                    tiles[x, y] = tile;
                    count++;
                    
                }
                else
                {
                    tt = Instantiate(tilePrefab, gridParent).transform;
                    tiles[x, y] = null; // Bo'sh katak
                    emptyPos = new Vector2Int(x, y);
                    tt.SetSiblingIndex(emptyPos.y * alar + emptyPos.x);
                }
            }
        }
    }

    public void TryMove(Tile tile)
    {
        Vector2Int pos = tile.position;

        if ((Mathf.Abs(pos.x - emptyPos.x) == 1 && pos.y == emptyPos.y) ||
            (Mathf.Abs(pos.y - emptyPos.y) == 1 && pos.x == emptyPos.x))
        {
            tiles[emptyPos.x, emptyPos.y] = tile;
            tiles[pos.x, pos.y] = null;

            tile.transform.SetSiblingIndex(emptyPos.y * alar + emptyPos.x);

            Vector2Int oldPos = tile.position;
            tile.position = emptyPos;
            emptyPos = oldPos;
            tt.SetSiblingIndex(emptyPos.y * alar + emptyPos.x);
            if(boshshlandi){
                tekshir();
            }
        }else{
            Debug.Log(Mathf.Abs(pos.x - emptyPos.x)+"sad"+Mathf.Abs(pos.y - emptyPos.y));
        }
    }

    void Shuffle()
    {
        // Oddiy shuffle: ko‘plab random harakatlar
        for (int i = 0; i < 100; i++)
        {
            List<Tile> movable = new List<Tile>();

            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    Vector2Int pos = tile.position;
                    if ((Mathf.Abs(pos.x - emptyPos.x) == 1 && pos.y == emptyPos.y) ||
                        (Mathf.Abs(pos.y - emptyPos.y) == 1 && pos.x == emptyPos.x))
                    {
                        movable.Add(tile);
                    }
                }
            }

            if (movable.Count > 0)
            {
                Tile t = movable[Random.Range(0, movable.Count)];
                TryMove(t);
            }
        }
        boshshlandi=true;
    }
    void randomer(int x,int y,int x1,int y1){

    }
    void tekshir(){
        int maks=0;
        for (int y = 0; y < blar; y++)
        {
            for (int x = 0; x < alar; x++)
            {
                if(tiles[x,y]!=null){
                if(tiles[x,y].number<maks){
                    return;
                }else{
                    maks=tiles[x,y].number;
                }
                }else{
                    if(maks!=alar*blar-1){
                        return;
                    }
                }
            }
        }
        Debug.Log("Siz yutdingiz");
        winf();
    }
    void winf(){
        Debug.Log(sanoq);
        nextScene();
    }
    void gOver(){
        Debug.Log("Yutqazdiz");
        SceneManager.LoadScene(1);
    }
    public int sanoq;
    IEnumerator sanoqi(){
        while(true){
            yield return new WaitForSeconds(1f);
            sanoq--;
            timetext.text=sanoq.ToString();
            if(sanoq<=0){
                gOver();
            }
        }
    }
    public void nextScene(){
        PlayerPrefs.SetInt("level",SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void PlayGame(){
        //PlayerPrefs.DeleteAll(); // Barcha ma’lumotlarni o‘chiradi
        //PlayerPrefs.Save();
        PlayerPrefs.Save();
        SceneManager.LoadScene(level+1);
    }
    public void Quit(){
        Application.Quit();
    }
    public void gotoMM(){
        SceneManager.LoadScene(0);
    }
}
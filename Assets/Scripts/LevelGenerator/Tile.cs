using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Range(0,100)]
    public int spawnProbability = 100;
    public int x { get; private set; }
    public int y { get; private set; }

    private SpriteRenderer spriteRenderer;

    public bool hasDecorations;

    public GameObject[] decorationUp;
    public GameObject[] decorationDown;
    public GameObject[] decorationRight;
    public GameObject[] decorationLeft;

    public Sprite[] alter;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteUpDown;

    public const int Width = 16;
    public const int Height = 16;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void InitTile(int x,int y)
    {
        this.x = x;
        this.y = y;
        gameObject.name = gameObject.name + "[" + x + "," + y + "]";
        // Khoi tao sprite
        LevelGeneration.instance.Tiles[x, y] = this;
    }

    public void SetupTile()
    {
        if (!hasDecorations)
        {
            return;
        }
        bool up = false;
        bool right = false;
        bool down = false;
        bool left = false;
        //Kiem tra neu chung ta co dirt 4 huong
        //mang 2 chieu nen xet x>0 y>0 x<sohang y<socot 
        if (y < LevelGeneration.instance.Tiles.GetLength(1) - 1 && (LevelGeneration.instance.Tiles[x, y + 1] == null || !LevelGeneration.instance.Tiles[x, y + 1].hasDecorations))
        {
            up = true;
        }
        if (x < LevelGeneration.instance.Tiles.GetLength(0) - 1 && (LevelGeneration.instance.Tiles[x + 1, y] == null || !LevelGeneration.instance.Tiles[x + 1, y].hasDecorations))
        {          
            right = true;
        }
        if (y > 0 && (LevelGeneration.instance.Tiles[x, y - 1] == null || !LevelGeneration.instance.Tiles[x, y - 1].hasDecorations))
        {
            down = true;
        }
        if (x > 0 && (LevelGeneration.instance.Tiles[x - 1, y] == null || !LevelGeneration.instance.Tiles[x - 1, y].hasDecorations))
        {
            left = true;
        }
        if (up)
        {
            if (Random.value < 0.1f)
            {
                decorationUp[1].SetActive(true);
            }
            else
            {
                decorationUp[0].SetActive(true);
            }
            spriteRenderer.sprite = spriteUp[0]
;       }
        if (down)
        {
            decorationDown[0].SetActive(true);
            spriteRenderer.sprite = spriteDown[0];
        }
        if(up && down)
        {
            spriteRenderer.sprite = spriteUpDown[0];
        }
        if (left)
        {
            if (Random.value < 0.5f)
            {
                decorationLeft[1].SetActive(true);
            }
            else
            {
                decorationLeft[0].SetActive(true);
            }
        }
        if (right)
        {
            decorationRight[0].SetActive(true);
        }
        if (!up && !down)
        {
            if (Random.value < 0.1f)
            {
                spriteRenderer.sprite = alter[0];
            }
        }
    }
    public void Remove()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GroundBounds : MonoBehaviour
{
    public SpriteRenderer boundsStraight;
    public SpriteRenderer boundsCorner;
    private Transform boundsParent;
    private void Awake()
    {
        boundsParent = GameObject.Find("Bounds").GetComponent<Transform>();
    }
    public void CreateBounds(int width,int height)
    {
        //Tao straights
        // Straights.
        // Straights.
        SpriteRenderer boundsTop = Instantiate(boundsStraight, new Vector3(0, height + 48, 0), Quaternion.identity, boundsParent);
        boundsTop.size = new Vector2(width, 64);
        boundsTop.GetComponent<BoxCollider2D>().size = new Vector2(width, 48);
        boundsTop.GetComponent<BoxCollider2D>().offset = new Vector2(width / 2f, 24);
        boundsTop.transform.localScale = new Vector3(1, -1, 1);
        SpriteRenderer boundsRight = Instantiate(boundsStraight, new Vector3(width + 48, 0, 0), Quaternion.identity, boundsParent);
        boundsRight.size = new Vector2(height, 64);
        boundsRight.GetComponent<BoxCollider2D>().size = new Vector2(height, 48);
        boundsRight.GetComponent<BoxCollider2D>().offset = new Vector2(height / 2f, 24);
        boundsRight.transform.localRotation = Quaternion.Euler(0, 0, 90);
        SpriteRenderer boundsBottom = Instantiate(boundsStraight, new Vector3(0, -48, 0), Quaternion.identity, boundsParent);
        boundsBottom.size = new Vector2(width, 64);
        boundsBottom.GetComponent<BoxCollider2D>().size = new Vector2(width, 48);
        boundsBottom.GetComponent<BoxCollider2D>().offset = new Vector2(width / 2f, 24);
        SpriteRenderer boundsLeft = Instantiate(boundsStraight, new Vector3(-48, height, 0), Quaternion.identity, boundsParent);
        boundsLeft.size = new Vector2(height, 64);
        boundsLeft.GetComponent<BoxCollider2D>().size = new Vector2(height, 48);
        boundsLeft.GetComponent<BoxCollider2D>().offset = new Vector2(height / 2f, 24);
        boundsLeft.transform.localRotation = Quaternion.Euler(0, 0, -90);

        // Corners.
        SpriteRenderer boundsCornerTopLeft = Instantiate(boundsCorner, new Vector3(0, height + 48, 0), Quaternion.identity, boundsParent);
        boundsCornerTopLeft.transform.localRotation = Quaternion.Euler(0, 0, 180);
        SpriteRenderer boundsCornerTopRight = Instantiate(boundsCorner, new Vector3(width + 48, height, 0), Quaternion.identity, boundsParent);
        boundsCornerTopRight.transform.localRotation = Quaternion.Euler(0, 0, 90);
        SpriteRenderer boundsCornerBottomRight = Instantiate(boundsCorner, new Vector3(width, -48, 0), Quaternion.identity, boundsParent);
        boundsCornerBottomRight.transform.localRotation = Quaternion.Euler(0, 0, 0);
        SpriteRenderer boundsCornerBottomLeft = Instantiate(boundsCorner, new Vector3(-48, 0, 0), Quaternion.identity, boundsParent);
        boundsCornerBottomLeft.transform.localRotation = Quaternion.Euler(0, 0, -90);
    }
}

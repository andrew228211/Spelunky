using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Rope : MonoBehaviour
{
    public float maxRopeLength;
    public float ropeSpeed;

    public LayerMask layerMask;

    public SpriteRenderer ropeTop;
    public SpriteRenderer ropeMid;
    public SpriteRenderer ropeEnd;

    public Vector3 placePos;

    // day thung dat sau thi hien truoc
    private static int sortOrder;

    private void Awake()
    {
        ropeMid.gameObject.SetActive(false);
    }
    private void Start()
    {
        if (placePos != Vector3.zero)
        {
            PlaceRope(placePos);
        }
        sortOrder++;
        ropeTop.sortingOrder = sortOrder + 1;
        ropeMid.sortingOrder = sortOrder;
        ropeEnd.sortingOrder = sortOrder + 1;
        //Kiem tra va cham tren duong den vi tri moi hay khong va gan day vao vi tri hien tai neu no va cham phai.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * maxRopeLength, maxRopeLength, layerMask);
        //print(hit.distance);
        if (hit.collider != null && hit.transform.CompareTag("LadderTop") == false)
        {
            //print(transform.position);
            transform.position = new Vector3(transform.position.x, transform.position.y + hit.distance - 8, transform.position.z);
            //print(transform.position);
            PlaceRope(transform.position);
        }
        if (hit.collider == null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + maxRopeLength - 8, transform.position.z);
            PlaceRope(transform.position);
        }

    }
    //private void Update()
    //{   
    //    //Kiem tra va cham tren duong den vi tri moi hay khong va gan day vao vi tri hien tai neu no va cham phai.
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up*maxRopeLength, maxRopeLength, layerMask);
    //    //print(hit.distance);
    //    print(hit.collider);
    //    if (hit.collider != null && hit.transform.CompareTag("LadderTop") == false)
    //    {
    //        print(transform.position);
    //        transform.position = new Vector3(transform.position.x, transform.position.y + hit.distance-8, transform.position.z);
    //        print(transform.position);
    //        PlaceRope(transform.position);
    //    }
    //    if (hit.collider == null)
    //    {
    //        transform.position = new Vector3(transform.position.x, transform.position.y + maxRopeLength- 8, transform.position.z);
    //        PlaceRope(transform.position);
    //    }

    //}
    private void PlaceRope(Vector3 pos)
    {
        StartCoroutine(ExtendRope());
    }
    IEnumerator ExtendRope()
    {
        
        ropeTop.gameObject.SetActive(true);
        ropeMid.gameObject.SetActive(true);
        float ropeLength = maxRopeLength;
        //Tao raycast chieu tia xuong duoi kiem tra va cham gi hay khonng Neu no du do dai >=ropeLength thi dung va thiet lap lai size cua ropeeMid
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, maxRopeLength, layerMask);
       
        if(hit.collider!=null && hit.transform.CompareTag("LadderTop") == false)
        {
            //chieu dai day la diem goc cua no den diem va cham
            ropeLength = hit.distance;
        }
        while (ropeMid.size.y <= ropeLength)
        {
            ropeMid.size += new Vector2(0, ropeSpeed * Time.deltaTime * 0.5f);
            ropeEnd.transform.position = new Vector3(transform.position.x, transform.position.y - ropeMid.size.y, 0);
            yield return null;
        }
        ropeMid.size = new Vector2(ropeMid.size.x, Mathf.FloorToInt(ropeMid.size.y/16)*16+8);
        ropeMid.GetComponent<BoxCollider2D>().size = new Vector2(ropeMid.GetComponent<BoxCollider2D>().size.x, ropeMid.size.y);
        ropeMid.GetComponent<BoxCollider2D>().offset = new Vector2(ropeMid.GetComponent<BoxCollider2D>().offset.x, -1 * ropeMid.size.y / 2f);
        ropeEnd.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius;
    public LayerMask layerMask;
    private SpriteAnimator spriteAnimator;
    private void Awake()
    {
        spriteAnimator = GetComponentInChildren<SpriteAnimator>();
    }
    public void Start()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);
        List<Tile> tilesToRemove = new List<Tile>();
        foreach (Collider2D collider in collider2Ds)
        {
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null)
            {
                //print(tile);
                tilesToRemove.Add(tile);
            }
            Bomb bomb = collider.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.Explode();
            }
        }
        LevelGeneration.instance.RemoveTiles(tilesToRemove.ToArray());
        spriteAnimator.Play("Explosion");
        Destroy(gameObject, 1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridDestructive : MonoBehaviour
{
    public Tilemap tilemapCurrent;

    private void Start()
    {
        tilemapCurrent = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer") || collision.gameObject.CompareTag("BulletEnemy"))
        {
            Debug.Log("tocou na grid destrutiva");
            Destroy(collision.gameObject);
            Vector3 hiPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hiPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hiPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemapCurrent.SetTile(tilemapCurrent.WorldToCell(hiPosition), null);
            }

        }
    }
}

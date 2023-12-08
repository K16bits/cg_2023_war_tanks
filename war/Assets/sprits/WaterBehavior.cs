using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterBehavior : MonoBehaviour
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
        }
    }
}

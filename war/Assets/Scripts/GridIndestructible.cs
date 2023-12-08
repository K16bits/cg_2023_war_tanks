using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridIndestructible : MonoBehaviour
{
    public Tilemap tilemapCurrent;
    private AudioSource soundColision;


    private void Awake()
    {
        soundColision = GetComponent<AudioSource>();
    }
    private void Start()
    {
        tilemapCurrent = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            soundColision.Play();
        }

        if (collision.gameObject.CompareTag("BulletPlayer") || collision.gameObject.CompareTag("BulletEnemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}

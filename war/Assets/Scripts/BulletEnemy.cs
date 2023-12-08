using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            Debug.Log("Toque de tiros");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

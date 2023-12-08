using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 8f;
    private bool canShoot = true; // Variável para verificar se é possível atirar


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Criar uma instância do projetil
        GameObject projectile = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

        // Definir que não é possível atirar enquanto houver um projetil ativo
        canShoot = false;

        // Determinar a direção padrão (cima, baixo, direita, esquerda)
        Vector2 shootDirection = Vector2.left; // Você pode alterar para outras direções conforme necessário

        // Adicionar força ao projetil para movê-lo
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;

        // Chamar a função que permite atirar novamente após o tempo de vida do projetil
        StartCoroutine(EnableShootingAfterDelay(2f, projectile));
    }


    IEnumerator EnableShootingAfterDelay(float delay, GameObject projectile)
    {
        // Esperar pelo tempo especificado
        yield return new WaitForSeconds(delay);

        // Destruir o projetil
        Destroy(projectile);

        // Permitir atirar novamente
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Vector2 position = GetComponent<Rigidbody>().position;
        // Vector2 translation = position * bulletSpeed * Time.fixedDeltaTime;
        // GetComponent<Rigidbody>().MovePosition(position + translation);
    }
}

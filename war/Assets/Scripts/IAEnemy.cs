using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IAEnemy : MonoBehaviour
{
    private AudioSource soundDeadEnemy;
    public float speed = 5f;
    public LayerMask obstacleLayer; // Defina a layer dos obstáculos no Unity e atribua a essa variável.

    public Transform spawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private float offSetbullet = 0.7f;

    private Vector2 shootDirection = Vector2.up;
    private bool canShoot = true; // Variável para verificar se é possível atirar

    public new Rigidbody2D rigidbody { get; private set; }

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private Vector2 direction = Vector2.down;

    private float linearMoveTimer = 2f; // Tempo de movimento linear
    private float currentLinearMoveTime;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererUp;
        soundDeadEnemy = GetComponent<AudioSource>();

    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SetRandomDirection();

    }

    void Update()
    {
        // Verificar se o inimigo precisa mudar de direção
        CheckDirectionChange();
    }

    void FixedUpdate()
    {
        MoveEnemy();
    }

    void CheckDirectionChange()
    {
        currentLinearMoveTime += Time.deltaTime;

        if (currentLinearMoveTime >= linearMoveTimer)
        {
            SetRandomDirection();
            currentLinearMoveTime = 0f;
        }
    }

    void SetRandomDirection()
    {
        // Gere uma direção aleatória restrita a direções ortogonais
        int randomDirection = Random.Range(0, 4); // 0: cima, 1: baixo, 2: esquerda, 3: direita

        switch (randomDirection)
        {
            case 0:
                direction = Vector2.up;
                SetDirection(direction, spriteRendererUp);
                break;
            case 1:
                direction = Vector2.down;
                SetDirection(direction, spriteRendererDown);
                break;
            case 2:
                direction = Vector2.left;
                SetDirection(direction, spriteRendererLeft);
                break;
            case 3:
                direction = Vector2.right;
                SetDirection(direction, spriteRendererRight);
                break;
        }
    }

    void MoveEnemy()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        // Use Raycast para verificar se há obstáculos antes de mover o inimigo
        RaycastHit2D hit = Physics2D.Raycast(position, direction, speed * Time.fixedDeltaTime, obstacleLayer);

        if (hit.collider == null)
        {
            // Mova o inimigo se não houver obstáculos
            rigidbody.MovePosition(position + translation);

        }
        else
        {
            // Caso contrário, mude a direção aleatoriamente
            SetRandomDirection();
        }
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        shootDirection = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
        if (canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector2 position = rigidbody.position;

        // Use Raycast para verificar se há obstáculos antes de mover o inimigo
        RaycastHit2D hit = Physics2D.Raycast(position, direction, speed * Time.fixedDeltaTime, obstacleLayer);

        // Verificar se NÃO há colisão (nenhum obstáculo à frente)
        if (!hit.collider)
        {
            Vector2 posicaoPersonagem = transform.position;
            Vector2 posicaoInicialTiro = posicaoPersonagem + offSetbullet * shootDirection;

            // Cria o tiro na posição inicial calculada
            GameObject projectile = Instantiate(bulletPrefab, posicaoInicialTiro, Quaternion.identity);

            // Definir que não é possível atirar enquanto houver um projetil ativo
            canShoot = false;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = shootDirection * bulletSpeed;

            // Chamar a função que permite atirar novamente após o tempo de vida do projetil
            StartCoroutine(EnableShootingAfterDelay(2f, projectile));
        }
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
        if (ScoreScript.scoreValue == 30)
        {
            SceneManager.LoadScene("Fase2");
            ScoreScript.scoreValue += 10;
        }

        if (ScoreScript.scoreValue == 60)
        {
            ScoreScript.scoreValue += 10;
            SceneManager.LoadScene("Fase3");
        }

        if (ScoreScript.scoreValue == 130)
        {
            Invoke(nameof(WinScene), 1.25f);
        }

        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            Destroy(collision.gameObject);
            soundDeadEnemy.Play();
            ScoreScript.scoreValue += 10;

            DeathSequence();
        }
        else if (collision.gameObject.CompareTag("BulletEnemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {

        gameObject.SetActive(false);
    }

    private void WinScene()
    {
    }

    private void Home()
    {
        SceneManager.LoadScene("EndGame");
    }

}

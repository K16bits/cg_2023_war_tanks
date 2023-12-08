using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementPlayer : MonoBehaviour
{

    private AudioSource soundShoot;
    public GameObject gameOverUI;

    public GameObject gameWinUI;

    public Transform spawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 8f;
    private float offSetbullet = 0.7f;
    private bool canShoot = true; // Variável para verificar se é possível atirar
    private Vector2 shootDirection = Vector2.down; // Você pode alterar para outras direções conforme necessário

    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;


    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputShoot = KeyCode.Space;

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
        soundShoot = GetComponent<AudioSource>();
    }

    private void Update()
    {


        if (ScoreScript.scoreValue >= 100)
        {
            WinScene();
        }

        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
            shootDirection = Vector2.up;
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
            shootDirection = Vector2.down;
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
            shootDirection = Vector2.left;
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
            shootDirection = Vector2.right;
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }


        if (Input.GetKey(inputShoot) && canShoot)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("BulletEnemy"))
        {
            Destroy(collision.gameObject);
            DeathSequence();
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
        gameOverUI.SetActive(true);

        // FindObjectOfType<GameManager>().CheckWinState();
        Invoke(nameof(LoadGameMenu), 3.0f);
    }

    private void WinScene()
    {
        enabled = false;
        gameObject.SetActive(false);
        gameWinUI.SetActive(true);

        // FindObjectOfType<GameManager>().CheckWinState();
        Invoke(nameof(LoadGameMenu), 3.0f);
    }
    private void LoadGameMenu()
    {
        SceneManager.LoadScene("menu");
    }

    void Shoot()
    {

        // Obtém a posição atual do personagem
        Vector2 posicaoPersonagem = transform.position;

        Vector2 posicaoInicialTiro = posicaoPersonagem + offSetbullet * shootDirection;

        // Cria o tiro na posição inicial calculada
        soundShoot.Play();
        GameObject projectile = Instantiate(bulletPrefab, posicaoInicialTiro, Quaternion.identity);

        // Definir que não é possível atirar enquanto houver um projetil ativo
        canShoot = false;


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

}

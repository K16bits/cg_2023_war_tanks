using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public float speed = 120.0f;  // Velocidade do movimento
    public GameObject mainMenu;


    void Start()
    {
        // Obtém a referência para o componente TextMeshProUGUI
        ScoreScript.scoreValue = 0;
        mainMenu = GetComponent<GameObject>();
    }

    void Update()
    {
        // Move o objeto na direção positiva do eixo Y (de baixo para cima)
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Se o texto atingir a posição desejada, pare o movimento
        if (transform.position.y >= Screen.height / 2)
        {
            speed = 0;  // Ou qualquer outra lógica que você desejar
        }
    }

    public void PlayerGame()
    {
        SceneManager.LoadScene("Fase1");
    }

    public void Player2Game()
    {
        SceneManager.LoadScene("Game2Player");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using TMPro;

public class GameOverText : MonoBehaviour
{
    public float speed = 25.0f;  // Velocidade do movimento
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        // Obtém a referência para o componente TextMeshProUGUI
        textMeshPro = GetComponent<TextMeshProUGUI>();
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
}

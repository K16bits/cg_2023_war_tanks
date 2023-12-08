using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    public GameObject objetoPrefab; // Referência ao prefab do objeto que será spawnado
    private int objetosSpawnados = 0;
    private int limiteObjetos = 4;

    void Start()
    {
        SpawnObjetosIniciais();
    }

    void Update()
    {
        if (objetosSpawnados < 3)
        {
            SpawnObjeto();
        }
    }

    void SpawnObjetosIniciais()
    {
        for (int i = 0; i < limiteObjetos; i++)
        {
            SpawnObjeto();
            objetosSpawnados++;
        }
    }

    void SpawnObjeto()
    {
        if (objetosSpawnados < limiteObjetos)
        {
            // Lógica para spawnar o objeto
            Instantiate(objetoPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Lógica para obter uma posição de spawn aleatória
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Pole pre prefaby terénov prvej úrovne.
    public GameObject[] terrainPrefabs1;

    // Pole pre prefaby terénov druhej úrovne.
    public GameObject[] terrainPrefabs2;

    // Pole pre prefaby terénov tretej úrovne.
    public GameObject[] terrainPrefabs3;

    // Pole pre prefaby terénov štvrtej úrovne.
    public GameObject[] terrainPrefabs4;

    // Premenná pre aktuálny vybraný prefab terénu.
    private GameObject terrainPrefab;

    // Transform hráèa pre sledovanie jeho pozície.
    public Transform player;

    // Transform koncovej pozície, kde sa generuje nový terén.
    public Transform endPosition;

    // Výška, na ktorej sa generuje nový terén.
    public float terrainHeight = 5f;

    // Odkaz na posledný vygenerovaný terén.
    private GameObject lastTerrain;

    // Zoznam všetkých aktuálne vygenerovaných terénov.
    private List<GameObject> activeTerrains = new List<GameObject>();

    // Vzdialenos, po ktorej sa terény za hráèom nièia.
    public float destroyDistance = 20f;

    private void Update()
    {
        // Kontrola, èi je hráè blízko konca aktuálneho terénu.
        if (player.position.y > endPosition.position.y - terrainHeight / 2)
        {
            // Ak áno, generujeme nový terén.
            GenerateNewTerrain();
        }

        // Kontrola a nièenie terénov za hráèom.
        DestroyOldTerrains();
    }

    void GenerateNewTerrain()
    {
        // Premenná na výber náhodného terénu.
        int randomTerrain;

        // Ak je hráè pod výškou 300.
        if (player.transform.position.y < 300)
        {
            randomTerrain = Random.Range(0, terrainPrefabs1.Length);
            terrainPrefab = terrainPrefabs1[randomTerrain];
        }
        // Ak je hráè medzi výškou 300 a 600.
        else if (player.transform.position.y > 300 && player.transform.position.y < 600)
        {
            randomTerrain = Random.Range(0, terrainPrefabs2.Length);
            terrainPrefab = terrainPrefabs2[randomTerrain];
        }
        // Ak je hráè medzi výškou 600 a 900.
        else if (player.transform.position.y > 600 && player.transform.position.y < 900)
        {
            randomTerrain = Random.Range(0, terrainPrefabs3.Length);
            terrainPrefab = terrainPrefabs3[randomTerrain];
        }
        // Ak je hráè nad výškou 600 a viac.
        else
        {
            randomTerrain = Random.Range(0, terrainPrefabs4.Length);
            terrainPrefab = terrainPrefabs4[randomTerrain];
        }

        // Vytvárame nový terén na pozícii "endPosition".
        GameObject terrain = Instantiate(terrainPrefab, endPosition.position, Quaternion.identity);

        // Pridávame nový terén do zoznamu aktívnych terénov.
        activeTerrains.Add(terrain);

        // Ukladáme referenciu na práve vygenerovaný terén.
        lastTerrain = terrain;

        // Aktualizujeme pozíciu "endPosition" na pozíciu urèenú v novom teréne.
        endPosition = lastTerrain.transform.GetChild(0).transform;
    }

    void DestroyOldTerrains()
    {
        // Prechádzame cez všetky vygenerované terény.
        for (int i = activeTerrains.Count - 1; i >= 0; i--)
        {
            // Kontrola vzdialenosti medzi hráèom a terénom.
            if (player.position.y - activeTerrains[i].transform.position.y > destroyDistance)
            {
                // Ak je terén príliš ïaleko za hráèom, znièíme ho.
                Destroy(activeTerrains[i]);
                activeTerrains.RemoveAt(i);
            }
        }
    }
}

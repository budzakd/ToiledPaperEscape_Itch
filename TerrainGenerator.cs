using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Pole pre prefaby ter�nov prvej �rovne.
    public GameObject[] terrainPrefabs1;

    // Pole pre prefaby ter�nov druhej �rovne.
    public GameObject[] terrainPrefabs2;

    // Pole pre prefaby ter�nov tretej �rovne.
    public GameObject[] terrainPrefabs3;

    // Pole pre prefaby ter�nov �tvrtej �rovne.
    public GameObject[] terrainPrefabs4;

    // Premenn� pre aktu�lny vybran� prefab ter�nu.
    private GameObject terrainPrefab;

    // Transform hr��a pre sledovanie jeho poz�cie.
    public Transform player;

    // Transform koncovej poz�cie, kde sa generuje nov� ter�n.
    public Transform endPosition;

    // V��ka, na ktorej sa generuje nov� ter�n.
    public float terrainHeight = 5f;

    // Odkaz na posledn� vygenerovan� ter�n.
    private GameObject lastTerrain;

    // Zoznam v�etk�ch aktu�lne vygenerovan�ch ter�nov.
    private List<GameObject> activeTerrains = new List<GameObject>();

    // Vzdialenos�, po ktorej sa ter�ny za hr��om ni�ia.
    public float destroyDistance = 20f;

    private void Update()
    {
        // Kontrola, �i je hr�� bl�zko konca aktu�lneho ter�nu.
        if (player.position.y > endPosition.position.y - terrainHeight / 2)
        {
            // Ak �no, generujeme nov� ter�n.
            GenerateNewTerrain();
        }

        // Kontrola a ni�enie ter�nov za hr��om.
        DestroyOldTerrains();
    }

    void GenerateNewTerrain()
    {
        // Premenn� na v�ber n�hodn�ho ter�nu.
        int randomTerrain;

        // Ak je hr�� pod v��kou 300.
        if (player.transform.position.y < 300)
        {
            randomTerrain = Random.Range(0, terrainPrefabs1.Length);
            terrainPrefab = terrainPrefabs1[randomTerrain];
        }
        // Ak je hr�� medzi v��kou 300 a 600.
        else if (player.transform.position.y > 300 && player.transform.position.y < 600)
        {
            randomTerrain = Random.Range(0, terrainPrefabs2.Length);
            terrainPrefab = terrainPrefabs2[randomTerrain];
        }
        // Ak je hr�� medzi v��kou 600 a 900.
        else if (player.transform.position.y > 600 && player.transform.position.y < 900)
        {
            randomTerrain = Random.Range(0, terrainPrefabs3.Length);
            terrainPrefab = terrainPrefabs3[randomTerrain];
        }
        // Ak je hr�� nad v��kou 600 a viac.
        else
        {
            randomTerrain = Random.Range(0, terrainPrefabs4.Length);
            terrainPrefab = terrainPrefabs4[randomTerrain];
        }

        // Vytv�rame nov� ter�n na poz�cii "endPosition".
        GameObject terrain = Instantiate(terrainPrefab, endPosition.position, Quaternion.identity);

        // Prid�vame nov� ter�n do zoznamu akt�vnych ter�nov.
        activeTerrains.Add(terrain);

        // Uklad�me referenciu na pr�ve vygenerovan� ter�n.
        lastTerrain = terrain;

        // Aktualizujeme poz�ciu "endPosition" na poz�ciu ur�en� v novom ter�ne.
        endPosition = lastTerrain.transform.GetChild(0).transform;
    }

    void DestroyOldTerrains()
    {
        // Prech�dzame cez v�etky vygenerovan� ter�ny.
        for (int i = activeTerrains.Count - 1; i >= 0; i--)
        {
            // Kontrola vzdialenosti medzi hr��om a ter�nom.
            if (player.position.y - activeTerrains[i].transform.position.y > destroyDistance)
            {
                // Ak je ter�n pr�li� �aleko za hr��om, zni��me ho.
                Destroy(activeTerrains[i]);
                activeTerrains.RemoveAt(i);
            }
        }
    }
}

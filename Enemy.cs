using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Hr��
    private Transform player;
    // Komponent zvuku
    [SerializeField] private AudioSource audioSource;
    // Maxim�lna vzdialenos� pre po�utie
    public float maxDistance = 5f; 

    // Premenn� pre objekt, ktor� predstavuje "spray efekt" napriklad v hre.
    public GameObject hitObject;

    // Premenn� na kontrolu, �i sa hit objekt pr�ve pou��va, aby sa zabr�nilo viacer�m spusteniam.
    bool hit;

    private void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
    }

    // Funkcia Update je volan� ka�d� sn�mok (frame).
    void Update()
    {
        // Ak "hit object" nie je akt�vny, aktivuje sa.
        if (!hit)
        {
            // Ozna��me, �e sa teraz pou��va.
            hit = true;

            // Spust�me funkciu EnableHitObject().
            StartCoroutine(EnableHitObject());
        }

        // Vypo��ta vzdialenos� medzi hr��om a objektom
        float distance = Vector2.Distance(player.position, transform.position);

        // Nastav hlasitos� na z�klade vzdialenosti
        if (distance < maxDistance)
        {
            audioSource.volume = 1; // Tlmenie hlasitosti
        }
        else
        {
            audioSource.volume = 0; // Zvuk vypnut�
        }
    }

    // Funkcia, ktor� umo��uje �asov� oneskorenia medzi jednotliv�mi krokmi.
    IEnumerator EnableHitObject()
    {
        // Po�k�me 2 sekundy pred aktivovan�m hit objectu.
        float randomTimeToEnable = Random.Range(2f, 4f);
        yield return new WaitForSeconds(randomTimeToEnable);

        // Aktivujeme hit object (zvidite�n�me objekt).
        hitObject.SetActive(true);

        // Po�k�me 1 sekundu, k�m je hit object akt�vny.
        yield return new WaitForSeconds(1f);

        // Deaktivujeme hit object(skryjeme objekt).
        hitObject.SetActive(false);

        // Ozna��me, �e hit object je pripraven� na �al�ie pou�itie.
        hit = false;
    }
}

using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Hráè
    private Transform player;
    // Komponent zvuku
    [SerializeField] private AudioSource audioSource;
    // Maximálna vzdialenos pre poèutie
    public float maxDistance = 5f; 

    // Premenná pre objekt, ktorı predstavuje "spray efekt" napriklad v hre.
    public GameObject hitObject;

    // Premenná na kontrolu, èi sa hit objekt práve pouíva, aby sa zabránilo viacerım spusteniam.
    bool hit;

    private void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
    }

    // Funkcia Update je volaná kadı snímok (frame).
    void Update()
    {
        // Ak "hit object" nie je aktívny, aktivuje sa.
        if (!hit)
        {
            // Oznaèíme, e sa teraz pouíva.
            hit = true;

            // Spustíme funkciu EnableHitObject().
            StartCoroutine(EnableHitObject());
        }

        // Vypoèíta vzdialenos medzi hráèom a objektom
        float distance = Vector2.Distance(player.position, transform.position);

        // Nastav hlasitos na základe vzdialenosti
        if (distance < maxDistance)
        {
            audioSource.volume = 1; // Tlmenie hlasitosti
        }
        else
        {
            audioSource.volume = 0; // Zvuk vypnutı
        }
    }

    // Funkcia, ktorá umoòuje èasové oneskorenia medzi jednotlivımi krokmi.
    IEnumerator EnableHitObject()
    {
        // Poèkáme 2 sekundy pred aktivovaním hit objectu.
        float randomTimeToEnable = Random.Range(2f, 4f);
        yield return new WaitForSeconds(randomTimeToEnable);

        // Aktivujeme hit object (zvidite¾níme objekt).
        hitObject.SetActive(true);

        // Poèkáme 1 sekundu, kım je hit object aktívny.
        yield return new WaitForSeconds(1f);

        // Deaktivujeme hit object(skryjeme objekt).
        hitObject.SetActive(false);

        // Oznaèíme, e hit object je pripravenı na ïalšie pouitie.
        hit = false;
    }
}

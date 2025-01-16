using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    // Premenná pre objekt "player" (hráè), ku ktorému sa bude pozícia ruky prispôsobova.
    public GameObject player;

    // Èas kım sa zaène ruka pohybova
    public float timeToStartChasing = 3;
    private float timer = 0f;

    private void Update()
    {
        if (timer < timeToStartChasing)
        {
            timer += 1 * Time.deltaTime;
        }
        else
        {
            // Aktualizovanie pozície ruky na osi X pod¾a hráèa, prièom os Y zostáva nezmenená.
            transform.position = new Vector3(player.transform.position.x, transform.position.y);

            // Ak je pozícia ruky na osi Y menšia ako 200.
            if (transform.position.y < 200)
            {
                // Posuò ruku nahor rıchlosou 1.2 za sekundu.
                transform.position += new Vector3(0, 1.2f) * Time.deltaTime;
            }

            // Ak je pozícia ruky na osi Y medzi 200 a 400.
            if (transform.position.y > 200 && transform.position.y < 400)
            {
                // Posuò ruku nahor rıchlosou 1.3 za sekundu.
                transform.position += new Vector3(0, 1.3f) * Time.deltaTime;
            }

            // Ak je pozícia ruky na osi Y väèšia ako 400.
            if (transform.position.y > 400)
            {
                // Posuò ruku nahor rıchlosou 1.4 za sekundu.
                transform.position += new Vector3(0, 1.4f) * Time.deltaTime;
            }
        }

    }
}

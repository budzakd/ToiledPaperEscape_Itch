using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    // Premenn� pre objekt "player" (hr��), ku ktor�mu sa bude poz�cia ruky prisp�sobova�.
    public GameObject player;

    // �as k�m sa za�ne ruka pohybova�
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
            // Aktualizovanie poz�cie ruky na osi X pod�a hr��a, pri�om os Y zost�va nezmenen�.
            transform.position = new Vector3(player.transform.position.x, transform.position.y);

            // Ak je poz�cia ruky na osi Y men�ia ako 200.
            if (transform.position.y < 200)
            {
                // Posu� ruku nahor r�chlos�ou 1.2 za sekundu.
                transform.position += new Vector3(0, 1.2f) * Time.deltaTime;
            }

            // Ak je poz�cia ruky na osi Y medzi 200 a 400.
            if (transform.position.y > 200 && transform.position.y < 400)
            {
                // Posu� ruku nahor r�chlos�ou 1.3 za sekundu.
                transform.position += new Vector3(0, 1.3f) * Time.deltaTime;
            }

            // Ak je poz�cia ruky na osi Y v��ia ako 400.
            if (transform.position.y > 400)
            {
                // Posu� ruku nahor r�chlos�ou 1.4 za sekundu.
                transform.position += new Vector3(0, 1.4f) * Time.deltaTime;
            }
        }

    }
}

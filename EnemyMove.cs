using UnityEngine;
using UnityEngine.Audio;

public class EnemyMove : MonoBehaviour
{
    // Body, medzi ktor�mi sa objekt pohybuje
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private AudioSource audioSource;

    // R�chlos� pohybu
    [SerializeField] private float speed = 2f;

    // Aktu�lny cie�ov� bod
    private Transform targetPoint;

    private void Start()
    {
        targetPoint = pointB;
    }

    private void Update()
    {
        // Pohyb smerom k cie�ov�mu bodu
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Ak objekt dosiahne cie�ov� bod
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == pointB)
            {
                // Ak je v bode B, teleportuj ho na bod A
                transform.position = pointA.position;
                // Aktivuj komponent audia
                audioSource.enabled = true;
                // Pokra�uj k bodu B
                targetPoint = pointB; 
            }
        }
    }
}

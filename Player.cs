using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    // Rıchlos pohybu hráèa.
    public float moveSpeed = 1f;
    // Sila skoku hráèa.
    public float jumpForce = 2f;
    // Premenná na sledovanie, èi hráè èelí doprava.
    private bool isFacingRight = true;

    [Header("Physics")]
    // Rigidbody2D komponent na fyzikálne správanie hráèa.
    private Rigidbody2D rb;
    // Premenná na kontrolu, èi hráè stojí na zemi.
    public bool isGrounded;
    // Premenná na kontrolu, èi hráè naráa do objektov nad sebou.
    private bool isTouchingBelow;

    [Header("References")]
    // Animator pre spúšanie animácií.
    private Animator animator;
    // Objekt pre limitovanie kamery.
    public GameObject cameraLimits;
    // Zvukovı zdroj pre zvuk skoku.
    public AudioSource audioJumpSource;
    // Pole zvukovıch klipov na skok.
    public AudioClip[] audioJumpClips;
    // Pole zvukovıch klipov na doskok na zem AK budes chciet inak zakomentuj.
    public AudioClip[] audioLandClips;
    private bool landSound;

    [Header("UI")]
    // Obrazovka "Game Over".
    public GameObject gameOverScreen;
    // UI text pre zobrazenie skóre.
    public TextMeshProUGUI pointsCounterText;
    // Aktuálne skóre hráèa.
    private int pointsCounter;

    [Header("Score")]
    // Základná vıška hráèa, od ktorej sa poèíta skóre.
    private float baseHeight;

    void Start()
    {
        // Inicializácia Rigidbody2D komponentu.
        rb = GetComponent<Rigidbody2D>();
        // Inicializácia Animator komponentu.
        animator = GetComponent<Animator>();
        // Nastavenie skóre na nulu.
        pointsCounter = 0;
        // Nastavenie základnej vıšky hráèa.
        baseHeight = transform.position.y;
        // Aktualizácia skóre v UI na zaèiatku.
        UpdateScoreUI();
    }

    void Update()
    {
        // Naèítanie vstupu na osi Horizontal (¾avı/pravı pohyb).
        float moveInput = Input.GetAxis("Horizontal");
        // Nastavenie rıchlosti pohybu hráèa.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Kontrola, èi je hráè na zemi a stlaèil tlaèidlo skoku.
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Hráè u nie je na zemi.
            isGrounded = false;
            // Nastavenie rıchlosti pre skok.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // Spustenie animácie skoku.
            animator.Play("Jump");
            // Prehrávanie náhodného zvuku skoku.
            int randomSound = Random.Range(0, audioJumpClips.Length);
            audioJumpSource.PlayOneShot(audioJumpClips[randomSound]);

            // Resetovanie spustenia zvuku doskoku.
            landSound = false; 
        }

        // Otoèenie hráèa, ak mení smer pohybu.
        if (moveInput < 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput > 0 && isFacingRight)
        {
            Flip();
        }

        // Aktualizácia pozície kamery na vıšku hráèa.
        cameraLimits.transform.position = new Vector3(cameraLimits.transform.position.x, transform.position.y);

        // Spustenie animácie pristátia, ak je hráè na zemi a nehıbe sa nahor.
        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            animator.Play("Land");
            // Prehrávanie náhodného zvuku doskoku.
            LandSound();
        }

        // Aktualizácia skóre na základe vıšky hráèa.
        UpdateScore();
    }

    void Flip()
    {
        // Zmena smeru hráèa.
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        // Kontrola, èi hráè pristál na zemi.
        if (rb.linearVelocity.y <= 0)
        {
            // Spustenie animácie pristátia, ak hráè nenaráa do objektov nad sebou.
            if (!isTouchingBelow)
            {
                animator.Play("Land");
                // Prehrávanie náhodného zvuku doskoku.
                LandSound();
            }
        }
    }

    private void LandSound()
    {
        if (!landSound)
        {
            landSound = true;
            int randomSound = Random.Range(0, audioLandClips.Length);
            audioJumpSource.PlayOneShot(audioLandClips[randomSound]);
        }
    }

    public void RestartButton()
    {
        // Reštartovanie hry naèítaním prvej scény.
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Ak hráè opustí povrch, nastaví sa, e u nie je na zemi.
        isGrounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Kontrola, èi hráè naráa do objektov nad sebou.
        isTouchingBelow = rb.linearVelocity.y > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kontrola, èi hráè zasiahne objekt s tagom "Hit".
        if (collision.CompareTag("Hit"))
        {
            print("GAME OVER");
            // Zobrazenie obrazovky Game Over.
            gameOverScreen.SetActive(true);
            // Zastavenie hry.
            Time.timeScale = 0f;
        }
    }

    // Aktualizácia skóre na základe vıšky hráèa.
    private void UpdateScore()
    {
        // Vıpoèet nového skóre pod¾a vıšky.
        int newScore = Mathf.FloorToInt(transform.position.y - baseHeight) * 10;
        // Ak je nové skóre vyššie, aktualizujeme.
        if (newScore > pointsCounter)
        {
            pointsCounter = newScore;
            UpdateScoreUI();
        }
    }

    // Aktualizácia textu skóre v UI.
    private void UpdateScoreUI()
    {
        pointsCounterText.text = pointsCounter.ToString();
    }

}


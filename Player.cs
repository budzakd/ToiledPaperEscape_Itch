using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    // R�chlos� pohybu hr��a.
    public float moveSpeed = 1f;
    // Sila skoku hr��a.
    public float jumpForce = 2f;
    // Premenn� na sledovanie, �i hr�� �el� doprava.
    private bool isFacingRight = true;

    [Header("Physics")]
    // Rigidbody2D komponent na fyzik�lne spr�vanie hr��a.
    private Rigidbody2D rb;
    // Premenn� na kontrolu, �i hr�� stoj� na zemi.
    public bool isGrounded;
    // Premenn� na kontrolu, �i hr�� nar�a do objektov nad sebou.
    private bool isTouchingBelow;

    [Header("References")]
    // Animator pre sp���anie anim�ci�.
    private Animator animator;
    // Objekt pre limitovanie kamery.
    public GameObject cameraLimits;
    // Zvukov� zdroj pre zvuk skoku.
    public AudioSource audioJumpSource;
    // Pole zvukov�ch klipov na skok.
    public AudioClip[] audioJumpClips;
    // Pole zvukov�ch klipov na doskok na zem AK budes chciet inak zakomentuj.
    public AudioClip[] audioLandClips;
    private bool landSound;

    [Header("UI")]
    // Obrazovka "Game Over".
    public GameObject gameOverScreen;
    // UI text pre zobrazenie sk�re.
    public TextMeshProUGUI pointsCounterText;
    // Aktu�lne sk�re hr��a.
    private int pointsCounter;

    [Header("Score")]
    // Z�kladn� v��ka hr��a, od ktorej sa po��ta sk�re.
    private float baseHeight;

    void Start()
    {
        // Inicializ�cia Rigidbody2D komponentu.
        rb = GetComponent<Rigidbody2D>();
        // Inicializ�cia Animator komponentu.
        animator = GetComponent<Animator>();
        // Nastavenie sk�re na nulu.
        pointsCounter = 0;
        // Nastavenie z�kladnej v��ky hr��a.
        baseHeight = transform.position.y;
        // Aktualiz�cia sk�re v UI na za�iatku.
        UpdateScoreUI();
    }

    void Update()
    {
        // Na��tanie vstupu na osi Horizontal (�av�/prav� pohyb).
        float moveInput = Input.GetAxis("Horizontal");
        // Nastavenie r�chlosti pohybu hr��a.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Kontrola, �i je hr�� na zemi a stla�il tla�idlo skoku.
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Hr�� u� nie je na zemi.
            isGrounded = false;
            // Nastavenie r�chlosti pre skok.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // Spustenie anim�cie skoku.
            animator.Play("Jump");
            // Prehr�vanie n�hodn�ho zvuku skoku.
            int randomSound = Random.Range(0, audioJumpClips.Length);
            audioJumpSource.PlayOneShot(audioJumpClips[randomSound]);

            // Resetovanie spustenia zvuku doskoku.
            landSound = false; 
        }

        // Oto�enie hr��a, ak men� smer pohybu.
        if (moveInput < 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput > 0 && isFacingRight)
        {
            Flip();
        }

        // Aktualiz�cia poz�cie kamery na v��ku hr��a.
        cameraLimits.transform.position = new Vector3(cameraLimits.transform.position.x, transform.position.y);

        // Spustenie anim�cie prist�tia, ak je hr�� na zemi a neh�be sa nahor.
        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            animator.Play("Land");
            // Prehr�vanie n�hodn�ho zvuku doskoku.
            LandSound();
        }

        // Aktualiz�cia sk�re na z�klade v��ky hr��a.
        UpdateScore();
    }

    void Flip()
    {
        // Zmena smeru hr��a.
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        // Kontrola, �i hr�� prist�l na zemi.
        if (rb.linearVelocity.y <= 0)
        {
            // Spustenie anim�cie prist�tia, ak hr�� nenar�a do objektov nad sebou.
            if (!isTouchingBelow)
            {
                animator.Play("Land");
                // Prehr�vanie n�hodn�ho zvuku doskoku.
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
        // Re�tartovanie hry na��tan�m prvej sc�ny.
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Ak hr�� opust� povrch, nastav� sa, �e u� nie je na zemi.
        isGrounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Kontrola, �i hr�� nar�a do objektov nad sebou.
        isTouchingBelow = rb.linearVelocity.y > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kontrola, �i hr�� zasiahne objekt s tagom "Hit".
        if (collision.CompareTag("Hit"))
        {
            print("GAME OVER");
            // Zobrazenie obrazovky Game Over.
            gameOverScreen.SetActive(true);
            // Zastavenie hry.
            Time.timeScale = 0f;
        }
    }

    // Aktualiz�cia sk�re na z�klade v��ky hr��a.
    private void UpdateScore()
    {
        // V�po�et nov�ho sk�re pod�a v��ky.
        int newScore = Mathf.FloorToInt(transform.position.y - baseHeight) * 10;
        // Ak je nov� sk�re vy��ie, aktualizujeme.
        if (newScore > pointsCounter)
        {
            pointsCounter = newScore;
            UpdateScoreUI();
        }
    }

    // Aktualiz�cia textu sk�re v UI.
    private void UpdateScoreUI()
    {
        pointsCounterText.text = pointsCounter.ToString();
    }

}


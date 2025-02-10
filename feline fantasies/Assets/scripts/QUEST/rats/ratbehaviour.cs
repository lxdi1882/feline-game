using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class RatBehavior : MonoBehaviour
{
    private PlayerNutCollection playerNut;
    public float detectionRadius = 3f;
    public float runSpeed = 3f;
    public float idleTime = 1.5f;
    public float hopHeight = 3f;
    public float hopSpeed = 0.2f;
    public float fadeDuration = 0.5f;
    public Color hitColor = Color.red;
    public float hitDuration = 0.5f;

    public AudioClip hitSound;
    public AudioClip deathSound;
    public Animator ratAnimator;
    public GameObject shadow; // Assign this in Unity
    public GameObject interactionPromptE;

    private bool isRunning = false;
    private bool isHit = false;
    private bool facingLeft = true;
    private int health = 2;
    private Vector2 runDirection;
    private Vector2 lastMoveDirection;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Transform player;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = spriteRenderer.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the PlayerNutCollection script dynamically
        playerNut = FindObjectOfType<PlayerNutCollection>();

        if (playerNut == null)
        {
            Debug.LogError("PlayerNutCollection script not found in the scene!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectionRadius && !isRunning)
        {
            RunAway();
        }

        Animate();
    }

    void RunAway()
    {
        isRunning = true;
        runDirection = (transform.position - player.position).normalized;
        Invoke(nameof(StopRunning), idleTime);
    }

    void StopRunning()
    {
        isRunning = false;
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            rb.linearVelocity = runDirection * runSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        interactionPromptE.SetActive(true);
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isHit)
        {
            TakeDamage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        interactionPromptE.SetActive(false);
    }

    private void SetActive(bool v)
    {
        throw new NotImplementedException();
    }

    void TakeDamage()
    {
        health--;
        isHit = true;
        spriteRenderer.color = hitColor;
        StartCoroutine(HopEffect(health <= 0));
        PlayHitSound();
        Invoke(nameof(ResetColor), hitDuration);

        if (health <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator HopEffect(bool isDeathJump)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetUp = startPosition + new Vector3(0, hopHeight, 0);
        Vector3 targetDown = startPosition;

        float elapsedTime = 0f;
        while (elapsedTime < hopSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetUp, elapsedTime / hopSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetUp;

        elapsedTime = 0f;
        while (elapsedTime < hopSpeed)
        {
            transform.position = Vector3.Lerp(targetUp, targetDown, elapsedTime / hopSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetDown;

        if (isDeathJump)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    IEnumerator DeathSequence()
    {
        PlayDeathSound();
        ratAnimator.SetTrigger("Die");
        yield return StartCoroutine(HopEffect(true));

        if (playerNut != null)
        {
            playerNut.ratkilled++;
            Debug.Log("Rat killed: " + playerNut.ratkilled);
            playerNut.UpdateRatKilledUI();
        }

        Destroy(gameObject);
    }

    IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    void Animate()
    {
        Vector2 moveInput = rb.linearVelocity;

        if (moveInput.magnitude > 0)
        {
            lastMoveDirection = moveInput.normalized;
        }

        ratAnimator.SetFloat("MoveX", moveInput.x);
        ratAnimator.SetFloat("MoveY", moveInput.y);
        ratAnimator.SetFloat("MoveMagnitude", moveInput.magnitude);
        ratAnimator.SetFloat("LastMoveX", lastMoveDirection.x);
        ratAnimator.SetFloat("LastMoveY", lastMoveDirection.y);

        if ((moveInput.x < 0 && !facingLeft) || (moveInput.x > 0 && facingLeft))
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;

        foreach (Transform child in transform)
        {
            child.localScale = new Vector3(Mathf.Abs(child.localScale.x), child.localScale.y, child.localScale.z);
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    void ResetColor()
    {
        spriteRenderer.color = originalColor;
        isHit = false;
    }
}

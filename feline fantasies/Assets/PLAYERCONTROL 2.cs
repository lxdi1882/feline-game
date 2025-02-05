using UnityEngine;

public class PLAYERCONTROL2 : MonoBehaviour
{
    public float speed = 0.5f;
    public Rigidbody2D rb;
    public Animator playerAnimator;

    private Vector2 input;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;

    void Start()
    {
        // Apply the selected animator from BreedManager when entering the game scene
        if (BreedManager.selectedAnimator != null)
        {
            playerAnimator.runtimeAnimatorController = BreedManager.selectedAnimator;
            Debug.Log("Loaded Animator: " + BreedManager.selectedAnimator.name);
        }
        else
        {
            Debug.LogWarning("No animator selected in BreedManager!");
        }
    }

    void Update()
    {
        ProcessInputs();
        Animate();
        if (input.x < 0 && !facingLeft || input.x > 0 && facingLeft)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * speed; // Use rb.velocity instead of linearVelocity
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }

        input.x = moveX;
        input.y = moveY;
        input.Normalize();
    }

    void Animate()
    {
        playerAnimator.SetFloat("MoveX", input.x);
        playerAnimator.SetFloat("MoveY", input.y);
        playerAnimator.SetFloat("MoveMagnitude", input.magnitude);
        playerAnimator.SetFloat("LastMoveX", lastMoveDirection.x);
        playerAnimator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }
}

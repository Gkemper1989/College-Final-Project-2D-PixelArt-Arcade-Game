using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variables
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip jumpSFX;

    //state
    bool isAlive = true;

    //cache component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    //referencing components
    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent <BoxCollider2D>() ;
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        Jump();
        Climb();
        FlipSprite();
        Die();
    }

    //enabling the player to move left and right
    private void Run()
    {
        float controlThrow = Input.GetAxisRaw("Horizontal"); //value between -runSpeed and +runSpeed
        Vector2 runVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = runVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //set true when the absolute value of the movement is greater than 0 
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("Running", true);
        }
        else
        {
            myAnimator.SetBool("Running", false);
        }
    }

    //enabling the player to jump when is on the ground avoiding multiple jumps and wall jumps
    private void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) //check if the player is on the ground to enable it to jump
        {
            myAnimator.SetBool("Jumping", false);
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
            myAnimator.SetBool("Jumping", true); //trigger the jump animation
            AudioSource.PlayClipAtPoint(jumpSFX, transform.position); //trigger the jump sound effect
        }
    }

    //enabling the player to climb when is on the object which the player can climb
    private void Climb()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) //check if the player is on the ladder to enable it to climb
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }
        float controlThrow = Input.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f; // enable the player to stop in the middle of the ladder withou the gravity pulling it back to the ground
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    //Setting up the player death when get hit by an enemy or step on a trap
    public void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))//chek if the player got hit by an enemy
        {
            isAlive = false;
            myAnimator.SetTrigger("Hurting");
            GetComponent<Rigidbody2D>().velocity = deathKick; //throw the player in a distance after getting hit
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            StartCoroutine(WaitToLoad()); //enable a time for the animaiton be played before the reset of the level
        }
        else if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))//check if the player step on a trap
        {
            isAlive = false;
            myAnimator.SetTrigger("Hurting");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            StartCoroutine(WaitToLoad());
        }
    }

    //courotine reset the level after an amount of time 
    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();//access a method in other class
    }

    //flipping the sprite to face the right direction
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);//reverse the current scaling of x axis
        }
    }
}

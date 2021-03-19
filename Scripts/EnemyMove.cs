using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //variables
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfVFX = 1f;

    //cache
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myHeadRB;
    AudioSource enemyDeathSFX;

    // caching references
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myHeadRB = GetComponent<CapsuleCollider2D>();
        enemyDeathSFX = GetComponent<AudioSource>();
    }

    //moving the enemy
    void Update()
    {
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = new Vector2(moveSpeed, 0f);//positive velocity to move the enemy to the right
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(-moveSpeed, 0f);//negative velocity to move the enemy to the left
        }
    }

    //setting the enemy direction to the right
    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    //changing the enemy direction when exiting a collision
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x) ,1f);//reverse the current scaling of x axis
    }

    //destroy the enemy when the player jumo over it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        if (myHeadRB.IsTouchingLayers(LayerMask.GetMask("Player")))//check if the enemy got hit by the player
        {
            
            GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, durationOfVFX); //trigger the death visual effect
        }
    }
}

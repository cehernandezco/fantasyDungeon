using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static int health;

    public float speed;
    public float jumpforce;
    public float wallSlidingSpeed;
    public float checkRadius;
    public float attackingRange = 0.5f;
   
    public bool isGrounded = false;
    public bool moveRight = true;
    public bool isTouching;
    public bool wallSliding;

    public Transform ledgeCheck;
    public Transform attack;


    public LayerMask isGround;
    public LayerMask enemy;

    public Animator animator;

    public Rigidbody2D rb2d;

    AudioSource jump;
    AudioSource hit;
    AudioSource walk;
    AudioSource music;

    

  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        health = 3;
        AudioSource[] audios = GetComponents<AudioSource>();
       jump =  audios[0];
        hit = audios[1];
        walk = audios[2];
        music = audios[3];

        music.Play();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float movement = Input.GetAxisRaw("Horizontal"); // to make player move horizontal axis
        rb2d.velocity = new Vector2(movement * speed, rb2d.velocity.y); // make player movement times its speed

        if(movement == 0)
        {
            animator.SetBool("Run",false);
            walk.Play();
        }
        else
        {
            animator.SetBool("Run", true);
            
        }
        
       
        
      
        Jump(); //calling jump function



        //TO FLIP THE CHARACTER SCALE
        Vector3 characterScale = transform.localScale;
        if(Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -1; //change player scale to -1 or opposite direction
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 1;//change player scale to 1
        }
        transform.localScale = characterScale;



        if (Input.GetKeyDown(KeyCode.R))
        {
            Attack(); //calling attack function
            hit.Play();
        }



        isTouching = Physics2D.OverlapCircle(ledgeCheck.position, checkRadius, isGround); //to make sure the player is touching ground

        if(isTouching == true && isGrounded == false && movement == 0)//if the player is touching ground and movement is 0
        {
            wallSliding = true;
            animator.SetBool("Slide", true);//play slide animation
        }
        else
        {
            wallSliding = false;
            animator.SetBool("Slide", false);//not to play slide animation
        }


        if (wallSliding )
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }


    //Funtion To make player jump
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            
            rb2d.velocity = Vector2.up * jumpforce;
            animator.SetTrigger("Jump"); //play animation
            jump.Play();
            print("Jumping");
        }
        
    }

    //Funtion to player Attack
    void Attack()
    {
        
        animator.SetTrigger("Attack1"); //play animation

        Collider2D[] hitEnemey = Physics2D.OverlapCircleAll(attack.position, attackingRange, enemy ); //to hit the enemy with the position and range of the player to enemy
        

        foreach (Collider2D enemy in hitEnemey)
        {
            print("hitting " + enemy.name); //print out name of the enemy
            enemy.GetComponent<EnemyPatrol>().TakeDamage(20); // enemy damage is equals to 20
        }

       
    }


    void OnDrawGizmosSelected() //gizmos to create attack range for the player
    {
        if (attack == null)
            return;
        Gizmos.DrawWireSphere(attack.position, attackingRange);//gizmos is equals to position and range
    }



    public void TakeDamage(int Damage) //take damage funcction
    {
        
        health -= Damage;//if hit then current health will be minus the damage point

        animator.SetTrigger("Hurt");//play hurt animation

        if (health <= 0) //if enemy health is less than or equals to 0 then run the function
        {
            
            Die();//die function


        }

    }


    public  void Die()
    {
       
        animator.SetBool("Die", true); //play death animation
        print("player Died");//print out 
                            // GetComponent<Collider2D>().enabled = true;//to disable the collider after death
        this.enabled = false;//to disable the script after death
        Destroy(gameObject, 2);


    }













}

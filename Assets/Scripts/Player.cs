using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health;
    public Text countText;
    private int count = 0;

    public float distance;
    public float inputVertical;
    public float inputHorizontal;


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
    public LayerMask whatIsLadder;
    public bool isClimbing;

    public Animator animator;

    public Rigidbody2D rb2d;

    AudioSource jump;
    AudioSource hit;
    AudioSource walk;
    AudioSource music;
    AudioSource pickup;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;





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
        pickup = audios[4];
       

        music.Play();
        CountText();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this != null)
        {
            float movement = Input.GetAxisRaw("Horizontal"); // to make player move horizontal axis
            rb2d.velocity = new Vector2(movement * speed, rb2d.velocity.y); // make player movement times its speed

            //climb ladder
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
            if (hitInfo.collider != null )
            {
                print(hitInfo.collider);
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    isClimbing = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isClimbing = false;
                }

            }
            if (isClimbing)
            {
                inputVertical = Input.GetAxisRaw("Vertical");
                rb2d.velocity = new Vector2(rb2d.position.x, inputVertical * speed);
                rb2d.gravityScale = 0;
            }
            else
            {
                rb2d.gravityScale = 3;
            }

            if (movement == 0)
            {
                animator.SetBool("Run", false);
                walk.Play();
            }
            else
            {
                animator.SetBool("Run", true);

            }

            Jump(); //calling jump function



            //TO FLIP THE CHARACTER SCALE
            Vector3 characterScale = transform.localScale;
            if (Input.GetAxis("Horizontal") < 0)
            {
                characterScale.x = -1; //change player scale to -1 or opposite direction
            }
            if (Input.GetAxis("Horizontal") > 0)
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

            if (isTouching == true && isGrounded == false && movement == 0)//if the player is touching ground and movement is 0
            {
                wallSliding = true;
                animator.SetBool("Slide", true);//play slide animation
            }
            else
            {
                wallSliding = false;
                animator.SetBool("Slide", false);//not to play slide animation
            }


            if (wallSliding)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
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

        if (health == 3)
        {
            heart3.SetActive(true);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }

        if (health == 2)
        {
            heart3.SetActive(false);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }

        if (health == 1)
        {
            heart3.SetActive(false);
            heart2.SetActive(false);
            heart1.SetActive(true);
        }

        if (health == 0)
        {
            heart3.SetActive(false);
            heart2.SetActive(false);
            heart1.SetActive(false);


            Die();//die function

            print("dead");
            continueMenu();
        }

      

    }


    public  void Die()
    {
       
        animator.SetBool("Die", true); //play death animation
        print("player Died");//print out 
                           
        this.enabled = false;//to disable the script after death
        Destroy(gameObject, 2);


        continueMenu();

    }


    public void continueMenu()
    {
        SceneManager.LoadScene("Menu");

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            count += 1;
            pickup.Play();
            Destroy(other.gameObject);
            print("its touching");
            CountText();
        }
        if (other.gameObject.CompareTag("spikes"))
        {
            Die();
        }
        if (other.gameObject.CompareTag("closed"))
        {
            
            /*
            if(other.name == "stick_closed")
            {
                other.visible = false;
                
            }*/
        }
        if (other.gameObject.CompareTag("ladder"))
        {
            //climbing ladder
            print("ladder");
            
        }
    }


    void CountText()
    {
        countText.text = " X" + count.ToString();
    }








}

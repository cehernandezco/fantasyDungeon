using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpforce;
    public float wallSlidingSpeed;
    public float checkRadius;
   
    public bool isGrounded = false;
    public bool moveRight = true;
    public bool isTouching;
    public bool wallSliding;

    public Transform ledgeCheck;

    public LayerMask isGround;

    public Animator animator;

    public Rigidbody2D rb2d;

  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float movement = Input.GetAxisRaw("Horizontal"); // to make player move horizontal axis
        rb2d.velocity = new Vector2(movement * speed, rb2d.velocity.y);

        if(movement == 0)
        {
            animator.SetBool("Run",false);
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
            characterScale.x = -1; //change player scale to -1
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 1;//change player scale to 1
        }
        transform.localScale = characterScale;



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Attack(); //calling attack function
        }



        isTouching = Physics2D.OverlapCircle(ledgeCheck.position, checkRadius, isGround);

        if(isTouching == true && isGrounded == false && movement == 0)
        {
            wallSliding = true;
            animator.SetBool("Slide", true);
        }
        else
        {
            wallSliding = false;
            animator.SetBool("Slide", false);
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
            print("Jumping");
        }
        
    }

    //Funtion to player Attack
    void Attack()
    {
        animator.SetTrigger("Attack1"); //play animation

    }

    
    
















}

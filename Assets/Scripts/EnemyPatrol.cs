using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPatrol : MonoBehaviour
{
    /* public int maxHealth;
     public int currentHealth;

     public Animator anim;

     public float patrolSpeed;
     public float moveSpeed;
     public float attackingRange;
     public float distance;


     public bool moveRight;

     public Transform player;
     public Transform attack;

     public LayerMask Player;



    // public float still;




     void Start()
     {
         currentHealth = maxHealth; 
         anim = GetComponent<Animator>();
         player = GameObject.FindGameObjectWithTag("Player").transform;
     }


     void Update()
     {

        Patrol();

     }

     public void Patrol()
     {
         if (moveRight)//if moveright is true then
         {
             transform.Translate(2 * patrolSpeed * Time.deltaTime, 0, 0);
             transform.localScale = new Vector2(1, 1);
             anim.SetTrigger("Run");

         }
         else
         {
             transform.Translate(-2 * patrolSpeed * Time.deltaTime, 0, 0);
             transform.localScale = new Vector2(-1, 1);
             anim.SetTrigger("Run");

         }

     }

     public void TakeDamage(int Damage) //take damage funcction
     {
         currentHealth -= Damage;//if hit then current health will be minus the damage point
         anim.SetTrigger("Hurt");//play hurt animation
         if(currentHealth <= 0) //if enemy health is less than or equals to 0 then run the function
         {
             Die();//die function
         }
     }


     void Die()
     {
         anim.SetBool("Die", true); //play death animation
         print("Enemy Died");//print out 
         GetComponent<Collider2D>().enabled = true;//to disable the collider after death
         this.enabled = false;//to disable the script after death

     }

     void Attack()
     {
         anim.SetTrigger("Attack");


         Collider2D[] hitEnemey = Physics2D.OverlapCircleAll(attack.position, attackingRange, Player); //to hit the enemy with the position and range of the player to enemy

         foreach (Collider2D enemy in hitEnemey)
         {
             print("hitting enemy" + enemy.name); //print out name of the enemy
             enemy.GetComponent<Player>().TakeDamage(20); // enemy damage is equals to 20
         }

     }



     //TO TURN THE ENEMY SIDE DURING PATROL
     void OnTriggerEnter2D( Collider2D other)
     {
         if(other.gameObject.CompareTag("turn"))//comparing tag with thee turn tag(checking)
         {
             if(moveRight)//if moving right 
             {
                 moveRight = false;


             }
             else//else move to right
             {
                 moveRight = true;
             }
         }

     }

     */

    public int maxHealth;
    public int currentHealth;
    public float attackingRange;
    public Animator anim;
    public bool patrol;
    public Rigidbody2D rb2d;
    public float moveSpeed;
    public Transform attack;
    public Transform player;
    public LayerMask Player;
    public Transform groundCheck;
    private bool turn;
    public float range;
    public float disToPlayer;
    public LayerMask groundLayer;
    public GameObject healthbar;
    public Text killCount;
    public int enemyDeath;
    public GameObject blood;

    void Start()
    {
        currentHealth = maxHealth;
        
        anim = GetComponent<Animator>();
        patrol = true;

        enemyDeath = 0;


    }

    

    void Update()
    {
        if(patrol)
        {
            Patrol();
        }

        disToPlayer = Vector2.Distance(transform.position, player.position);
        if(disToPlayer <= range)
        {
            
            if (player.position.x > transform.position.x && transform.localScale.x < 0
                || player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                
                Flip();
                Attack();
            }
            Attack();
            patrol = false;
            rb2d.velocity = Vector2.zero;
            
        }
        else
        {
            patrol = true;
        }

       
    }


    void FixedUpdate()
    {
        if(patrol)
        {
            turn = !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            
        }
    }

    void Patrol()
    {
        if(turn )
        {
            Flip();
        }
        rb2d.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
        anim.SetTrigger("Run");
    }


    void Flip() // to flip the player
    {
        patrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
        patrol = true;
    }

    public void TakeDamage(int Damage) //take damage funcction
    {
        
        healthbar.transform.localScale = new Vector3(currentHealth / maxHealth, healthbar.transform.localScale.x, healthbar.transform.localScale.z);
        currentHealth -= Damage;//if hit then current health will be minus the damage point
        Instantiate(blood, transform.position, Quaternion.identity);
        ScreenShake.instance.StartShake(0.2f, .2f);
        anim.SetTrigger("Hurt");//play hurt animation
        
        if (currentHealth <= 0) //if enemy health is less than or equals to 0 then run the function
        {

            Die();//die function
            

        }
      
    }


    void Die()
    {
        enemyDeath += 1;

        killCount.text = "" + enemyDeath.ToString();
        anim.SetBool("Die", true); //play death animation
        print("Enemy Died");//print out 
       // GetComponent<Collider2D>().enabled = true;//to disable the collider after death
        this.enabled = false;//to disable the script after death
        Destroy(gameObject, 1);
        

    }


    public void Attack()
    {
        
        

      /*  Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attack.position, attackingRange, Player); //to hit the enemy with the position and range of the player to enemy


        foreach (Collider2D player in hitPlayer)
        {
            print("hitting " + player.name); //print out name of the enemy
            player.GetComponent<Player>().TakeDamage(1); // enemy damage is equals to 20

        }*/
        anim.SetTrigger("Attack");
        player.GetComponent<Player>().TakeDamage(1 );
    }


  
}

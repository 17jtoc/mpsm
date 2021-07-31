using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public int maxHealth = 50;
    public int currentHealth;
    private Rigidbody2D myRigidbody;
    private Vector3 mChange;
    private Animator animator;
    public int trapState = 0;
    public int weaponState = 0;
    public int maxWeaponState = 1;
    private int maxTrapState = 3;
    public bool noMove = false;
    public float knockDistance = 300;
    public bool invincible;
    private bool bonked = false;
    private int tool = 0;
    private int honeytrapcount = 0;
    private float oldSpeed;
    private Color oldColor;
    private float knockSize = 300f;

    public int trapsAllowed = 1;

    public Vector3 playerDirection = Vector3.down;
    public GameObject beartrap;
    public GameObject honeytrap;
    public GameObject minetrap;
    public HealthBar healthBar;
    public PlayerHand playerHand;

    public Color flashColor;
    public Color regularColor;
    public Color slowColor;
    public float flashDuration;
    public float numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    //for enemy collision
    public Transform enemyDirection;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        mChange = Vector3.down;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHealth);
        if(mChange.y == -1f)
        {
            playerDirection = Vector3.down;
        }else if(mChange.y == 1f)
        {
            playerDirection = Vector3.up;
        }else if (mChange.x == -1f)
        {
            playerDirection = Vector3.left;
        }else if (mChange.x == 1f)
        {
            playerDirection = Vector3.right;
        }


        if (!bonked)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = 0f;
        }


        playerHand.setWeapon(weaponState);
        playerHand.setTrap(trapState);
        
        

        if (!noMove){
            mChange = Vector3.zero;
            mChange.x = Input.GetAxisRaw("Horizontal");
            mChange.y = Input.GetAxisRaw("Vertical");
        }
        
        var toolSel = Input.GetButtonDown("HorizontalSelector");

        if (toolSel)
        {
            if (tool == 0)
            {
                tool = 1;
                playerHand.setBack(1);
            }
            else
            {
                tool = 0;
                playerHand.setBack(0);
            }
        }

        var stateSel = Input.GetButtonDown("VerticalSelector");
        var stateSelSign = Input.GetAxisRaw("VerticalSelector");

        if (tool == 0)
        {
            if (trapState == 0 && Input.GetButton("Attack") && !noMove)
            {
                StartCoroutine(BearTrapCo());
            }
            else if (trapState == 1 && Input.GetButton("Attack") && !noMove)
            {

                StartCoroutine(HoneyTrapCo());
            }
            else if (trapState == 2 && Input.GetButton("Attack") && !noMove)
            {

                StartCoroutine(MineTrapCo());
            }
            else if (stateSel)
            {

                if(stateSelSign > 0)
                {
                    trapState = trapState + 1;
                    if (trapState == maxTrapState)
                    {
                        trapState = 0;
                    }
                }else if (stateSelSign < 0)
                {
                    trapState = trapState - 1;
                    if (trapState == -1)
                    {
                        trapState = maxTrapState - 1;
                    }
                }


            }
        }
        else
        {
            if (Input.GetButton("Attack") && !noMove)
            {

                StartCoroutine(AttackCo());
            }
            else if (stateSel)
            {

                weaponState = weaponState + 1;
                if (weaponState == maxWeaponState)
                {
                    weaponState = 0;
                }
            }
        }

       
        

        UpdateAnimationAndMove();
        
    }
    
    void UpdateAnimationAndMove()
    {
        if (tool == 1)
        {
            animator.SetBool("knife_selected", true);
        }
        else
        {
            animator.SetBool("knife_selected", false);
        }

        if (mChange != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", mChange.x);
            animator.SetFloat("moveY", mChange.y);
            animator.SetBool("moving_unarmed", true);
        }
        else
        {
            animator.SetBool("moving_unarmed", false);
        }
    }

    void MoveCharacter()
    {
        if (!noMove)
        {
            mChange.Normalize();
            myRigidbody.MovePosition(transform.position + mChange * speed * Time.deltaTime);
        }
        
    }

    private IEnumerator AttackCo()
    {
        noMove = true;
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("attacking", false);
        noMove = false;
        
        
    }

    private IEnumerator BearTrapCo()
    {
        noMove = true;
        LowerTrapCounts();
        animator.SetBool("placing", true);
        if (playerDirection == Vector3.left || playerDirection == Vector3.right)
        {
            
            GameObject newtrap = Instantiate(beartrap, transform.position + playerDirection + Vector3.down * 0.2f, transform.rotation);
            newtrap.gameObject.GetComponent<Trap>().trapCount = trapsAllowed;
        }
        else
        {
            GameObject newtrap = Instantiate(beartrap, transform.position + playerDirection, transform.rotation);
            newtrap.gameObject.GetComponent<Trap>().trapCount = trapsAllowed;
        }
        yield return new WaitForSeconds(0.45f);
        
        animator.SetBool("placing", false);
        noMove = false;


    }

    private IEnumerator HoneyTrapCo()
    {
        currentHealth = currentHealth + 10;
        noMove = true;
        LowerTrapCounts();
        animator.SetBool("placing", true);
        if (playerDirection == Vector3.left || playerDirection == Vector3.right)
        {
            GameObject newtrap = Instantiate(honeytrap, transform.position + playerDirection + Vector3.down * 0.2f, transform.rotation);
            newtrap.gameObject.GetComponent<Trap>().trapCount = trapsAllowed;
        }
        else
        {
            GameObject newtrap = Instantiate(honeytrap, transform.position + playerDirection, transform.rotation);
            newtrap.gameObject.GetComponent<Trap>().trapCount = trapsAllowed;
        }
        
        yield return new WaitForSeconds(0.45f);

        animator.SetBool("placing", false);
        noMove = false;


    }

    private IEnumerator MineTrapCo()
    {
        noMove = true;
        LowerTrapCounts();
        animator.SetBool("placing", true);
        if (playerDirection == Vector3.left || playerDirection == Vector3.right)
        {
            GameObject newtrap = Instantiate(minetrap, transform.position + playerDirection + Vector3.down * 0.2f, transform.rotation);
            newtrap.gameObject.GetComponent<Trap>().trapCount = trapsAllowed;
        }
        else
        {
            GameObject newtrap = Instantiate(minetrap, transform.position + playerDirection, transform.rotation);
            newtrap.gameObject.GetComponent<Trap>().trapCount = trapsAllowed;
        }
        yield return new WaitForSeconds(0.45f);

        animator.SetBool("placing", false);
        noMove = false;


    }


    //COLLISION HANDLING
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
           if (!noMove && !invincible)
            {
                currentHealth = currentHealth - 10;
                enemyDirection = collision.GetComponent<Enemy>().transform;
                knockSize = 500f;
                StartCoroutine(KnockCo());
            }
        }else if(collision.CompareTag("trap"))
        {
            if (!noMove && !invincible)
            {
                Destroy(collision.gameObject);
                if (collision.GetComponent<Trap>().trapType == "bear")
                {
                    currentHealth = currentHealth - 10;
                    StartCoroutine(BearTrappedCo());
                }
                if (collision.GetComponent<Trap>().trapType == "honey")
                {
                     StartCoroutine(HoneyTrappedCo());
                }
                if (collision.GetComponent<Trap>().trapType == "mine")
                {
                    collision.GetComponent<MineTrap>().generateExplosion();
                }

            }
        }else if (collision.CompareTag("explosion"))
        {
            if (!noMove && !invincible)
            {
                currentHealth = currentHealth - 15;
                enemyDirection = collision.GetComponent<Explosion>().transform;
                knockSize = 900f;
                StartCoroutine(KnockCo());
            }
        }
    }

    private IEnumerator KnockCo()
    {
        noMove = true;
        Vector2 difference = transform.position - enemyDirection.position;
        difference = difference.normalized;
        bonked = true;
        animator.SetBool("reg_damage", true);
        StartCoroutine(IncincibleCo());
        myRigidbody.AddForce(difference * knockSize);
        yield return new WaitForSeconds(0.08f);
        animator.SetBool("reg_damage", false);
        noMove = false;
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = 0f;
        bonked = false;


    }


    private IEnumerator BearTrappedCo()
    {
        noMove = true;
        animator.SetBool("bearTrapped", true);
        StartCoroutine(IncincibleCo());
        yield return new WaitForSeconds(0.30f);
        noMove = false;
        animator.SetBool("bearTrapped", false);
    }

    private IEnumerator HoneyTrappedCo()
    {
        honeytrapcount++;
        if(honeytrapcount == 1)
        {
            oldSpeed = speed;
            speed = 2.0f;
            oldColor = regularColor;
            regularColor = slowColor;
            mySprite.color = regularColor;
        }
        yield return new WaitForSeconds(3.30f);
        honeytrapcount--;
        if(honeytrapcount == 0)
        {
            speed = oldSpeed;
            regularColor = oldColor;
            mySprite.color = regularColor;
        }
        


    }

    private IEnumerator IncincibleCo()
    {
        int temp = 0;
        invincible = true;
        while (temp < numberOfFlashes)
        {
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        mySprite.color = regularColor;
        invincible = false;
    }

    private void LowerTrapCounts()
    {
        Trap[] traps = FindObjectsOfType<Trap>();
        foreach (Trap i in traps){
            i.trapCount--;
            
        }
    }

}

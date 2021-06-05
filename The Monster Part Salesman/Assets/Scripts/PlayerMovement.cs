using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 mChange;
    private Animator animator;
    public int trapState = 0;
    public int weaponState = 0;
    public int maxWeaponState = 1;
    public int maxTrapState = 1;
    public bool noMove = false;
    public float knockDistance = 300;
    public bool invincible;
    private bool bonked = false;
    private int tool = 0;

    public Vector3 playerDirection = Vector3.down;
    public GameObject beartrap;

    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public float numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    //for enemy collision
    public Transform enemyDirection;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        mChange = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {

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
            }
            else
            {
                tool = 0;
            }
        }

        var stateSel = Input.GetButtonDown("VerticalSelector");

        if(tool == 0)
        {
            if (Input.GetButton("Attack") && !noMove)
            {

                StartCoroutine(BearTrapCo());
            }
            else if (stateSel)
            {

                trapState = trapState + 1;
                if (trapState == maxTrapState)
                {
                    trapState = 0;
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

        animator.SetBool("placing", true);
        Instantiate(beartrap, transform.position + playerDirection, transform.rotation);
        yield return new WaitForSeconds(0.45f);
        
        animator.SetBool("placing", false);
        noMove = false;


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
           if (!noMove && !invincible)
            {
                enemyDirection = collision.GetComponent<Enemy>().transform;
                StartCoroutine(KnockCo());
            }
        }else if(collision.CompareTag("trap"))
        {
            if (!noMove && !invincible)
            {
                enemyDirection = collision.GetComponent<BearTrap>().transform;
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
        myRigidbody.AddForce(difference * 300f);
        yield return new WaitForSeconds(0.08f);
        animator.SetBool("reg_damage", false);
        noMove = false;
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = 0f;
        bonked = false;


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

}

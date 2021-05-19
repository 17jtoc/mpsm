using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 mChange;
    private Animator animator;
    public int state = 0;
    public int maxState = 2;
    public bool noMove = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (!noMove){
            mChange = Vector3.zero;
            mChange.x = Input.GetAxisRaw("Horizontal");
            mChange.y = Input.GetAxisRaw("Vertical");
        }
        
        var sel = Input.GetButtonDown("VerticalSelector");

        if (Input.GetButton("Attack") && state == 1 && !noMove)
        {
            
            StartCoroutine(AttackCo());
        }
        else if (sel)
        {
            
            state = state + 1;
            if(state == maxState)
            {
                state = 0;
            }
        }
        

        UpdateAnimationAndMove();
        
    }

    void UpdateAnimationAndMove()
    {
        if (state == 1)
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
}

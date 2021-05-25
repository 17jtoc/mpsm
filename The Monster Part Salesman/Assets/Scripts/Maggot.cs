using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : Enemy

    
{
    private int knockCount = 0;
    private Animator anim;
    private Rigidbody2D myRigidbody;
    private bool striking = false;
    private Vector2 tempStrike;
    private bool strikeCD = false;
    private bool bonked = false;
    private int CDcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        anim.SetFloat("moveX", 0);
        anim.SetFloat("moveY", -1);
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bonked)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = 0f;
        }
        CheckDist();
    }

    public override void Hurt()
    {
        
        anim.SetBool("smack", true);
        StartCoroutine(ImGood());
        knockCount++;
        StartCoroutine(CoolDown());
        attackRange = 1.5f;
        moveSpeed = 1f;



    }

    private IEnumerator ImGood()
    {
        bonked = true;
        Vector2 difference = transform.position - hitPos.position;
        difference = difference.normalized;
        myRigidbody.AddForce(difference * 30f);
        yield return new WaitForSeconds(0.12f);
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = 0f;
        bonked = false;
        yield return new WaitForSeconds(0.5f);
        knockCount--;
        if (knockCount < 1)
        {
            anim.SetBool("smack", false);
            
        }
        
        
        
    }

    void CheckDist()
    {
        if (striking)
        {
            anim.SetFloat("moveX", tempStrike.x);
            anim.SetFloat("moveY", tempStrike.y);
            Vector3 temp = Vector3.MoveTowards(transform.position, tempStrike, moveSpeed * Time.deltaTime);
            myRigidbody.MovePosition(temp);

        }
        else if(Vector3.Distance(target.position,transform.position) <= chaseRange && Vector3.Distance(target.position, transform.position) > attackRange && knockCount < 1)
        {
            
            anim.SetFloat("moveX", target.position.x - transform.position.x);
            anim.SetFloat("moveY", target.position.y - transform.position.y);
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            myRigidbody.MovePosition(temp);
        }
        else if(Vector3.Distance(target.position, transform.position) < 3f && !strikeCD)
        {
            tempStrike = target.position - transform.position;
            tempStrike = tempStrike * 30f;
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        anim.SetBool("strike", true);
        attackRange = 0f;
        moveSpeed = 0f;
        yield return new WaitForSeconds(0.30f);
        striking = true;
        attackRange = 0.1f;
        moveSpeed = 5f;
        yield return new WaitForSeconds(0.30f);
        attackRange = 1.5f;
        moveSpeed = 1f;
        anim.SetBool("strike", false);
        striking = false;
        StartCoroutine(CoolDown());

    }

    private IEnumerator CoolDown()
    {
        CDcount++;
        strikeCD = true;
        yield return new WaitForSeconds(1.80f);
        CDcount--;
        if(CDcount < 1)
        {
            strikeCD = false;
        }
        
    }
}

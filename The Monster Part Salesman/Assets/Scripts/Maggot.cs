using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : Enemy

    
{
    private bool trapped = false;
    private int knockCount = 0;
    private Animator anim;
    private Rigidbody2D myRigidbody;
    private bool striking = false;
    private Vector2 tempStrike;
    private bool strikeCD = false;
    private bool bonked = false;
    private int CDcount = 0;
    private float slowForce = 1.0f;
    public SpriteRenderer mySprite;
    public int honeytrapcount = 0;


    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
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

        if (trapped)
        {
            anim.SetBool("smack", true);
            StartCoroutine(ImGoodTrapped());
            knockCount++;
            StartCoroutine(CoolDown());
        }
        else
        {
            anim.SetBool("smack", true);
            StartCoroutine(ImGood());
            knockCount++;
            StartCoroutine(CoolDown());
        }
        
        
        



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
        
        knockCount--;
        if (knockCount < 1)
        {
            anim.SetBool("smack", false);
            
        }
        
        
        
    }

    private IEnumerator ImGoodTrapped()
    {
        anim.SetBool("traphit", true);
        yield return new WaitForSeconds(0.12f);
        

        knockCount--;
        if (knockCount < 1)
        {
            
            anim.SetBool("smack", false);

        }



    }

    void CheckDist()
    {
        if(Vector3.Distance(target.position,transform.position) <= chaseRange && Vector3.Distance(target.position, transform.position) > attackRange && knockCount < 1 && !striking && !trapped)
        {
            
            anim.SetFloat("moveX", target.position.x - transform.position.x);
            anim.SetFloat("moveY", target.position.y - transform.position.y);
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            myRigidbody.MovePosition(temp);
        }
        else if(Vector3.Distance(target.position, transform.position) < 3f && !strikeCD && !striking && !trapped)
        {
            tempStrike = target.position - transform.position;
            tempStrike = tempStrike.normalized;
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        anim.SetBool("strike", true);
        
        
        striking = true;
        yield return new WaitForSeconds(0.30f);
        
        bonked = true;
        myRigidbody.AddForce(tempStrike * 300f * slowForce);
        yield return new WaitForSeconds(0.30f);
        bonked = false;
        
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("trap") && !trapped)
        {
            
            Destroy(collision.gameObject);
            if (collision.GetComponent<Trap>().trapType == "bear")
            {
                StartCoroutine(BearTrappedCo());
            }
            if (collision.GetComponent<Trap>().trapType == "honey")
            {

                StartCoroutine(HoneyTrappedCo());


            }

            
            
            
            
        }
    }

    private IEnumerator BearTrappedCo()
    {
        myRigidbody.bodyType = RigidbodyType2D.Static;
        trapped = true;
        bonked = false;
        strikeCD = true;
        
        anim.SetBool("bearTrap", true);
        
        yield return new WaitForSeconds(6.00f);
        strikeCD = false;
        anim.SetBool("bearTrap", false);
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;
        trapped = false;
        StartCoroutine(CoolDown());
    }

    private IEnumerator HoneyTrappedCo()
    {
        honeytrapcount++;
        if(honeytrapcount == 1)
        {
            slowForce = 0.5f;
            moveSpeed = 0.5f;
            mySprite.color = slowColor;
        }
        yield return new WaitForSeconds(6.00f);
        honeytrapcount--;
        if(honeytrapcount == 0)
        {
            slowForce = 1.0f;
            moveSpeed = 1.0f;
            mySprite.color = regularColor;
        }
        

    }
}

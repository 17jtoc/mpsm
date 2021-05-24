using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : Enemy

    
{
    private int knockCount = 0;
    private Animator anim;
    private Rigidbody2D myRigidbody;

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
        CheckDist();
    }

    public override void Hurt()
    {
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;
        anim.SetBool("smack", true);
        StartCoroutine(ImGood());
        knockCount++;
        
        


    }

    private IEnumerator ImGood()
    {
        Vector2 difference = transform.position - hitPos.position;
        difference = difference.normalized;
        myRigidbody.AddForce(difference * 30f);
        yield return new WaitForSeconds(0.12f);
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = 0f;
        myRigidbody.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.5f);
        knockCount--;
        if (knockCount < 1)
        {
            anim.SetBool("smack", false);
        }
        
        
    }

    void CheckDist()
    {
        if(Vector3.Distance(target.position,transform.position) <= chaseRange && Vector3.Distance(target.position, transform.position) > attackRange && knockCount < 1)
        {
            
            anim.SetFloat("moveX", target.position.x - transform.position.x);
            anim.SetFloat("moveY", target.position.y - transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}

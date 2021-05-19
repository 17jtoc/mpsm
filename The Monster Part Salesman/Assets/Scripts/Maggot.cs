using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : Enemy

    
{
    
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        anim.SetFloat("moveX", 0);
        anim.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDist();
    }

    public override void Hurt()
    {
        var smacked = anim.GetBool("smack");
        if (!smacked)
        {
            anim.SetBool("smack", true);
            StartCoroutine(ImGood());
        } 
       
    }

    private IEnumerator ImGood()
    {
        yield return new WaitForSeconds(0.45f);
        anim.SetBool("smack", false);
    }

    void CheckDist()
    {
        if(Vector3.Distance(target.position,transform.position) <= chaseRange && Vector3.Distance(target.position, transform.position) > attackRange)
        {
            
            anim.SetFloat("moveX", target.position.x - transform.position.x);
            anim.SetFloat("moveY", target.position.y - transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}

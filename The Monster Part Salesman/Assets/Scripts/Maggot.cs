using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : MonoBehaviour

    
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hurt()
    {
        anim.SetBool("smack", true);
        StartCoroutine(ImGood());
    }

    private IEnumerator ImGood()
    {
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("smack", false);
    }
}

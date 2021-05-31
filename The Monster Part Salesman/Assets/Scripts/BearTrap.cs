using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{

    private Animator animator;
    public bool ready = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrapCo());
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TrapCo()
    {
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("readyTrap", false);
        ready = true;
    }
}

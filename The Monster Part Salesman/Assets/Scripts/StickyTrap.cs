﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyTrap : Trap
{
    private Animator animator;
    private Collider2D trapCollider;
    public bool ready = false;
    public bool debug = false;
    
    // Start is called before the first frame update
    void Start()
    {
        trapType = "honey";
        StartCoroutine(TrapCo());
        animator = GetComponent<Animator>();
        trapCollider = GetComponent<Collider2D>();
        trapCollider.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (trapCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator TrapCo()
    {
        yield return new WaitForSeconds(0.40f);
        animator.SetBool("readyTrap", true);
        ready = true;
        trapCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        debug = true;
        if (collision.CompareTag("destroytrap"))
        {

            Object.Destroy(this);



        }
    }
}

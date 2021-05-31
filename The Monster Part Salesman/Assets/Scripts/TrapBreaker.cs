using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBreaker : MonoBehaviour
{
    public bool debug = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        debug = true;
        if (collision.CompareTag("trap"))
        {

            Destroy(collision.gameObject);



        }
    }
}

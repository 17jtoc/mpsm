using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
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
        if (collision.CompareTag("enemy"))
        {

            collision.GetComponent<Enemy>().hitPos = transform;
            collision.GetComponent<Enemy>().Hurt();
        }
    }
}

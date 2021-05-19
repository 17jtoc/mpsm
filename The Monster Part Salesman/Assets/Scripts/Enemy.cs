using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health;
    public float moveSpeed;
    public Transform target;
    public float chaseRange;
    public float attackRange;
    public Transform home;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Hurt() { }
}

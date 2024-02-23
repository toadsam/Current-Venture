using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody bulletRid;

    void Start()
    {
        bulletRid = GetComponent<Rigidbody>();
        bulletRid.velocity = transform.forward * speed;

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //피 닳는거 추가
        }
    }
}

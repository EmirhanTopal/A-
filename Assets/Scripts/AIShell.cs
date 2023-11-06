using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShell : MonoBehaviour
{

    public GameObject explosion;
    private Rigidbody rb;
    
    void OnCollisionEnter(Collision col) {

        if (col.gameObject.tag == "tank") {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }
    
    void Start()
    {
       rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = rb.velocity;
    }
}

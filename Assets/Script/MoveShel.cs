using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MoveShell : MonoBehaviour
{
    private float speed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0,speed * Time.deltaTime * 0.5f ,speed * Time.deltaTime);
    }
}

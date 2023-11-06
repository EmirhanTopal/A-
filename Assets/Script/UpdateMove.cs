using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMove : MonoBehaviour
{
    float speed = 2f;
    void Update()
    {
        this.transform.Translate(0,0,speed * Time.deltaTime);
    }
}

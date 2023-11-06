using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateUpdateMove : MonoBehaviour
{
    float speed = 2f;
    void LateUpdate()
    {
        this.transform.Translate(0,0,speed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float strafe;
    float foward;
    float turn;
    float speed = 6f;
    float turnSpeed = 10f;

    public void Update()
    {
        strafe = Input.GetAxis("Horizontal");
        foward = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(strafe, 0, foward) * speed * Time.deltaTime);
    }
}

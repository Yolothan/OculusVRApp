using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESRollBall : MonoBehaviour
{
    // Start is called before the first frame update
    public float BallSpeed = 10;
    public float JumpForce = 100f;
    Rigidbody myrigidbody;
    void Start()
    {
        myrigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //jump
        float j = 0;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            j = JumpForce * Input.GetAxis("Jump") * JumpForce;
        }
        Vector3 force = new Vector3(Input.GetAxis("Horizontal") * BallSpeed, j, Input.GetAxis("Vertical") * BallSpeed);
        myrigidbody.AddForce(force);
    }
}

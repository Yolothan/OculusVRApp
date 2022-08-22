using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_ctrl : MonoBehaviour
{
    public float jumpforce = 50f;
    public Rigidbody _rigidbody;
    public bool jump = false;
    [SerializeField] private float speed = 50f;
    [SerializeField] private bool isgrounded = false;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }
    //
    private void FixedUpdate()
    {
        Movement();
    }
    //
    private void Movement()
    {
        if (_rigidbody != null)
        {
            _jump();
            //move the ball
            float horizontal = Input.GetAxisRaw("Horizontal") * speed;
            float vertical = Input.GetAxisRaw("Vertical") * speed;
            _rigidbody.AddForce(new Vector3(horizontal, _rigidbody.velocity.y,vertical));
        }
    }
    //
    private void _jump()
    {
        //check ground
        //RaycastHit raycastHit = new RaycastHit();
        //isgrounded = Physics.Raycast(this.transform.position, -transform.up, out raycastHit, 1.5f);
        Debug.DrawRay(this.transform.position, -transform.up);
        if (Input.GetButton("Jump"))
        {
            jump = true;
        }
        //
        if (isgrounded && jump)
        {
            _rigidbody.AddForce(new Vector3(_rigidbody.velocity.x, jumpforce,_rigidbody.velocity.z));
            jump = false;
        }
    }
    //
    private void OnTriggerEnter(Collider collider)
    {
        // if(collider.tag !=  this.gameObject.tag)
        isgrounded = true;
    }
    private void OnTriggerExit(Collider collider)
    {
        // if(collider.tag !=  this.gameObject.tag)
        isgrounded = false;
    }
}

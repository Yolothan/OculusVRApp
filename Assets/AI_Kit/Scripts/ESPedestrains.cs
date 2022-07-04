using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESPedestrains : MonoBehaviour
{
    // Declaration
    [HideInInspector] public bool ReverseNavigationMode;
    [HideInInspector] public bool ReturnControls, S_peds;

    [HideInInspector] public float NavigationSpeed = 3;
    [HideInInspector] public float UpdateDistance = 1.5f;
    [HideInInspector] public float AvoidanceDistance = 2f, T;
    [HideInInspector]
    public Transform TargetNode;
    [HideInInspector] public Animator m_Animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public ESGameManager gameManager;
    Vector3 playerVelocity, move, BodyRotation, RelativePoint;
    float diff, FowardValue, TurnValue, mul = 1;
    [HideInInspector] public float M, navacc;
    //[SerializeField]
    float Manuever = 1, count, backspeed, exitnormalizedTime;
    //[SerializeField]
    bool StopTracking = false, callanim, IScrossing, flee;
    bool Stop, avoid, S_T;
    int AvoidIndex, randme, animclipindex, StopProb;
    string statename;
    ESRagdollz ragdollz;
    Transform Triggered;
    List<Collider> _collider;

    void Start()
    {
        //starts at begin
        backspeed = NavigationSpeed;
        controller = this.GetComponent<CharacterController>();
        //controller.detectCollisions = false;
        //InvokeRepeating("enablecoll", 0.05f, .5f);
        m_Animator = this.GetComponent<Animator>();
        ragdollz = this.GetComponent<ESRagdollz>();
        InvokeRepeating("Callprobabilty", 0.0f, 2f);
        InvokeRepeating("InvokeFlee", 0.0f, .95f);
    }
    //
    //enables collision for charactercntroller
    //
    //Reacts based on collision
    private void OnTriggerEnter(Collider hit)
    {
        //as promised, i have made peds react based on player collision ,
        /*
        here you can your methods ,like
        -peds can say hi to player if hit returns true.
        -peeds run from player
        -peds attacks player ......................             // its unlimited what yoou can add here 
        (Thanks for choosing IPS)
        */
        if (hit.CompareTag("Player"))
        {
            Rigidbody body = hit.attachedRigidbody;
            //check for rigidbody
            if (body == null || body.isKinematic)
            {
                return;
            }
            if (body.velocity.magnitude > .5f && body.velocity.magnitude < gameManager.DeathSpeed)
            {
                // peds will fall 
                ESRagdollz rags = this.GetComponent<ESRagdollz>();
                if (rags == null) return;
                if (!rags.Ragdollz)
                {
                    rags.AddbusrtEffect = true;
                    rags.power = body.velocity.magnitude * 100f;
                    rags.Ragdollz = true;

                }
            }
            if (body.velocity.magnitude > gameManager.DeathSpeed)
            {
                //peds can die 
                ESRagdollz rags = this.GetComponent<ESRagdollz>();
                if (rags == null) return;
                if (!rags.Ragdollz)
                {
                    rags.AddbusrtEffect = true;
                    rags.power = body.velocity.magnitude * 100f;
                    rags.Ragdollz = true;
                    rags.die = true;
                }
                //
                Destroy(this.gameObject, gameManager.corpseDuration);
            }
        }
        //
    }
    //
    void RunFromIncoming()
    {
        if (flee)
        {
            if (Vector3.Distance(this.transform.position, gameManager.Player.transform.position) > 10f)
            {
                StopProb = 0;
                StopTracking = false;
                S_peds = false;
                statename = "";
                callanim = false;
                m_Animator.SetTrigger("Return");
                flee = false;
            }
            NavigationSpeed = gameManager.FleeSpeed;
            float lerp = 0;
            //calculate movement of peds and store in the move local variable.
            move = NavigationSpeed * mul * Vector3.forward + lerp * Vector3.right;
            //multiply the body rotation with velocity in order to  make peds move in direction they face.
            move = transform.rotation * move;
            //apply rotation :)
            transform.Rotate(BodyRotation);
            //move = moveplayer;
            //if character controller is enabled , then apply movement.
            if (controller.enabled == true)
                controller.Move(move * Time.deltaTime);


            playerVelocity.y -= 9.98f * Time.deltaTime;
            if (controller.enabled == true)
                controller.Move(playerVelocity * Time.deltaTime);

            FowardValue = 1.5f;
        }
    }
    //
    void CheckForbjectIncoming()
    {
        //blast a sphere to see anything unfreindly is incoming with speed
        _collider = new List<Collider>();
        Collider[] hits = Physics.OverlapSphere(this.transform.position, gameManager.DangerZone);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; ++i)
            {
                if (hits[i].attachedRigidbody != null)
                {
                    if (hits[i].attachedRigidbody.transform != this.transform && hits[i].attachedRigidbody.CompareTag("Player"))
                    {
                        if (hits[i].attachedRigidbody.velocity.magnitude >= gameManager.DeathSpeed)
                        {
                            Triggered = hits[i].transform;
                            flee = true;
                        }
                    }
                }
            }
        }
        //

    }
    //
    //
    void InvokeFlee()
    {
        CheckForbjectIncoming();
    }
    //
    void Callprobabilty()
    {
        if (!StopTracking && !flee && !ragdollz.Ragdollz)
        {
            StopProb = Random.Range(0, gameManager.StopProirty);
            animclipindex = Random.Range(0, 3);
        }
    }
    //
    void FixedUpdate()
    {
        if (ragdollz != null)
        {
            if (ragdollz.Ragdollz)
            {
                StopProb = 0;
                StopTracking = false;
                S_peds = false;
                statename = "";
                callanim = false;
                flee = false;
                NavigationSpeed = backspeed;
                m_Animator.SetTrigger("Return");
            }
        }
        if (!StopTracking)
        {
            PedsMovement();
            if (gameManager.EnableSmartWondering && IScrossing == false)
            {
                if (StopProb == 1)
                {
                    callanim = false;
                    StopTracking = true;
                    S_peds = true;
                }
            }
            //

        }
        else
        {
            NavigationSpeed = 0.0f;
        }
        //
        RunFromIncoming();
        //
        PlayAnim();
    }
    //Handles player animation
    void PlayAnim()
    {
        if (NavigationSpeed != 0)
        {
            //moves peds based on fowardvalue with repect to Time deltatime.
            m_Animator.SetFloat("Forward", FowardValue, 0.1f, Time.deltaTime);
        }
        else
        if (S_peds == false)
            m_Animator.SetFloat("Forward", FowardValue * 0.0f, 0.1f, Time.deltaTime);
        //m_Animator.SetBool("Crouch", m_Crouching);
        //updates the turn animation value.
        m_Animator.SetFloat("Turn", TurnValue, 0.1f, Time.deltaTime);
        //
        if (StopTracking && S_peds)
        {
            if (!callanim)
            {
                m_Animator.ResetTrigger("Return");
                statename = animclipindex == 0 ? gameManager.wonderAnim1 :
                 animclipindex == 1 ? gameManager.wonderAnim2 : gameManager.wonderAnim3;
                m_Animator.Play(statename, 0, 0);

                callanim = true;
            }
            //
            if (callanim)
            {
                if (statename == gameManager.wonderAnim1 || statename == gameManager.wonderAnim2
                || statename == gameManager.wonderAnim3)
                {
                    exitnormalizedTime = m_Animator.GetCurrentAnimatorStateInfo(0).IsName(statename) ? m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0.0f;
                    if (exitnormalizedTime > 0.8f)
                    {
                        StopProb = 0;
                        StopTracking = false;
                        S_peds = false;
                        statename = "";
                        callanim = false;
                        m_Animator.SetTrigger("Return");
                    }
                }
            }
        }
    }
    //controls pedestrain movement
    void PedsMovement()
    {
        //do not execute if target not found.
        if (TargetNode == null) return;
        if (flee) return;
        //move to target;
        //gets the relative point of the target gameobject.

        //
        float lerp = 0;
        //calculate movement of peds and store in the move local variable.
        move = NavigationSpeed * mul * Vector3.forward + lerp * Vector3.right;
        //multiply the body rotation with velocity in order to  make peds move in direction they face.
        move = transform.rotation * move;
        //apply rotation :)
        transform.Rotate(BodyRotation);
        //move = moveplayer;
        //if character controller is enabled , then apply movement.
        if (controller.enabled == true)
            controller.Move(move * Time.deltaTime);
        //declare a defualt value for as PI.
        float Pi = .5f;
        //Get magnitude of body and try to normalize with PI.
        T = (controller.velocity.normalized.magnitude * Pi);
        FowardValue = T;
        //controller.velocity = (TopSpeed / Pi) * CarRb.velocity.normalized;
        //
        playerVelocity.y -= 9.98f * Time.deltaTime;
        if (controller.enabled == true)
            controller.Move(playerVelocity * Time.deltaTime);
        //focus at target
        //var step = 500 * Time.deltaTime;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetNode.rotation, step);
        Vector3 Tarpos = TargetNode.position;
        //T = controller.velocity.magnitude;
        Vector3 relativepoint = transform.InverseTransformDirection(move);
        TurnValue = Mathf.Atan2(relativepoint.x, relativepoint.z);
        //Checks for the probabilty to avoid an object.
        avoid = !Stop && T < 0.1f && Vector3.Distance(Tarpos, transform.position) > UpdateDistance ? true : avoid;
        //if true, run 
        if (avoid)
        {
            count += Time.deltaTime;
            if (count > 5)
            {
                avoid = false;
            }
        }
        else
        {
            //reset 
            count = 0;
        }
        //applys value based on avoid status
        Manuever = avoid ? M : 1;
        if (TargetNode.GetComponent<ESNodeIdentity>().ZebraCrossNodes.Count == 0)
        {
            if (AvoidIndex == 1)
            {
                Tarpos += TargetNode.right * navacc * Manuever;
            }
            else if (AvoidIndex == 0)
            {
                Tarpos -= TargetNode.right * navacc * Manuever;
            }
            else
            {
                if (Manuever <= 1)
                    Tarpos = TargetNode.position;
                else
                {
                    Tarpos += TargetNode.right * navacc * Manuever;
                }
            }
        }
        else
        {
            Tarpos = TargetNode.position;
        }        //
        transform.LookAt(Tarpos);
        Vector3 euler = transform.eulerAngles;
        euler.z = 0;
        euler.x = 0;
        transform.eulerAngles = euler;
        //
        ESNodeIdentity TarIdentity = TargetNode.GetComponent<ESNodeIdentity>();
        if (TarIdentity.Stop && Vector3.Distance(this.transform.position,
         TargetNode.position) < 1.5f)
        {
            //
            NavigationSpeed = 0;
        }
        else
        {
            NavigationSpeed = backspeed;
        }
        //
        //Updating the Target based on how close peds is to target.
        if (Vector3.Distance(this.transform.position, Tarpos) < UpdateDistance)
        {
            //random set direction for ped to avoid
            AvoidIndex = Random.Range(0, 3);
            //
            if (!ReverseNavigationMode && !TarIdentity.Stop)
            {
                if (TarIdentity.CrossingPeds == false)
                    if (TarIdentity.NextNode != null)
                    {
                        IScrossing = false;
                        TargetNode = TarIdentity.NextNode;
                        /*
                        //adds a little random movement to peds
                        int randmode = Random.Range(0, 10);
                        if (randmode <= 4)
                        {
                            ReverseNavigationMode = true;
                        }
                        else
                        {
                            //if next node is avaible and peds is due to update next node then change target to nextnode
                           
                        }
                        */
                    }
                    else
                    {
                        //if not , peds try to repeat nodes in reverse mode
                        ReverseNavigationMode = true;
                    }
                else
                {
                    /*
                      runs when peds are waiting on zebra cross
                    */
                    IScrossing = true;
                    if (TarIdentity.Ccount < TarIdentity.ZebraCrossNodes.Count)
                    {
                        TargetNode = TarIdentity.ZebraCrossNodes[TarIdentity.Ccount];
                        TarIdentity.Ccount++;
                    }
                    else
                    {
                        ReverseNavigationMode = true;
                    }
                }
            }
            else
            {
                if (TargetNode.GetComponent<ESNodeIdentity>().PreNode != null)
                {
                    TargetNode = TargetNode.GetComponent<ESNodeIdentity>().PreNode;
                }
                else
                {
                    ReverseNavigationMode = false;
                }
            }
        }
    }
}

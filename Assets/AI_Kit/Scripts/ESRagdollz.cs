using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESRagdollz : MonoBehaviour
{
    /*
    this scripts controls the ragdoll system of the pedestrains , detects when its right
    to enable or disable ragdoll.
    */
    public GameObject RagSkelentonParent;

    //
    [Header("AnimationName")]
    [SerializeField] private string FromBelly = "GetUpFromBelly";
    [SerializeField] private string FromBehind = "GetUpFromBehind";
    [Tooltip("Based On The Get UP Animation Length")]
    [SerializeField] private float RagdollDelay = 0.7f;

    [HideInInspector] public List<Collider> Ragcols;
    [HideInInspector] public List<Rigidbody> RagRigids;
    [HideInInspector] public bool die;
    [HideInInspector] public bool AddbusrtEffect = false;
    [HideInInspector] public float raduis = 250.0f;
    [HideInInspector] public float power = 1500.0f;
    [Header("ReadOnly")]
    public string BoneState = "Null";
    public bool Ragdollz;
    private Animator animator;
    private CapsuleCollider capsule;
    private Transform BodyHipBone;
    private bool BackPose;
    private List<Vector3> StoredPosition, PrivPosition;
    private List<Quaternion> StoredRotation, PrivRotation;
    private List<Transform> BonesTransformComp;
    private bool BlendRagdollz, startragdollcountdown, isSet = true;
    CharacterController controller;
    private string statename;
    float _ragdollingEndTime, StataCountDownTime;
    const float RagdollToMecanimBlendTime = 0.5f;
    [SerializeField] float ragcount, exitnormalizedTime;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        BodyHipBone = animator.GetBoneTransform(HumanBodyBones.Hips);
        BonesTransformComp = new List<Transform>();
        Transform[] Bones = RagSkelentonParent.GetComponentsInChildren<Transform>();
        foreach (Transform B in Bones)
        {
            if (B != transform)
            {
                BonesTransformComp.Add(B);
            }
        }
        RefreshRags();
        if (isSet) UnSetRags();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RagdollPhysics(Ragdollz);
        checkForStaticBone();
        if (Ragdollz && !die)
        {
            if (BoneState == "Calm")
            {
                ragcount += Time.deltaTime;
                if (ragcount > RagdollDelay)
                {
                    ragcount = 0.0f;
                    Ragdollz = false;

                }
            }
        }
        else
        {
            if (statename == FromBelly || statename == FromBehind)
            {
                exitnormalizedTime = animator.GetCurrentAnimatorStateInfo(0).IsName(statename) ? animator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0.0f;
                if (exitnormalizedTime > 0.8f)
                {
                    if (controller != null)
                        controller.enabled = true;
                    statename = "";
                }
            }
        }
    }
    //
    private void UnSetRags()
    {
        //
        if (Ragcols.Count > 0)
            foreach (Collider c in Ragcols)
            {
                c.isTrigger = true;
                c.enabled = false;
            }
        //
        if (RagRigids.Count > 0)
        {
            foreach (Rigidbody R in RagRigids)
            {
                R.isKinematic = true;
            }
        }
        isSet = false;
    }
    //
    private void SetRags()
    {
        //
        if (Ragcols.Count > 0)
            foreach (Collider c in Ragcols)
            {
                c.isTrigger = false;
                c.enabled = true;
            }
        if (RagRigids.Count > 0)
        {
            foreach (Rigidbody R in RagRigids)
            {
                R.isKinematic = false;
            }
        }
        isSet = false;
    }
    private void RefreshRags()
    {
        RagRigids = new List<Rigidbody>();
        Ragcols = new List<Collider>();
        //
        Collider[] cols = RagSkelentonParent.GetComponentsInChildren<Collider>();
        Rigidbody[] RigsBody = RagSkelentonParent.GetComponentsInChildren<Rigidbody>();
        foreach (Collider c in cols)
        {
            Ragcols.Add(c);
        }
        //
        foreach (Rigidbody R in RigsBody)
        {
            RagRigids.Add(R);
        }
    }
    //
    void LateUpdate()
    {
        //
        BlendRagdollzToMecanim();
    }
    void BlendRagdollzToMecanim()
    {
        if (BlendRagdollz == false) return;
        float ragdollBlendAmount = 1f - Mathf.InverseLerp(
             _ragdollingEndTime,
             _ragdollingEndTime + RagdollToMecanimBlendTime,
             Time.time);

        //        print(ragdollBlendAmount);
        if (BackPose)
        {

            for (int i = 0; i < BonesTransformComp.Count; ++i)
            {
                if (PrivRotation[i] != BonesTransformComp[i].localRotation)
                {
                    PrivRotation[i] = Quaternion.Slerp(BonesTransformComp[i].localRotation, StoredRotation[i], ragdollBlendAmount);
                    BonesTransformComp[i].localRotation = PrivRotation[i];
                }
                if (PrivPosition[i] != BonesTransformComp[i].localPosition)
                {
                    PrivPosition[i] = Vector3.Slerp(BonesTransformComp[i].localPosition, StoredPosition[i], ragdollBlendAmount);
                    BonesTransformComp[i].localPosition = PrivPosition[i];
                }
            }
        }
        if (Mathf.Abs(ragdollBlendAmount) < Mathf.Epsilon)
        {
            BackPose = false;
            BlendRagdollz = false;

            if (isSet)
                UnSetRags();
        }
    }

    private bool CheckIfLieOnBack()
    {
        var left = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
        var right = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).position;
        var hipsPos = BodyHipBone.position;

        left -= hipsPos;
        left.y = 0f;
        right -= hipsPos;
        right.y = 0f;

        var q = Quaternion.FromToRotation(left, Vector3.right);
        var t = q * right;

        return t.z < 0f;
    }
    void GetRagdollPosition()
    {
        StoredPosition = new List<Vector3>();
        StoredRotation = new List<Quaternion>();
        PrivPosition = new List<Vector3>();
        PrivRotation = new List<Quaternion>();
        foreach (Transform T in BonesTransformComp)
        {
            StoredPosition.Add(T.localPosition);
            PrivPosition.Add(T.localPosition);
            StoredRotation.Add(T.localRotation);
            PrivRotation.Add(T.localRotation);
        }
    }

    void GetUp()
    {
        if (BoneState == "Calm" && !Ragdollz)
        {
            _ragdollingEndTime = Time.time;
            BackPose = true;
            BlendRagdollz = true;
            animator.enabled = true;
            Vector3 shiftPos = BodyHipBone.position - transform.position;
            CorrectCharacterControllerCoordinate(shiftPos);
            GetRagdollPosition();

            statename = CheckIfLieOnBack() ? FromBehind : FromBelly;
            animator.Play(statename, 0, 0);
            StataCountDownTime = 0.0f;

            if (capsule != null)
                capsule.enabled = true;
            if (this.GetComponent<ESPedestrains>() != null)
            {
                this.GetComponent<ESPedestrains>().ReturnControls = true;
            }
            BoneState = "Null";
        }
        if (BoneState != "Null")
            return;
    }
    //

    void checkForStaticBone()
    {
        GetUp();
        if (!Ragdollz) return;
        Rigidbody hiprigid = BodyHipBone.GetComponent<Rigidbody>();

        if (AddbusrtEffect)
        {
            //print("done");
            Vector3 explosionpos = hiprigid.position;
            Collider[] bodycolliders = Physics.OverlapSphere(explosionpos, raduis);
            foreach (Collider hit in bodycolliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if (rb.CompareTag("Player"))
                    {
                        //do nothing
                    }
                    else
                    {
                        rb.AddExplosionForce(power, explosionpos, raduis, 3.0f);
                    }
                }
            }
            AddbusrtEffect = false;
        }
        if (hiprigid.velocity.sqrMagnitude < 0.01f)
            BoneState = "Calm";
        else
            BoneState = "Jerky";
    }
    bool ShootAray()
    {
        RaycastHit hit;
        bool CheckifFacedDown = Physics.Raycast(BodyHipBone.transform.position, BodyHipBone.transform.forward, out hit, 10);
        Debug.DrawLine(BodyHipBone.transform.position, hit.point, Color.red);
        return CheckifFacedDown;
    }
    //
    void RagdollPhysics(bool Status)
    {

        if (Ragdollz)
        {
            if (!isSet)
                SetRags();
            startragdollcountdown = true;
            animator.enabled = false;
            if (controller != null)
                controller.enabled = false;
        }
    }
    //
    //
    private Vector3 HipBoneDirection()
    {
        Vector3 ragdolledFeetPosition = (
            animator.GetBoneTransform(HumanBodyBones.Hips).position);
        Vector3 ragdolledHeadPosition = animator.GetBoneTransform(HumanBodyBones.Head).position;
        Vector3 ragdollDirection = ragdolledFeetPosition - ragdolledHeadPosition;
        ragdollDirection.y = 0;
        ragdollDirection = ragdollDirection.normalized;
        //
        if (CheckIfLieOnBack())
            return ragdollDirection;
        else
            return -ragdollDirection;
    }
    //

    void CorrectCharacterControllerCoordinate(Vector3 JumpPos)
    {
        Vector3 ragdollDirection = HipBoneDirection();
        BodyHipBone.position -= JumpPos;
        transform.position += JumpPos;


        Vector3 forward = transform.forward;
        transform.rotation = Quaternion.FromToRotation(forward, ragdollDirection) * transform.rotation;
        BodyHipBone.rotation = Quaternion.FromToRotation(ragdollDirection, forward) * BodyHipBone.rotation;
    }
}

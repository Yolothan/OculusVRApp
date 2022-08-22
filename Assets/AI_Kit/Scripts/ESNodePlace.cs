using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class ESNodePlace : MonoBehaviour
{
    /*
     This script spawns peds into the game scene , based player position
     handles Zebra cross  node arrangement and zebra cross nodes.

    */
    [HideInInspector] public GameObject Node;
    public GameObject ConnectedNodeParent;
    [HideInInspector] public float MinSpeed = 2;
    [HideInInspector] public float MaxSpeed = 2;

    public float ArrowSize = 2f;
    public bool UseInverse = true;
    public bool AutoItrate = true;
    public float SpawnRate = 1.5f;

    [Range(1, 20)]
    public int Row = 3, Columb = 2;
    [Range(0, 10)]
    public float FowardSpacing = 2, SideSpacing = 2, Spacing = 2;
    [HideInInspector]
    public bool done, full;
    [HideInInspector] public List<Transform> Nodelist = new List<Transform>(1);
    [HideInInspector] public GameObject FirstConnectedNode, RowParent, HumanHolder;
    [HideInInspector] public ESGameManager GameManager;
    //[SerializeField]
    int SpawnCount = 10, Cnum;
    //[SerializeField] 
    float Zcount, mul;
    List<Transform> MainKids;
    [HideInInspector] public List<Transform> LocalHumanList = new List<Transform>();
    Transform[] childtrans;
    void Start()
    {
        GameManager = GameObject.FindObjectOfType<ESGameManager>();
        GameManager.Player = GameObject.FindWithTag("Player");
        SpawnRate = GameManager.SpawnRate;
        if (GameManager.MaxCount == GameManager.MinCount)
            SpawnCount = GameManager.MaxCount;
        else
            SpawnCount = Random.Range(GameManager.MinCount, GameManager.MaxCount);
        //
        MinSpeed = GameManager.MinSpeed;
        MaxSpeed = GameManager.MaxSpeed;
        LocalHumanList = new List<Transform>();
        NodeMeshManager();
        //calls human spawn method based on spawn rate.
        InvokeRepeating("CallHumans", 0.1f, SpawnRate);
    }
    // Update is called once per framen
    void NodeMeshManager()
    {
        if (Nodelist.Count == 0 || Nodelist == null) return;
        Material NodeMat = Resources.Load("Node/Trans") as Material;
        for (int i = 0; i < Nodelist.Count; ++i)
        {
            Nodelist[i].GetComponent<MeshRenderer>().sharedMaterial = NodeMat;
        }
        //
        if (childtrans != null)
            if (childtrans.Length > 0)
            {
                for (int i = 0; i < childtrans.Length; ++i)
                {
                    if (childtrans[i].transform != RowParent.transform)
                    {
                        childtrans[i].GetComponent<MeshRenderer>().sharedMaterial = NodeMat;
                    }
                }
            }
    }
    //
    void Update()
    {
        if (Application.isPlaying == false)
        {
            PrepareFisrtNodePedCross();
            if (GameManager != null)
                ArrowSize = GameManager.ArrowSize;
        }
        if (Application.isPlaying)
        {
            ShuffleHumans();
            ZebraCrossControl();
        }
        //
    }
    //
    void ShuffleHumans()
    {
        if (HumanHolder == null) return;
        Transform[] Temp_T = HumanHolder.GetComponentsInChildren<Transform>();
        LocalHumanList = new List<Transform>();
        if (Temp_T.Length > 0)
        {
            for (int i = 0; i < Temp_T.Length; ++i)
            {
                if (Temp_T[i].transform != HumanHolder.transform && Temp_T[i].GetComponent<ESPedestrains>() != null)
                {
                    //stores all spawned humans in a container
                    LocalHumanList.Add(Temp_T[i]);
                }
            }
        }
        //checks if spawned humans have exceeded limit  
        if (LocalHumanList.Count > SpawnCount)
        {
            full = true;
        }
        else
        {
            full = false;
        }
    }
    //
    void CallHumans()
    {

        if (Application.isPlaying == false) return;
        //
        if (GameManager.OnPlayerPosition)
        {
            if (GameManager.Player == null) return;

        }
        //
        if (GameManager == null)
        {
            Debug.Log("GameManager  Can not spawnHumans because it is missing");
            return;
        }
        //
        ShuffleHumans();
        if (full) return;
        if (GameManager.HumanPrefab.Length > 0)
        {
            int NodeIndex = Random.Range(0, Nodelist.Count);
            int humanindex = Random.Range(0, GameManager.HumanPrefab.Length);
            if (Vector3.Distance(GameManager.Player.transform.position, Nodelist[NodeIndex].transform.position) < 100)
            {
                bool Canspawn = false;
                if (!Nodelist[NodeIndex].GetComponent<MeshRenderer>().isVisible)
                {
                    Canspawn = true;
                }
                else
                {
                    if (Vector3.Distance(GameManager.Player.transform.position, Nodelist[NodeIndex].transform.position) <= (GameManager.PlayerDistance) &&
                    Vector3.Distance(GameManager.Player.transform.position, Nodelist[NodeIndex].transform.position) > (GameManager.PlayerDistance) - 5)
                        Canspawn = true;
                    else
                        Canspawn = false;
                }
                //
                if (Canspawn)
                    if (Nodelist[NodeIndex].GetComponent<ESNodeIdentity>().NextNode != null && Nodelist[NodeIndex].GetComponent<ESNodeIdentity>().PreNode != null)
                    {
                        if (HumanHolder == null)
                        {
                            GameObject G = new GameObject("HumanHolder");
                            G.transform.parent = this.transform.parent;
                            HumanHolder = G;
                        }
                        //
                        int I_index = Random.Range(0, 2);
                        float val = GameManager.RandomizeSpawnPoint;
                        Vector3 Randomspawnpos = new Vector3(Nodelist[NodeIndex].position.x + Random.Range(-val, val), Nodelist[NodeIndex].position.y, Nodelist[NodeIndex].position.z + Random.Range(-val, val));
                        Transform H = Instantiate(GameManager.HumanPrefab[humanindex], Randomspawnpos, Nodelist[NodeIndex].rotation);
                        if (HumanHolder != null)
                        {
                            H.parent = HumanHolder.transform;
                        }
                        H.GetComponent<ESPedestrains>().M = GameManager.ManueverAccuracy;
                        H.GetComponent<ESPedestrains>().navacc = GameManager.PedsNavigationAccuracy;
                        H.GetComponent<ESPedestrains>().gameManager = GameManager;
                        if (H.GetComponent<ESHumanTrafficManager>() != null)
                        {
                            H.GetComponent<ESHumanTrafficManager>().Player = GameManager.Player;
                            H.GetComponent<ESHumanTrafficManager>().dist = GameManager.PlayerDistance;
                            H.GetComponent<ESHumanTrafficManager>().DeleteBasednplayer = GameManager.OnPlayerPosition;

                        }
                        //set walk speed of peds
                        if (MinSpeed == MaxSpeed)
                            H.GetComponent<ESPedestrains>().NavigationSpeed = MinSpeed;
                        else
                        {
                            float ActualSpeed = Random.Range(MinSpeed, MaxSpeed);
                            H.GetComponent<ESPedestrains>().NavigationSpeed = ActualSpeed;
                        }


                        if (I_index == 1)
                        {
                            H.GetComponent<ESPedestrains>().ReverseNavigationMode = false;
                            H.GetComponent<ESPedestrains>().TargetNode = Nodelist[NodeIndex].GetComponent<ESNodeIdentity>().NextNode;
                        }
                        else
                        {
                            H.GetComponent<ESPedestrains>().ReverseNavigationMode = true;
                            H.GetComponent<ESPedestrains>().TargetNode = Nodelist[NodeIndex].GetComponent<ESNodeIdentity>().PreNode;
                        }
                    }
                //
            }
        }
    }
    //
    void ZebraCrossControl()
    {
        ESNodeIdentity nodeIdentity = Nodelist[Nodelist.Count - 1].GetComponent<ESNodeIdentity>();
        if (nodeIdentity.ZebraCrossNodes.Count == 0) return;
        childtrans = RowParent.transform.GetComponentsInChildren<Transform>();
        if (childtrans.Length < 0)
        {
            return;
        }
        Zcount += Time.deltaTime;
        if (Zcount > GameManager.ZebraCrossWaitTime)
        {
            nodeIdentity.CrossingPeds = !nodeIdentity.CrossingPeds;
            if (nodeIdentity.CrossingPeds == false)
            {
                nodeIdentity.Ccount = 0;
                if (childtrans.Length > 0)
                {
                    for (int i = 0; i < childtrans.Length; ++i)
                    {
                        if (childtrans[i].transform != RowParent.transform)
                        {
                            childtrans[i].GetComponent<ESNodeIdentity>().Occupied = false;
                            childtrans[i].GetComponent<ESNodeIdentity>().Stop = false;
                        }
                    }
                }
            }
            else
            {
                if (childtrans.Length > 0)
                {
                    for (int i = 0; i < childtrans.Length; ++i)
                    {
                        if (childtrans[i].transform != RowParent.transform)
                        {
                            childtrans[i].GetComponent<ESNodeIdentity>().Stop = true;
                        }
                    }
                }
            }
            Zcount = 0;
        }
    }
    //
    void PrepareFisrtNodePedCross()
    {
        //this method generates the zebra cross nodes
        if (ConnectedNodeParent == null) return;
        //finds all nodes assigned to nodeparent gamobject.
        Transform[] conntrans = ConnectedNodeParent.GetComponentsInChildren<Transform>();
        //finds all child in childconntrans
        List<Transform> ChildconnTrans = new List<Transform>();
        if (conntrans.Length > 0)
        {
            //loop
            for (int i = 0; i < conntrans.Length; ++i)
                if (ConnectedNodeParent.transform != conntrans[i].transform)
                {
                    //move all child to a new container excluding the parent
                    ChildconnTrans.Add(conntrans[i].transform);
                }
        }
        //
        if (ChildconnTrans.Count > 1)
        {
            //gets the first node in the list
            FirstConnectedNode = ChildconnTrans[0].gameObject;
        }
        //runs if auto_Itrate is true 
        if (AutoItrate == false) return;
        if (FirstConnectedNode != null)
        {
            //creates a parent gameobject to hold zebra crossing nodes.
            if (RowParent == null)
            {
                ESNodeIdentity nodeIdentity = Nodelist[Nodelist.Count - 1].GetComponent<ESNodeIdentity>();
                int WaitOnZebraCrossIndex = Random.Range(0, 2);
                if (WaitOnZebraCrossIndex == 0)
                {
                    nodeIdentity.GetComponent<ESNodeIdentity>().CrossingPeds = true;
                }
                else
                {
                    nodeIdentity.GetComponent<ESNodeIdentity>().CrossingPeds = false;
                }
                GameObject G = new GameObject("RowParent");
                G.transform.parent = this.transform.parent;
                RowParent = G;
                for (int i = 0; i < Row; ++i)
                {
                    GameObject newnode = Instantiate(Node.gameObject, this.transform.position, Quaternion.identity);
                    newnode.AddComponent<ESNodeIdentity>();
                    newnode.transform.parent = RowParent.transform;
                }
            }
            //

            //arrange in position
            if (RowParent != null)
            {
                childtrans = RowParent.transform.GetComponentsInChildren<Transform>();
                MainKids = new List<Transform>();
                ESNodeIdentity nodeIdentity = Nodelist[Nodelist.Count - 1].GetComponent<ESNodeIdentity>();
                nodeIdentity.ZebraCrossNodes = new List<Transform>();
                //
                for (int i = 0; i < childtrans.Length; ++i)
                {
                    if (childtrans[i].transform != RowParent.transform)
                    {
                        MainKids.Add(childtrans[i].transform);
                    }
                }
                //matrix ittrate ;)
                if (childtrans.Length > 0)
                {
                    mul = 1;
                    float Fspace = FowardSpacing;
                    for (int i = 0; i < MainKids.Count; ++i)
                    {
                        if (i % Columb == 0)
                        {
                            //mul = 1;
                            mul *= 1.5f;
                            Cnum = 0;
                        }
                        if (MainKids[i].transform != RowParent.transform)
                        {
                            //Vector3 pos = Nodelist[Nodelist.Count - 1].position;
                            nodeIdentity.ZebraCrossNodes.Add(MainKids[i]);
                            MainKids[i].GetComponent<ESNodeIdentity>().NextNode = FirstConnectedNode.transform;
                            MainKids[i].transform.position = Nodelist[Nodelist.Count - 1].position;
                            if (Nodelist.Count > 1)
                                MainKids[i].transform.rotation = Nodelist[Nodelist.Count - 2].rotation;
                            //
                            MainKids[i].transform.position += MainKids[i].transform.forward * (Fspace * mul);
                            float inverse = UseInverse ? -1 : 1;
                            Cnum++;
                            MainKids[i].transform.position += MainKids[i].transform.right * SideSpacing * Cnum * inverse;

                        }
                    }
                }

            }

        }
    }

}

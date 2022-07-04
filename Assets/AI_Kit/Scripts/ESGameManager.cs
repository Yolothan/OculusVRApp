using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESGameManager : MonoBehaviour
{
    [HideInInspector] public GameObject Player;
    public bool OnPlayerPosition = true;
    public bool EnableSmartWondering = true;
    public string wonderAnim1 = "wonder1";
    public string wonderAnim2 = "wonder2";
    public string wonderAnim3 = "wonder3";
    [Range(3, 50)]
    public int StopProirty = 5;
    public float ArrowSize = 2, SpawnRate = 1.5f;
    [Range(0, 20)]
    public float RandomizeSpawnPoint = 3.5f;
    public float PedsNavigationAccuracy = 2;
    public float SpawnRaduis = 100;
    public float PlayerDistance = 50f;
    public float FleeSpeed = 5f;
    public float DangerZone = 50f;
    public float DeathSpeed = 5f;
    public float corpseDuration = 5f;
    // Start is called before the first frame update
    [Header("Randomize Human Speed")]
    [Range(0.0000001f, 6)] public float MinSpeed = 2;
    [Range(0.000001f, 6)] public float MaxSpeed = 2;
    [Header("Randomize Human SpawnCount")]
    [Range(1, 1000)] public int MinCount = 10;
    [Range(1, 1000)] public int MaxCount = 10;
    [Header("ManueverAccuracy")]
    public float ManueverAccuracy = 20f;
    public float ZebraCrossWaitTime = 5;
    public Transform[] HumanPrefab;
}

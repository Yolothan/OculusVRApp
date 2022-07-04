namespace TurnTheGameOn.SimpleTrafficSystem
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.Events;

    [System.Serializable]
    public struct AITrafficCarOnReachWaypointInfo
    {
        public AITrafficWaypointRoute parentRoute;
        public AITrafficWaypoint waypoint;
        [HideInInspector] public AITrafficWaypoint nextPointInRoute;
        public int waypointIndexnumber;
        public float speedLimit;
        public bool stopDriving;
        public AITrafficWaypoint[] newRoutePoints;
        public List<AITrafficWaypoint> laneChangePoints;
        public List<AITrafficWaypointRouteInfo> yieldTriggers;
        public UnityEvent OnReachWaypointEvent;
        [HideInInspector] public Vector3 position;
    }
}
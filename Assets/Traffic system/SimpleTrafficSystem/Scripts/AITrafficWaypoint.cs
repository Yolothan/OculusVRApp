namespace TurnTheGameOn.SimpleTrafficSystem
{
    using UnityEngine;
    using System.Collections.Generic;

    public class AITrafficWaypoint : MonoBehaviour
    {
        public AITrafficCarOnReachWaypointInfo onReachWaypointSettings;
        private BoxCollider m_collider;
        

        private void OnEnable()
        {
            onReachWaypointSettings.position = transform.position;
            if (onReachWaypointSettings.parentRoute.waypointDataList.Count > onReachWaypointSettings.waypointIndexnumber)
            {
                onReachWaypointSettings.nextPointInRoute = onReachWaypointSettings.parentRoute.waypointDataList[onReachWaypointSettings.waypointIndexnumber]._waypoint;
            }
            if (onReachWaypointSettings.waypointIndexnumber < onReachWaypointSettings.parentRoute.waypointDataList.Count)
            {
                onReachWaypointSettings.waypoint = this;
            }
        }

        void OnTriggerEnter(Collider col)
        {
            col.transform.root.SendMessage("OnReachedWaypoint", onReachWaypointSettings, SendMessageOptions.DontRequireReceiver);
            if (onReachWaypointSettings.waypointIndexnumber == onReachWaypointSettings.parentRoute.waypointDataList.Count)
            {
                if (onReachWaypointSettings.newRoutePoints.Length == 0)
                {
                    col.transform.root.SendMessage("StopDriving", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public void TriggerNextWaypoint(AITrafficCar _AITrafficCar)
        {
            _AITrafficCar.OnReachedWaypoint(onReachWaypointSettings);
            if (onReachWaypointSettings.waypointIndexnumber == onReachWaypointSettings.parentRoute.waypointDataList.Count)
            {
                if (onReachWaypointSettings.newRoutePoints.Length == 0)
                {
                    _AITrafficCar.StopDriving();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (m_collider == null)
            {
                m_collider = GetComponent<BoxCollider>();
            }
            
            if (m_collider != null)
            {
                Gizmos.color = new Color(1, 1, 1, 0.75f);
                DrawCube
                    (
                    transform.position,
                    transform.rotation,
                    transform.localScale,
                    m_collider.center,
                    m_collider.size
                    );
            }
            
        }

        void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 center, Vector3 size)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;
            Gizmos.DrawCube(center, size);
            Gizmos.matrix = oldGizmosMatrix;
        }

        private List<AITrafficWaypoint> newWaypointList = new List<AITrafficWaypoint>();

        public void RemoveMissingLaneChangePoints()
        {
            newWaypointList = new List<AITrafficWaypoint>();
            for (int i = 0; i < onReachWaypointSettings.laneChangePoints.Count; i++)
            {
                if (onReachWaypointSettings.laneChangePoints[i] != null)
                {
                    newWaypointList.Add(onReachWaypointSettings.laneChangePoints[i]);
                }
            }
            onReachWaypointSettings.laneChangePoints = new List<AITrafficWaypoint>(newWaypointList);
        }

        public void RemoveMissingNewRoutePoints()
        {
            newWaypointList.Clear();
            for (int i = 0; i < onReachWaypointSettings.newRoutePoints.Length; i++)
            {
                if (onReachWaypointSettings.newRoutePoints[i] != null)
                {
                    newWaypointList.Add(onReachWaypointSettings.newRoutePoints[i]);
                }
            }
            onReachWaypointSettings.newRoutePoints = newWaypointList.ToArray();
        }

    }
}
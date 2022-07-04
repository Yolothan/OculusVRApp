namespace TurnTheGameOn.SimpleTrafficSystem
{
    using UnityEngine;

    public class AITrafficWaypointRouteInfo : MonoBehaviour
    {
        public bool stopForTrafficLight;
        public bool yieldForTrafficLight;
        public BoxCollider yieldTrigger;
        private Collider[] hitColliders;

        private void Update()
        {
            if (yieldTrigger != null)
            {
                hitColliders = Physics.OverlapBox(yieldTrigger.transform.position, yieldTrigger.size / 2, Quaternion.identity, AITrafficController.Instance.layerMask);
                yieldForTrafficLight = hitColliders.Length > 0 ? true : false;
            }
        }

        private void OnDisable()
        {
            yieldForTrafficLight = false;
        }
    }
}
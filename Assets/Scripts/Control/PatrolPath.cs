using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;

            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int index)
        {
            if(index == transform.childCount - 1)  { return 0; }

            return index + 1;
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}

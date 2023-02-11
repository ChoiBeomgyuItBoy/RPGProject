using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData 
    {
        GameObject user;
        Vector3 targetedPoint;
        IEnumerable<GameObject> targets;

        public AbilityData(GameObject user)
        {
            this.user = user;
        }

        public IEnumerable<GameObject> GetTargets()
        {
            return targets;
        }

        public GameObject GetUser()
        {
            return user;
        }

        public Vector3 GetTargetedPoint()
        {
            return targetedPoint;
        }

        public void SetTargetedPoint(Vector3 targetedPoint)
        {
            this.targetedPoint = targetedPoint;
        }

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            this.targets = targets;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData 
    {
        GameObject user;
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

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            this.targets = targets;
        }
    }
}

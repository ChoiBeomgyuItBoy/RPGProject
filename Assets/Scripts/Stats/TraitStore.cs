using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour
    {
        Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        int unassignedPoints = 10;

        public int GetUnassignedPoints()
        {
            return unassignedPoints;
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetPoints(trait) + GetStagedPoints(trait);
        }

        public int GetPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        public void AssignPoints(Trait trait, int points)
        {
            if(!CanAsignPoints(trait, points)) return;

            stagedPoints[trait] = GetStagedPoints(trait) + points;
            unassignedPoints -= points;
        }

        public bool CanAsignPoints(Trait trait, int points)
        {
            if(GetStagedPoints(trait) + points < 0) return false;
            if(unassignedPoints < points) return false;

            return true;
        }

        public void Commit()
        {
            foreach(Trait trait in stagedPoints.Keys)
            {
                assignedPoints[trait] = GetProposedPoints(trait);
            }

            stagedPoints.Clear();
        }
    }
}

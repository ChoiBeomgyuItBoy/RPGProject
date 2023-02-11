using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Filters/Tag Filter")]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] string tagToFilter = "";

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach(var objectToFilter in objectsToFilter)
            {
                if(objectToFilter.CompareTag(tagToFilter))
                {
                    yield return objectToFilter;
                }
            }
        }
    }
}
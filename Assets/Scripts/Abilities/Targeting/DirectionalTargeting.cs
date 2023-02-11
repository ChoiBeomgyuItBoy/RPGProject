using System;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Targeting/Directional Targeting")]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit hit;

            Ray ray  = PlayerController.GetMouseRay();

            if(Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                data.SetTargetedPoint(hit.point + ray.direction * groundOffset / ray.direction.y);
            }

            finished();
        }
    }
}

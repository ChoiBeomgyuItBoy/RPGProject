using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private GameObject player;
        private Fighter fighter;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if(GetComponent<Health>().IsDead) return;

            if(IsInChaseRange() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else 
            {
                fighter.Cancel();
            }
        }

        private bool IsInChaseRange()
        {
            float playerDistanceSqr = (transform.position - player.transform.position).sqrMagnitude;
            float chaseDistanceSqr = chaseDistance * chaseDistance;

            return playerDistanceSqr < chaseDistanceSqr;
        }
    }
}

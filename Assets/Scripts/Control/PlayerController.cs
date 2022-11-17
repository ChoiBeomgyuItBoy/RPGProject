using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    [RequireComponent(typeof(InputReader), typeof(Mover), typeof(Fighter))]
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if(!GetComponent<Fighter>().CanAttack(target)) continue;

                if(GetComponent<InputReader>().IsClicking)
                {
                    GetComponent<Fighter>().Attack(target);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if(hasHit)
            {
                if(GetComponent<InputReader>().IsClicking)
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }

                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

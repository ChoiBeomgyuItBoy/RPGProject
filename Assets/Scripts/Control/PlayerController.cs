using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    [RequireComponent(typeof(InputReader), typeof(Mover), typeof(Fighter))]
    public class PlayerController : MonoBehaviour
    {
        private InputReader inputReader;
        private Mover mover;
        private Fighter fighter;

        private void Start()
        {
            inputReader = GetComponent<InputReader>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

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

                if(target == null) continue; 

                if(inputReader.IsClicking)
                {
                    fighter.Attack(target);
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
                if(inputReader.IsClicking)
                {
                    mover.StartMoveAction(hit.point);
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

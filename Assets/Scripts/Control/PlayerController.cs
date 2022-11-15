using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private InputReader inputReader;
        private Mover mover;

        private void Start()
        {
            inputReader = GetComponent<InputReader>();
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if(inputReader.IsClicking) { MoveToCursor(); }
        }

        private void MoveToCursor()
        {
            Ray ray = GetCameraRay();
            RaycastHit hit = GetRaycastHit(ray);

            if(hit.transform != null)
            {
                mover.MoveTo(hit.point);
            }
        }

        private RaycastHit GetRaycastHit(Ray ray)
        {
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            return hit;
        }

        private Ray GetCameraRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

using UnityEngine;

namespace RPG.Core
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 10;

        void Update()
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
        }
    }
}

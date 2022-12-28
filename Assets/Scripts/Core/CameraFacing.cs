using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        Transform mainCameraTransform;

        void Start()
        {
            mainCameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.forward = mainCameraTransform.transform.forward;
        }
    }
}

using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private new ParticleSystem particleSystem;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if(!particleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
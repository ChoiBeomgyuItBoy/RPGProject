using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Targeting/Delayed Click")]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] GameObject targetingEffectPrefab;
        [SerializeField] float heightEffectOffset = 0.31f;

        GameObject targetingEffectInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;

            if(targetingEffectInstance == null)
            {
                targetingEffectInstance = Instantiate(targetingEffectPrefab);
            }
            else
            {
                targetingEffectInstance.SetActive(true);
            }

            targetingEffectInstance.transform.localScale = new Vector3(areaAffectRadius * 2, 0, areaAffectRadius * 2);

            while(true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                RaycastHit raycastHit;

                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    Vector3 targetingEffectOffset = Vector3.up * heightEffectOffset;
                    targetingEffectInstance.transform.position = raycastHit.point + targetingEffectOffset;

                    if(Input.GetMouseButtonDown(0))
                    {
                        // Get track of mouse click while in this condition
                        yield return new WaitWhile(() => Input.GetMouseButton(0));

                        playerController.enabled = true;
                        targetingEffectInstance.gameObject.SetActive(false);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        finished();

                        yield break;
                    }
                }

                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {

            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);

            foreach(var hit in hits)
            {
                yield return hit.collider.gameObject;
            }          
        }
    }
}
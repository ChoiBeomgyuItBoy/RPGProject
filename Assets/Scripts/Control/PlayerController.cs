using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using GameDevTV.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotSpot;
        }

        [SerializeField] InputReader inputReader;
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 40;
        [SerializeField] float raycastRadius = 1;

        bool isDraggingUI = false;
        Health health = null;
        ActionStore actionStore = null;
        const int actionSlots = 6;

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public InputReader GetInputReader()
        {
            return inputReader;
        }

        void Awake()
        {
            health = GetComponent<Health>();
            actionStore = GetComponent<ActionStore>();
        }

        void Update()
        {
            if(health.IsDead) 
            {
                SetCursor(CursorType.None);
                return;
            }

            UseAbilites();

            if(InteractWithUI()) return;
            if(InteractWithMovement()) return;
            if(InteractWithComponent()) return;

            SetCursor(CursorType.None);
        }

        bool InteractWithUI()
        {
            if(Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }

            if(EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButton(0))
                {
                    isDraggingUI = true;
                }

                SetCursor(CursorType.UI);
                return true;
            }

            if(isDraggingUI)
            {
                return true;
            }

            return false;
        }

        void UseAbilites()
        {
            if(Input.GetKeyDown(inputReader.GetFirstAbilityKey()))
            {
                actionStore.Use(0, gameObject);
            }
            if(Input.GetKeyDown(inputReader.GetSecondAbilityKey()))
            {
                actionStore.Use(1, gameObject);
            }
            if(Input.GetKeyDown(inputReader.GetThirdAbilityKey()))
            {
                actionStore.Use(2, gameObject);
            }
            if(Input.GetKeyDown(inputReader.GetFourthAbilityKey()))
            {
                actionStore.Use(3, gameObject);
            }
            if(Input.GetKeyDown(inputReader.GetFifthAbilityKey()))
            {
                actionStore.Use(4, gameObject);
            }
            if(Input.GetKeyDown(inputReader.GetSixthAbilityKey()))
            {
                actionStore.Use(5, gameObject);
            }
        }

        bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        bool InteractWithMovement()
        {
            Vector3 target;

            bool hasHit = RaycastNavMesh(out target);

            if(hasHit)
            {
                if(!GetComponent<Mover>().CanMoveTo(target)) return false;

                if(Input.GetKey(inputReader.GetMovementKey()))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Movement);

                return true;
            }

            return false;
        }

        bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out raycastHit);

            if(!hasHit) return false;

            NavMeshHit navMeshHit;

            bool hasCastToNavMesh = 
            (
                NavMesh.SamplePosition
                (
                    raycastHit.point, 
                    out navMeshHit, 
                    maxNavMeshProjectionDistance, 
                    NavMesh.AllAreas
                )
            );

            if(!hasCastToNavMesh) return false;
            
            target = navMeshHit.position;

            return true;
        }

        void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotSpot, CursorMode.Auto);
        }

        CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping cursorMap in cursorMappings)
            {
                if(cursorMap.type == type)
                {
                    return cursorMap;
                }
            }

            return cursorMappings[0];
        }
    }
}

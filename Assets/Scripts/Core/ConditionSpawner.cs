using UnityEngine;
using GameDevTV.Utils;
using System.Collections.Generic;

namespace RPG.Core
{
    public class ConditionSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] objectsToSpawn;
        [SerializeField] Condition condition;
        IEnumerable<IPredicateEvaluator> evaluators;

        void Awake()
        {
            evaluators = GameObject.FindWithTag("Player").GetComponents<IPredicateEvaluator>();
        }
        
        void Start()
        {   
            foreach(var gameObject in objectsToSpawn)
            {
                gameObject.SetActive(condition.Check(evaluators));
            }
        }   
    }
}
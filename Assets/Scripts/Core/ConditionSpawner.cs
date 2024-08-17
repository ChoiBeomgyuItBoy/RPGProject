using UnityEngine;
using GameDevTV.Utils;
using System.Collections.Generic;

namespace RPG.Core
{
    public class ConditionSpawner : MonoBehaviour
    {
        [SerializeField] Condition conditionToSpawn;
        IEnumerable<IPredicateEvaluator> evaluators;

        void Awake()
        {
            evaluators = GameObject.FindWithTag("Player").GetComponents<IPredicateEvaluator>();
        }

        void Start()
        {
            gameObject.SetActive(conditionToSpawn.Check(evaluators));
        }
    }
}
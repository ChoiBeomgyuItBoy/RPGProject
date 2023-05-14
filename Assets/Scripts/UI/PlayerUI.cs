using RPG.Combat;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    void Awake()
    {
        GameObject.FindWithTag("Player").GetComponent<Fighter>().onWeaponUpdated += GetComponent<Fighter>().EquipWeapon;
    }
}

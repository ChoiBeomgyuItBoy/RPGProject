using System.Collections.Generic;
using GameDevTV.Utils;
using PsychoticLab;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] Condition equipCondition;
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;
        [SerializeField] CharacterPartPath[] characterParts;

        // PUBLIC

        public bool CanEquip(EquipLocation equipLocation, Equipment equipment)
        {
            if(equipLocation != allowedEquipLocation) return false;

            return equipCondition.Check(equipment.GetComponents<IPredicateEvaluator>());
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

        public void ToggleCharacterParts(CharacterCustomizer customizer, bool enabled)
        {
            if(characterParts == null) return;

            foreach(var part in characterParts)
            {
                customizer.SetCharacterPart(part, enabled);
            }
        }
    }
}
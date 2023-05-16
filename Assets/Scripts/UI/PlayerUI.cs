using GameDevTV.Inventories;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Equipment>().onItemAdded += GetComponent<Equipment>().AddItem;
        player.GetComponent<Equipment>().onItemRemoved += GetComponent<Equipment>().RemoveItem;
    }
}

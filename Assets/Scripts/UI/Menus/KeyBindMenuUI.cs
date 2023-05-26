using RPG.Control;
using TMPro;
using UnityEngine;

namespace RPG.UI.Menus
{
    public class KeyBindMenuUI : MonoBehaviour
    {
        [SerializeField] KeyBindRowUI keyBindRowPrefab;
        [SerializeField] Transform listRoot;
        [SerializeField] GameObject choosingText;
        InputReader inputReader;

        void Start()
        {
            inputReader = InputReader.GetPlayerInputReader();
            inputReader.onChange += Redraw;
            Redraw();
        }

        void Redraw()
        {
            if(listRoot == null) return;

            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach(var pair in inputReader.GetActionPairs())
            {
                var rowInstance = Instantiate(keyBindRowPrefab, listRoot);
                rowInstance.Setup(inputReader, pair.Key, pair.Value, choosingText);
            }
        }
    }
}
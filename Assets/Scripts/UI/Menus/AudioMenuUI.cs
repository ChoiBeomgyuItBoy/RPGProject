using RPG.Audio;
using UnityEngine;

namespace RPG.UI.Menus
{
    public class AudioMenuUI : MonoBehaviour
    {
        [SerializeField] AudioSettingsSO audioSettingsSO;    
        [SerializeField] Transform listRoot;
        [SerializeField] AudioRowUI audioRowPrefab;

        void Start()
        {
            Redraw();
        }   

        void Redraw()
        {
            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach(var audioPair in audioSettingsSO.GetAudioPair())
            {
                var rowInstance = Instantiate(audioRowPrefab, listRoot);
                rowInstance.Setup(audioSettingsSO, audioPair.Key, audioPair.Value);
            }
        }
    }
}

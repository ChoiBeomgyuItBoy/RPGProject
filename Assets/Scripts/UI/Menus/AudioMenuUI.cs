using GameDevTV.Utils;
using RPG.Audio;
using UnityEngine;

namespace RPG.UI.Menus
{
    public class AudioMenuUI : MonoBehaviour
    {
        [SerializeField] Transform listRoot;
        [SerializeField] AudioRowUI audioRowPrefab;
        LazyValue<AudioManager> audioManager;
 
        void Start()
        {
            audioManager = new LazyValue<AudioManager>( () => FindObjectOfType<AudioManager>() );
            audioManager.ForceInit();
            audioManager.value.onRestored += Redraw;
            Redraw();
        }   

        void Redraw()
        {
            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach(var audioPair in audioManager.value.GetAudioPair())
            {
                var rowInstance = Instantiate(audioRowPrefab, listRoot);
                rowInstance.Setup(audioManager.value, audioPair.Key, audioPair.Value);
            }
        }
    }
}

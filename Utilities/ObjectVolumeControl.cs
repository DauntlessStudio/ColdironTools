using UnityEngine;
using ColdironTools.Scriptables;

namespace ColdironTools.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class ObjectVolumeControl : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] private FloatScriptable masterVolume = null;
        [SerializeField] private FloatScriptable objectCategoryVolume = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            masterVolume.RegisterListener(UpdateVolume);
            objectCategoryVolume.RegisterListener(UpdateVolume);
        }

        private void OnEnable()
        {
            UpdateVolume();
        }

        private void UpdateVolume()
        {
            audioSource.volume = masterVolume * objectCategoryVolume;
        }

        private void OnDestroy()
        {
            masterVolume.UnregisterListener(UpdateVolume);
            objectCategoryVolume.UnregisterListener(UpdateVolume);

        }
    }
}
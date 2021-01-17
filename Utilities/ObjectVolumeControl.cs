// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using ColdironTools.Scriptables;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// Allows volume to be controlled by float scriptables.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class ObjectVolumeControl : MonoBehaviour
    {
        #region Fields
        [Tooltip("The target of the volume control.")]
        [SerializeField] private AudioSource audioSource;

        [Tooltip("The float scriptable that controls the master volume.")]
        [SerializeField] private FloatScriptableReference masterVolume = new FloatScriptableReference(1.0f);

        [Tooltip("The float scriptable that controls the volume for this category.")]
        [SerializeField] private FloatScriptableReference objectCategoryVolume = new FloatScriptableReference(1.0f);
        #endregion

        #region Methods
        /// <summary>
        /// Automatically assign an audio source.
        /// </summary>
        private void OnValidate()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        /// <summary>
        /// Registers listeners.
        /// </summary>
        private void Awake()
        {
            masterVolume.RegisterListener(UpdateVolume);
            objectCategoryVolume.RegisterListener(UpdateVolume);
        }

        /// <summary>
        /// Updates volume when the object becomes active.
        /// </summary>
        private void OnEnable()
        {
            UpdateVolume();
        }

        /// <summary>
        /// Sets the source volume.
        /// </summary>
        private void UpdateVolume()
        {
            audioSource.volume = masterVolume * objectCategoryVolume;
        }

        /// <summary>
        /// Unregisters listeners.
        /// </summary>
        private void OnDestroy()
        {
            masterVolume.UnregisterListener(UpdateVolume);
            objectCategoryVolume.UnregisterListener(UpdateVolume);

        }
        #endregion
    }
}
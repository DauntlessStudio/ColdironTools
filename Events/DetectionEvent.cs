// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColdironTools.Events
{
    /// <summary>
    /// Invokes Unity Events when objects enter or exit the Collider.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DetectionEvent : MonoBehaviour
    {
        [SerializeField, Tooltip("Scene objects to detect")] private List<GameObject> targetObjects = new List<GameObject>();
        [SerializeField, Tooltip("Layers to detect")] private LayerMask targetLayers = 0;

        [Header("")]
        [SerializeField] private UnityEvent triggerEnterEvent = new UnityEvent();
        [SerializeField] private UnityEvent triggerExitEvent = new UnityEvent();

        private Collider detectionCollider;

        /// <summary>
        /// Assigns the collider and sets it to a trigger collider.
        /// </summary>
        private void OnValidate()
        {
            detectionCollider = GetComponent<Collider>();
            detectionCollider.isTrigger = true;
        }

        /// <summary>
        /// Makes sure the collider is assigned even if OnValidate is not called.
        /// </summary>
        private void Awake()
        {
            detectionCollider = GetComponent<Collider>();
        }

        /// <summary>
        /// Called when a Collider enters the Trigger. Invokes the Trigger Enter Event.
        /// </summary>
        /// <param name="collider">The Collider that entered the Trigger</param>
        private void OnTriggerEnter(Collider collider)
        {
            if (targetObjects.Contains(collider.gameObject) || EvaluateLayer(collider))
            {
                triggerEnterEvent.Invoke();
            }
        }

        /// <summary>
        /// Called when a Collider exits the Trigger. Invokes the Trigger Exit Event.
        /// </summary>
        /// <param name="collider">The Collider that exited the Trigger</param>
        private void OnTriggerExit(Collider collider)
        {
            if (targetObjects.Contains(collider.gameObject) || EvaluateLayer(collider))
            {
                triggerExitEvent.Invoke();
            }
        }

        /// <summary>
        /// Evaluates if the colliding GameObject's layer is in targetLayers.
        /// </summary>
        /// <param name="collider">The Collider to be evaluated</param>
        /// <returns></returns>
        private bool EvaluateLayer(Collider collider)
        {
            Vector3 direction = (collider.transform.position - transform.position).normalized;

            return Physics.Raycast(transform.position, direction, 5.0f, targetLayers);
        }

        /// <summary>
        /// Adds a GameObject to the targetObjects list.
        /// </summary>
        /// <param name="gameObject">The GameObject to add</param>
        public void AddTargetObject(GameObject gameObject)
        {
            targetObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes a GameObject from the targetObjects list.
        /// </summary>
        /// <param name="gameObject">The GameObject to remove</param>
        public void RemoveTargetObjects(GameObject gameObject)
        {
            targetObjects.Remove(gameObject);
        }
    }
}
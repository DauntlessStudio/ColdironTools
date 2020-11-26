using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColdironTools.Events
{
    [RequireComponent(typeof(Collider))]
    public class DetectionEvent : MonoBehaviour
    {
        [SerializeField, Tooltip("Scene objects to detect")] private List<GameObject> targetObjects = new List<GameObject>();
        [SerializeField, Tooltip("Layers to detect")] private LayerMask targetLayers = 0;

        [Header("")]
        [SerializeField] private UnityEvent triggerEnterEvent = new UnityEvent();
        [SerializeField] private UnityEvent triggerExitEvent = new UnityEvent();

        private Collider detectionCollider;

        private void OnValidate()
        {
            detectionCollider = GetComponent<Collider>();
            detectionCollider.isTrigger = true;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (targetObjects.Contains(collider.gameObject) || EvaluateLayer(collider))
            {
                triggerEnterEvent.Invoke();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (targetObjects.Contains(collider.gameObject) || EvaluateLayer(collider))
            {
                triggerExitEvent.Invoke();
            }
        }

        bool EvaluateLayer(Collider collider)
        {
            Vector3 direction = (collider.transform.position - transform.position).normalized;

            return Physics.Raycast(transform.position, direction, 5.0f, targetLayers);
        }

        public void AddTargetObject(GameObject gameObject)
        {
            targetObjects.Add(gameObject);
        }

        public void RemoveTargetObjects(GameObject gameObject)
        {
            targetObjects.Remove(gameObject);
        }
    }
}
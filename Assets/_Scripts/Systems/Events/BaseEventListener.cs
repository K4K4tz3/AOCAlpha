using System;
using UnityEngine;
using UnityEngine.Events;

namespace EventSpace
{
    public class BaseEventListener : MonoBehaviour{
        [SerializeField] private BaseEvent m_event;

        [SerializeField] private UnityEvent m_response;
        [SerializeField] private UnityEvent<GameObject> m_responseGameObject;
        [SerializeField] private UnityEvent<Transform> m_responseTransform;

        internal void OnEnable() => m_event?.RegisterListener(this);
        internal void OnDisable() => m_event?.RemoveListener(this);
        internal void Respond() => m_response.Invoke();
    }
}
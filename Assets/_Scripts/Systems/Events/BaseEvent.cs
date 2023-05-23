using System.Collections.Generic;
using UnityEngine;

namespace EventSpace
{
    [CreateAssetMenu(fileName = "New Event", menuName = "SO/Events/Distributer")]
    public class BaseEvent : ScriptableObject{
        [SerializeField] private List<BaseEventListener> m_listeners = new List<BaseEventListener>();

        internal void RegisterListener(BaseEventListener listener) => m_listeners.Add(listener);
        internal void RemoveListener(BaseEventListener listener) => m_listeners.Remove(listener);

        internal void InvokeEvent(){
            Debug.Log($"INvoked: {this.name}");
            if(m_listeners != null && m_listeners.Count > 0)
                for(int i = m_listeners.Count - 1; i >= 0; i--)
                    m_listeners[i].Respond();
        }
    }
}
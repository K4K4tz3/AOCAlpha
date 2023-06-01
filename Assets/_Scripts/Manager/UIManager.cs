using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Button m_serverBtn;
        [SerializeField] private Button m_hostBtn;
        [SerializeField] private Button m_clientBtn;



        protected override void Awake(){
            base.Awake();
            m_serverBtn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
            m_hostBtn.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
            m_clientBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        }
    }
}
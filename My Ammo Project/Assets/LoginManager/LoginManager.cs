using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public void OnServerButtionClick()
    {

        NetworkManager.Singleton.StartServer();
    }

    public async void OnHostButtionClick()
    {
        if (RelayManagerScript.Instance.IsRelayEnabled)
        {
            Debug.Log("In If RelaManaget");
            await RelayManagerScript.Instance.CreateRelay();
        }
        NetworkManager.Singleton.StartHost();
    }
    public TMP_InputField joinCodeInputField;
    public string joinCode;
    public async void OnClientButtionClick()
    {
        joinCode = joinCodeInputField.GetComponent<TMP_InputField>().text;
        if (RelayManagerScript.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCode))
        {
            await RelayManagerScript.Instance.JoinRelay(joinCode);
        }
        NetworkManager.Singleton.StartClient();
    }

}

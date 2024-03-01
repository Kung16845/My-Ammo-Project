using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LoginManager : NetworkBehaviour
{
    public void OnServerButtionClick()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void OnHostButtionClick()
    {
        NetworkManager.Singleton.StartHost();
    }
    
    public void OnClientButtionClick()
    {
        NetworkManager.Singleton.StartClient();
    }

}

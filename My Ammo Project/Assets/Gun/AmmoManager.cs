using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class AmmoManager : NetworkBehaviour
{
    // Dictionary to store current ammo counts for each player
    public TextMeshProUGUI ammoTextMeshPro;
    private Dictionary<ulong, PlayerAmmoData> playerAmmoData = new Dictionary<ulong, PlayerAmmoData>();
    [SerializeField]
    private GameObject ammoDisplayPrefab;
    
    private void Start()
    {
        // Subscribe to player join and leave events
        if (IsServer)
        {
            SpawnAmmoDisplays();
        }
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }
    private void Update()
    {
    }
    private void OnDestroy()
    {
        // Unsubscribe from player join and leave events
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }

    // Handle when a client connects to the server
    private void HandleClientConnected(ulong clientId)
    {
        // Initialize the ammo data for the newly connected client
        playerAmmoData.Add(clientId, new PlayerAmmoData());
    }

    // Handle when a client disconnects from the server
    private void HandleClientDisconnected(ulong clientId)
    {
        // Remove the disconnected client's ammo data
        playerAmmoData.Remove(clientId);
    }

    // Update the ammo data for a specific player
    public void UpdateAmmoData(ulong clientId, WeaponType weaponType, int ammoCount)
    {
        if (playerAmmoData.ContainsKey(clientId))
        {
            // Update the corresponding ammo count based on weapon type
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    playerAmmoData[clientId].currentAmmoPistol = ammoCount;
                    break;
                case WeaponType.Shotgun:
                    playerAmmoData[clientId].currentAmmoShotgun = ammoCount;
                    break;
                case WeaponType.AssaultRifle:
                    playerAmmoData[clientId].currentAmmoAssaultRifle = ammoCount;
                    break;
            }
        }

        // Update the TextMeshPro with the current ammo data
        UpdateTextMeshPro();
    }
    private void UpdateTextMeshPro()
    {
        string ammoText = "Ammo: ";
        // Append the current ammo counts for each player
        foreach (var playerAmmo in playerAmmoData)
        {
            ammoText += $"Player {playerAmmo.Key}: Pistol - {playerAmmo.Value.currentAmmoPistol}, Shotgun - {playerAmmo.Value.currentAmmoShotgun}, AssaultRifle - {playerAmmo.Value.currentAmmoAssaultRifle}\n";
        }
        // Update the TextMeshPro text
        ammoTextMeshPro.text = ammoText;
    }

    // Get the current ammo data for all players
    public Dictionary<ulong, PlayerAmmoData> GetCurrentAmmoData()
    {
        return playerAmmoData;
    }
       private void SpawnAmmoDisplays()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            GameObject ammoDisplayObject = Instantiate(ammoDisplayPrefab);
            NetworkObject networkObject = ammoDisplayObject.GetComponent<NetworkObject>();
            
            if (networkObject != null && networkObject.IsSpawned)
            {
                AmmoDisplay ammoDisplay = ammoDisplayObject.GetComponent<AmmoDisplay>();
                
                if (ammoDisplay != null)
                {
                    StartCoroutine(UpdateAmmoDisplayRoutine(clientId, ammoDisplay));
                }
            }
        }
    }

    private IEnumerator UpdateAmmoDisplayRoutine(ulong clientId, AmmoDisplay ammoDisplay)
    {
        while (ammoDisplay == null || !ammoDisplay.GetComponent<NetworkObject>().IsSpawned)
        {
            yield return null;
        }

        UpdateAmmoDisplay(clientId, ammoDisplay);
    }

    private void UpdateAmmoDisplay(ulong clientId, AmmoDisplay ammoDisplay)
    {
        string ammoText = GenerateAmmoText(clientId);
        ammoDisplay.UpdateAmmoDisplayClientRpc(ammoText);
    }

    private string GenerateAmmoText(ulong clientId)
    {
        if (playerAmmoData.ContainsKey(clientId))
        {
            PlayerAmmoData ammoData = playerAmmoData[clientId];
            return $"Ammo - Player {clientId}: Pistol - {ammoData.currentAmmoPistol}, Shotgun - {ammoData.currentAmmoShotgun}, AssaultRifle - {ammoData.currentAmmoAssaultRifle}";
        }
        return string.Empty;
    }
}

// Class to store ammo data for each player
public class PlayerAmmoData
{
    public int currentAmmoPistol;
    public int currentAmmoShotgun;
    public int currentAmmoAssaultRifle;
}

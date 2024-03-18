using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class AmmoManager : NetworkBehaviour
{
    public GameObject playerPanelPrefab;
    public GameObject mainPanel; // Reference to the main panel in the scene where player panels will be added
    private Dictionary<ulong, PlayerAmmoData> playerAmmoData = new Dictionary<ulong, PlayerAmmoData>();
    private Dictionary<ulong, GameObject> playerPanels = new Dictionary<ulong, GameObject>();

    private void Start()
    {
        // Subscribe to player join and leave events
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }

    // Handle when a client connects to the server
    private void HandleClientConnected(ulong clientId)
{
    // Initialize the ammo data for the newly connected client
    playerAmmoData.Add(clientId, new PlayerAmmoData());

    // Send the existing UI state to the newly connected client
    if (NetworkManager.Singleton.IsServer)
    {
        foreach (var playerId in playerPanels.Keys)
        {
            if (playerId != clientId)
            {
                PlayerAmmoData playerData = playerAmmoData[playerId];
                SendUIStateToClient(clientId, playerData);
            }
        }
    }

    // Instantiate the player panel prefab for the new player
    GameObject playerPanel = Instantiate(playerPanelPrefab, Vector3.zero, Quaternion.identity);
    
    // Set the canvas as the parent of the player panel
    playerPanel.transform.SetParent(mainPanel.transform, false);

    // Add the player panel to the dictionary for future reference
    playerPanels.Add(clientId, playerPanel);

    // Update the TextMeshPro with the current ammo data for all players
    UpdateTextMeshPro();
}

private void SendUIStateToClient(ulong clientId, PlayerAmmoData playerData)
{
    // Update the client's UI with the UI state of the specified player
    UpdateAmmoData(clientId, WeaponType.Pistol, playerData.currentAmmoPistol, playerData.currentReserveAmmoPistol);
    UpdateAmmoData(clientId, WeaponType.Shotgun, playerData.currentAmmoShotgun, playerData.currentReserveAmmoShotgun);
    UpdateAmmoData(clientId, WeaponType.AssaultRifle, playerData.currentAmmoAssaultRifle, playerData.currentReserveAmmoAssaultRifle);
}

    // Handle when a client disconnects from the server
    private void HandleClientDisconnected(ulong clientId)
    {
        // Remove the disconnected client's ammo data
        playerAmmoData.Remove(clientId);

        // Destroy the player panel for the disconnected player
        if (playerPanels.ContainsKey(clientId))
        {
            Destroy(playerPanels[clientId]);
            playerPanels.Remove(clientId);
        }

        // Update the TextMeshPro with the current ammo data
        UpdateTextMeshPro();
    }

    // Update the ammo data for a specific player
    public void UpdateAmmoData(ulong clientId, WeaponType weaponType, int ammoCount, int reserveAmmoCount)
    {
        if (playerAmmoData.ContainsKey(clientId))
        {
            // Update the corresponding ammo count based on weapon type
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    playerAmmoData[clientId].currentAmmoPistol = ammoCount;
                    playerAmmoData[clientId].currentReserveAmmoPistol = reserveAmmoCount;
                    break;
                case WeaponType.Shotgun:
                    playerAmmoData[clientId].currentAmmoShotgun = ammoCount;
                    playerAmmoData[clientId].currentReserveAmmoShotgun = reserveAmmoCount;
                    break;
                case WeaponType.AssaultRifle:
                    playerAmmoData[clientId].currentAmmoAssaultRifle = ammoCount;
                    playerAmmoData[clientId].currentReserveAmmoAssaultRifle = reserveAmmoCount;
                    break;
            }
        }

        // Update the TextMeshPro with the current ammo data
        UpdateTextMeshPro();
    }

    // Update the TextMeshPro UI object with the current ammo data
    private void UpdateTextMeshPro()
    {
        string ammoText = "Ammo: ";
        // Append the current ammo counts for each player
        foreach (var playerAmmo in playerAmmoData)
        {
            ammoText += $"Player {playerAmmo.Key}: Pistol - {playerAmmo.Value.currentAmmoPistol}/{playerAmmo.Value.currentReserveAmmoPistol}, " +
                        $"Shotgun - {playerAmmo.Value.currentAmmoShotgun}/{playerAmmo.Value.currentReserveAmmoShotgun}, " +
                        $"AssaultRifle - {playerAmmo.Value.currentAmmoAssaultRifle}/{playerAmmo.Value.currentReserveAmmoAssaultRifle}\n";
        }

        // Update the TextMeshPro text on each player panel
        foreach (var playerPanel in playerPanels.Values)
        {
            TextMeshProUGUI textMeshPro = playerPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro != null)
            {
                textMeshPro.text = ammoText;
            }
        }
    }
}

// Class to store ammo data for each player
public class PlayerAmmoData
{
    public int currentAmmoPistol;
    public int currentAmmoShotgun;
    public int currentAmmoAssaultRifle;
    
    // Reserve ammo data
    public int currentReserveAmmoPistol;
    public int currentReserveAmmoAssaultRifle;
    public int currentReserveAmmoShotgun;
}

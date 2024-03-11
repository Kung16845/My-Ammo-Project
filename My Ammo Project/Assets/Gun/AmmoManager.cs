using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class AmmoManager : NetworkBehaviour
{
    public GameObject playerPanelPrefab;
    // Dictionary to store current ammo counts for each player
    public Canvas canvas;
    private Dictionary<ulong, PlayerAmmoData> playerAmmoData = new Dictionary<ulong, PlayerAmmoData>();
    private Dictionary<ulong, GameObject> playerPanels = new Dictionary<ulong, GameObject>();

    private void Start()
    {
        // Subscribe to player join and leave events
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

        // Check if the current instance is the server (host)
        if (NetworkManager.Singleton.IsServer)
        {
            // Handle host spawn (you can customize this part as needed)
            HandleHostSpawn();
        }
    }
    private void HandleHostSpawn()
    {
        // Instantiate the player panel prefab for the host
        GameObject hostPanel = Instantiate(playerPanelPrefab, Vector3.zero, Quaternion.identity);

        // Set Canvas as the parent
        hostPanel.transform.SetParent(canvas.transform, false);

        // Get the RectTransform component
        RectTransform rectTransform = hostPanel.GetComponent<RectTransform>();

        // Adjust the anchored position for middle-left (e.g., x = 50f, y = 0f)
        rectTransform.anchoredPosition = new Vector2(50f, 0f); // Adjust as needed

        // Set the anchor to the middle-left (0, 0.5)
        rectTransform.anchorMin = new Vector2(0f, 0.5f);
        rectTransform.anchorMax = new Vector2(0f, 0.5f);

        // Add the host panel to the dictionary for future reference
        playerPanels.Add(NetworkManager.Singleton.LocalClientId, hostPanel);

        // Update the TextMeshPro with the current ammo data
        UpdateTextMeshPro();
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

    // Update the TextMeshPro UI object with the current ammo data
    private void UpdateTextMeshPro()
    {
        string ammoText = "Ammo: ";
        // Append the current ammo counts for each player
        foreach (var playerAmmo in playerAmmoData)
        {
            ammoText += $"Player {playerAmmo.Key}: Pistol - {playerAmmo.Value.currentAmmoPistol}, Shotgun - {playerAmmo.Value.currentAmmoShotgun}, AssaultRifle - {playerAmmo.Value.currentAmmoAssaultRifle}\n";
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
        private void HandleClientConnected(ulong clientId)
    {
        // Initialize the ammo data for the newly connected client
        playerAmmoData.Add(clientId, new PlayerAmmoData());

        // Instantiate the player panel prefab for the new player
        GameObject playerPanel = Instantiate(playerPanelPrefab, Vector3.zero, Quaternion.identity);
        
        // Set the canvas as the parent of the player panel
        playerPanel.transform.SetParent(canvas.transform, false);

        // Add the player panel to the dictionary for future reference
        playerPanels.Add(clientId, playerPanel);

        // Update the TextMeshPro with the current ammo data
        UpdateTextMeshPro();
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
}

// Class to store ammo data for each player
public class PlayerAmmoData
{
    public int currentAmmoPistol;
    public int currentAmmoShotgun;
    public int currentAmmoAssaultRifle;
}

using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class AmmoDisplay : NetworkBehaviour
{
    private TextMeshProUGUI ammoTextMeshPro;

    private void Start()
    {
        // Find the TextMeshProUGUI component in the hierarchy
        ammoTextMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (ammoTextMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in the AmmoDisplay hierarchy.");
        }
    }

    [ClientRpc]
    public void UpdateAmmoDisplayClientRpc(string ammoText)
    {
        if (ammoTextMeshPro != null)
        {
            ammoTextMeshPro.text = ammoText;
        }
    }
}

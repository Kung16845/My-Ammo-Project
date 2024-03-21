using UnityEngine;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{
    public Transform playerTransform; // ตัวแปรสำหรับตั้งค่า Transform ของผู้เล่น
    private void Awake()
    {
        // Check if there is already an audio listener in the scene
        if (FindObjectsOfType<AudioListener>().Length > 1)
        {
            // If there is more than one, disable the audio listener of this object
            GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            // Ensure that this object has the audio listener enabled
            GetComponent<AudioListener>().enabled = true;
        }
    }
    private void Update()
    {
        if (IsOwner) // ตรวจสอบว่า Instance นี้เป็นของผู้เล่นนี้หรือไม่
        {
            if (playerTransform != null)
            {
                // ปรับตำแหน่งกล้องให้ตามผู้เล่น โดยรักษาความสูงของกล้องเดิม
                transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            }
        }
        else 
        {
            this.gameObject.SetActive(false);
            
        }
    }
    
}

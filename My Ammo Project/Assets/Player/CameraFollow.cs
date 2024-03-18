using UnityEngine;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{
    public Transform playerTransform; // ตัวแปรสำหรับตั้งค่า Transform ของผู้เล่น

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

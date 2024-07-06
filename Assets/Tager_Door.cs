using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Tager_Door : NetworkBehaviour
{
    public GameObject pickupButton; // Tham chiếu đến nút nhặt
    private Animator animator;
    private bool isOpen; // trạng thái của cửa
    private bool isPlayerInRange = false; // vùng hiển thị button

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from this GameObject");
        }
        else
        {
            animator.SetBool("SateD", true);
        }
        isOpen = false;
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }
        else
        {
            Debug.LogError("PickupButton reference is missing");
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (IsClient)
            {
                Debug.Log("Nhận được nút E");
                ToggleDoor();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupButton.SetActive(true);
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupButton.SetActive(false);
            isPlayerInRange = false;
        }
    }

    // Hàm này sẽ được gọi bởi script của người chơi
    public void ToggleDoor()
    {
        ToggleDoorServerRpc();
    }

    // Hàm này sẽ được gọi để cập nhật trạng thái của cửa từ ServerRpc
    private void UpdateDoorState()
    {
        if (isOpen)
        {
            // Nếu cửa đang mở, đóng cửa
            animator.SetBool("closeD", true);
            OnCloseDAnimationEnd();
            animator.SetBool("OpenD", false);
            Debug.Log("Closing door");
        }
        else
        {
            // Nếu cửa đang đóng, mở cửa
            animator.SetBool("OpenD", true);
            animator.SetBool("closeD", false);
            Debug.Log("Opening door");
        }
        isOpen = !isOpen; // Chuyển trạng thái của cửa
    }

    public void OnCloseDAnimationEnd()
    {
        animator.SetBool("SateD", true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleDoorServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            var playerNetworkObject = client.PlayerObject;
            if (playerNetworkObject != null)
            {
                UpdateDoorState(); // Cập nhật trạng thái cửa trên máy chủ

                // Gửi cập nhật trạng thái cửa đến tất cả các client
                UpdateDoorStateClientRpc(isOpen);
            }
        }
    }

    [ClientRpc]
    void UpdateDoorStateClientRpc(bool newState, ClientRpcParams rpcParams = default)
    {
        isOpen = newState;
        UpdateDoorState(); // Cập nhật trạng thái cửa trên tất cả các client
    }
}

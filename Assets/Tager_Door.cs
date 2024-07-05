using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tager_Door : MonoBehaviour
{
    private Animator animator;
    private bool isOpen; // trạng thái của cửa

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SateD", true);
        isOpen = false;
    }

    // Hàm này sẽ được gọi bởi script của người chơi
    public void ToggleDoor()
    {
        if (isOpen)
        {
            // Nếu cửa đang mở, đóng cửa
            animator.SetBool("closeD",true);
            OnCloseDAnimationEnd();
            animator.SetBool("OpenD", false);
        }
        else
        {
            // Nếu cửa đang đóng, mở cửa
            animator.SetBool("OpenD", true);
            animator.SetBool("closeD", false);

        }
        isOpen = !isOpen; // Chuyển trạng thái của cửa
    }
    public void OnCloseDAnimationEnd()
    {
        animator.SetBool("SateD", true); 
    }
}

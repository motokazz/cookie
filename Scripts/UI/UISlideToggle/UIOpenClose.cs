using UnityEngine;

public class UIOpenClose : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;

    void Awake() => animator = GetComponent<Animator>();

    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetTrigger(isOpen ? "Open" : "Close");
    }
}

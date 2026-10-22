using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickTrigger : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onClick;
    /*Animator animator;*/
    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (animator.GetBool("IsOpen") == true)
        {
            animator.SetBool("IsOpen", false);
        }
        else
        {
            animator.SetBool("IsOpen", true);
        }*/
        onClick?.Invoke();

    }
    /*public void Start()
    {
        animator = GetComponent<Animator>();
    }*/
}
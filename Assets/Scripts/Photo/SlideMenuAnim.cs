using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMenuAnim : MonoBehaviour
{
    public GameObject ClotheMenu;

    public void ShowMenu()
    {
        Animator animator = ClotheMenu.GetComponent<Animator>();
        if(animator != null)
        {
            bool isOpen = animator.GetBool("show");
            animator.SetBool("show", true);
        }
    }

    public void HideMenu()
    {
        Animator animator = ClotheMenu.GetComponent<Animator>();
        if(animator != null)
        {
            bool isOpen = animator.GetBool("show");
            animator.SetBool("show", false);
        }
    }
}

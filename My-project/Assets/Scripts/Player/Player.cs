using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator animator;

    void Start()
    {
        //mengambil informasi dari Script PlayerMovement
        playerMovement = GetComponent<PlayerMovement>();

        //mengambil informasi dari Animator dari EngineEffect
        animator = GameObject.Find("EngineEffect")?.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //memanggil method Move dari PlayerMovement
        playerMovement.Move();
    }

    void LateUpdate()
    {
        //mengatur nilai Bool dari parameter IsMoving milik Animator sesuai dengan status gerak
        //dilihat dari return IsMoving
        if (animator != null)
        {
            animator.SetBool("IsMoving", playerMovement.IsMoving());
        }
    }
}

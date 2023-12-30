using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    Animator anim;

    [SerializeField] Sprite jumpSprite;
    SpriteRenderer spriteRenderer;
    PlayerManager _playerManagerScript;

    int playRunAnim = Animator.StringToHash("canPlayRunAnim");
    int isGrounded = Animator.StringToHash("isGrounded");
    int jumpBool = Animator.StringToHash("jump");

    void Start()
    {
        anim = GetComponent<Animator>();
        _playerManagerScript = GetComponentInParent<PlayerManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ManageRunAnimation();
        ManageJumpAnimation();

        anim.SetBool(isGrounded, _playerManagerScript.isGrounded);
    }

    void ManageRunAnimation()
    {
        if(Input.GetAxis("Horizontal") != 0 && _playerManagerScript.isGrounded)
        {
            anim.SetBool(playRunAnim, true);
        }
        else
        {
            anim.SetBool(playRunAnim, false);
        }
    }

    void ManageJumpAnimation()
    {
        if(Input.GetButtonDown("Jump"))
        {
            anim.SetBool(jumpBool, true);
        }
        if(Input.GetButtonUp("Jump"))
        {
            anim.SetBool(jumpBool, false);
        }
    }    
}

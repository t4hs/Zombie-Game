using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    public float jumpHeight;
    public float gravity;
    Vector3 velocity;
    bool isJumping;
    Animator animator;
    CharacterController cc;
    characterAiming characterAiming;
    Vector2 input;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        characterAiming = GetComponent<characterAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        animateMovement();
        updateIsSprinting();     
        jump();
        
    }
    //apply gravity to the character
    private void FixedUpdate(){
        if(isJumping){
            velocity.y -= gravity * Time.fixedDeltaTime;
            cc.Move(velocity * Time.fixedDeltaTime);
            isJumping = !cc.isGrounded;
            animator.SetBool("isJumping", isJumping);
        }
    }
    //apply motion through blend tree
    private void animateMovement(){
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("inputX", input.x);
        animator.SetFloat("inputY", input.y);
    }
    //make character jump
    private void jump()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(!isJumping){
                isJumping = true;
                velocity.y = MathF.Sqrt(2* gravity * jumpHeight);
            }
        }
    }
    //make character sprint
    private void updateIsSprinting()
    {
        if(characterAiming.aimLayer.weight == 0)animator.SetBool("isSprinting", Input.GetKey(KeyCode.LeftShift));
        else{animator.SetBool("isSprinting", false);}
    }
}

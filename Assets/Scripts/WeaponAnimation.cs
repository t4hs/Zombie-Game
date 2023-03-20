using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public Transform leftHand;
    public weaponRaycast weapon;
    GameObject clipInHand;
    void Start()
    {
        weapon = GetComponentInChildren<weaponRaycast>();
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
    }
    void Update()
    {
        startReloading();
    }
    private void startReloading(){
        //check user wants to reload, there is ammo remaining and clip is not already full
        if(Input.GetKeyDown(KeyCode.R)&weapon.weaponManager.extraAmmo>0&weapon.weaponManager.currentAmmo!=weapon.weaponManager.clipSize){
            weapon.weaponManager.reloading = true;
            rigController.SetTrigger("Reload");
        }
    }
    
    private void OnAnimationEvent(string eventName){
        switch(eventName){
            case "detachClip":
            detachClip();
            break;
            case "dropClip":
            dropClip();
            break;
            case "getNewClip":
            getNewClip();
            break;
            case "attachClip":
            attachClip();
            break;

        }
    }
    private void attachClip()
    {
        weapon.clip.SetActive(true);
        Destroy(clipInHand);
        weapon.weaponManager.Reload();
        weapon.weaponManager.reloading = false;
    }
    private void getNewClip()
    {
        clipInHand.SetActive(true);
    }
    private void dropClip()
    {
        GameObject droppedClip = Instantiate(clipInHand, clipInHand.transform.position, clipInHand.transform.rotation);
        droppedClip.AddComponent<Rigidbody>();
        droppedClip.AddComponent<BoxCollider>();
        clipInHand.SetActive(false);
    }
    private void detachClip()
    {
        clipInHand = Instantiate(weapon.clip, leftHand, true);
        weapon.clip.SetActive(false);
    }
}

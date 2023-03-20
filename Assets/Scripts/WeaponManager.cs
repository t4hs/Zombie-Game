using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public characterAiming characterAiming;
    public float recoil;
    public float recoilDelay;
    private float time;
    public int clipSize;
    public int extraAmmo;
    public int currentAmmo;
    public bool reloading = false;
    public TMPro.TMP_Text ammoText;
    // Update is called once per frame
    void Update()
    {
        recoilWeapon();
        //display how much ammo is remaining in the clip
        ammoText.text = currentAmmo.ToString();
    }
    public void generateRecoil(){
        time = recoilDelay;
    }
    public void Reload(){
        if (extraAmmo>= clipSize){
            int ammoToReload = clipSize - currentAmmo;
            extraAmmo -= ammoToReload;
            currentAmmo += ammoToReload;
        }
        else if (extraAmmo>0){
            if(extraAmmo + currentAmmo > clipSize){
                int leftOverAmmo = extraAmmo+currentAmmo-clipSize;
                extraAmmo = leftOverAmmo;
                currentAmmo = clipSize;
            }
            else {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
    }
    private void recoilWeapon(){
        if(time>0){
            characterAiming.yAxis.Value -= ((recoil/5) * Time.deltaTime)/recoilDelay;
            time -= Time.deltaTime;
        }
    }
}
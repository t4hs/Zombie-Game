using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class characterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float aimTime = 0.2f;
    public Rig aimLayer;
    public Transform cameraFocus;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    Camera mainCamera;
    weaponRaycast weapon;
    bool weaponUp;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<weaponRaycast>();
        weapon.weaponManager.currentAmmo = weapon.weaponManager.clipSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        adjustCamera();
    }
    private void Update(){
        adjustWeapon();
        shoot();
    }
    //rotate camera with mouse
    private void adjustCamera(){
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);
        cameraFocus.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        float cameraRotationY = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cameraRotationY, 0), turnSpeed * Time.fixedDeltaTime);
    }
    //raise or lower weapon so character can shoot
    private void adjustWeapon(){
        if(Input.GetMouseButtonDown(1))weaponUp = !weaponUp;
        if(!weaponUp||weapon.weaponManager.reloading){
            aimLayer.weight -= Time.deltaTime / aimTime;
        }else{
            aimLayer.weight += Time.deltaTime / aimTime;
        }
    }
    //fire weapon
    private void shoot(){
        //check gun is raised and user is firing
        if(Input.GetButtonDown("Fire1")&aimLayer.weight == 1&weapon.weaponManager.currentAmmo>0){
            weapon.StartFiring();
        }
        if(weapon.isFiring){
            weapon.UpdateFiring(Time.deltaTime);
        }
        weapon.UpdatBullets(Time.deltaTime);
        //check gun is lowered or user has stopped firing
        if(Input.GetButtonUp("Fire1")||aimLayer.weight != 1||weapon.weaponManager.currentAmmo <=0){
            weapon.StopFiring();
        }
    }
}

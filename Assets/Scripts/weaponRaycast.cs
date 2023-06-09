using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponRaycast : MonoBehaviour
{
    class Bullet {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    public bool isFiring = false;
    public int fireRate = 15;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public WeaponManager weaponManager;
    public GameObject clip;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifetime = 3.0f;

    private void Awake(){
        weaponManager = GetComponent<WeaponManager>();
    }
    //return position of a bullet
    Vector3 GetPosition(Bullet bullet){
        //p+vt+1/2gt^2
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity){
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
        weaponManager.currentAmmo--;
    }
    //keep shooting while there is time left
    public void UpdateFiring(float deltaTime){
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= 0.0f){
            FireBullet();
            weaponManager.currentAmmo--;
            accumulatedTime -= fireInterval;
        }
    }
    
    public void UpdatBullets(float  deltaTime){
        SimulateBullets(deltaTime);
        DestroyBullets();
    }
    
    void DestroyBullets(){
        bullets.RemoveAll(bullet => bullet.time >= maxLifetime);
    }
    //perform raycast along the distance a bullet has travelled
    void SimulateBullets(float deltaTime){
        bullets.ForEach(bullet => {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    //perform raycasts
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet){
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);

                bullet.tracer.transform.position = hitInfo.point;
                bullet.time = maxLifetime;
            }else{
                bullet.tracer.transform.position = end;
            }
    }

    private void FireBullet()
    {
        muzzleFlash.Emit(1);
        //create a bullet moving towards a target
        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        weaponManager.generateRecoil();
    }

    public void StopFiring(){
        isFiring = false;
    }
}

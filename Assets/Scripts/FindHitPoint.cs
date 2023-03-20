using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHitPoint : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //cast ray into the screen under the crosshair
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        //return if raycast hits something
        if (Physics.Raycast(ray, out hitInfo)) {
            transform.position = hitInfo.point;
        } else {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject Flashlight_light;
    private bool FlashlightActive = false;
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab
    [SerializeField] private Transform firePoint; // Reference to the fire point transform
    [SerializeField] private float projectileSpeed = 10f;
    void Start()
    {
        Flashlight_light.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FlashlightActive == false)
            {
                Flashlight_light.gameObject.SetActive(true);
                Shoot();
                FlashlightActive = true;
            }
            else
            {
                Flashlight_light.gameObject.SetActive(false);
                FlashlightActive = false;
            }
        }
    }
    private void Shoot()
    {
        
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
             rb.velocity = firePoint.forward * projectileSpeed;
        rb.AddForce(firePoint.forward * projectileSpeed, ForceMode.VelocityChange);




        Destroy(projectile, 2f);
        }
    }


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Grenade Settings")]
	public float grenadeSpawnDelay = 0.35f;
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 35f;
    [SerializeField] float dayum = 100f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;
    public int shots = 0;
    public int clip = 0;
    public int mag = 0;
    public GameObject enemHP;

    bool canShoot = true;

    private void OnEnable() 
    {
        canShoot = true;
        mag = ammoSlot.GetCurrentAmmo(ammoType);
        clip = 0;
    }

    [System.Serializable]
	public class spawnpoints
	{  
		[Header("Spawnpoints")]
		public Transform grenadeSpawnPoint;
	}
	public spawnpoints Spawnpoints;

    [System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform grenadePrefab;
	}
	public prefabs Prefabs;

    void Update()
    {
        InterEnemy();
        DisplayAmmo();
        if(Input.GetMouseButtonDown(0) && canShoot == true)
        {
            StartCoroutine(Shoot());
        }
        if (Input.GetMouseButtonDown(1) && canShoot == true)
        {
            StartCoroutine(Shoot1());
        }
    }
    public void addAmmo(int value)
    {
        mag += value;
    }
    private void DisplayAmmo()
    {
        if(clip == 0)
        {
            if(mag >= 20)
            {
                mag -= 20;
                clip = 20;
            }
            else
            {
                clip += mag;
                mag = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            int toReload = 20 - clip;
            if (mag >= toReload)
            {
                mag -= toReload;
                clip += toReload;
            }
            else
            {
                mag = 0;
                clip += mag;
            }
        }

        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType) % 21;
        
        //int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = clip.ToString() + '/' + mag.ToString();
    }

    IEnumerator Shoot1()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast1();
            ammoSlot.ReduceCurrentAmmo(ammoType);
            UnityEngine.Debug.Log(ammoSlot.GetCurrentAmmo(ammoType));
            clip--;
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }
    IEnumerator Shoot()
    {  
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
            UnityEngine.Debug.Log(ammoSlot.GetCurrentAmmo(ammoType));
            clip--;
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent
                    <EnemyHealth>().TakeDamage(damage);
            }            
            if (hit.transform.tag == "ExplosiveBarrel")
            {
                hit.transform.gameObject.GetComponent
                    <ExplosiveBarrelScript>().explode = true;
            }
        }
        else
        {
            return;
        }
    }
    
    private void ProcessRaycast1()
    {
        RaycastHit hit;
        if(Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent
                    <EnemyHealth>().TakeDamage(dayum);
            }            
            if (hit.transform.tag == "ExplosiveBarrel")
            {
                hit.transform.gameObject.GetComponent
                    <ExplosiveBarrelScript>().explode = true;
            }
        }
        else
        {
            return;
        }
    }private void InterEnemy()
    {
        RaycastHit hit;
        if(Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, 1000))
        {
            if (hit.transform.tag == "Enemy")
            {
                UnityEngine.Debug.Log("enemy");
                enemHP.SetActive(true);
            }
            else
            {
                enemHP.SetActive(false);
            }
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, .1f);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponData currentWeapon;
    public WeaponData[] availableWeapons;
    public int currentWeaponIndex = 0;

    [Header("Pooling")]
    public ObjectPool projectilePool;

    [Header("References")]
    public Camera playerCamera;
    public Transform firePoint;

    private float fireTimer;

    void Start()
    {
        if (availableWeapons != null && availableWeapons.Length > 0)
        {
            EquipWeapon(availableWeapons[currentWeaponIndex]);
        }
    }

    void Update()
    {
        if (currentWeapon == null)
            return;

        HandleWeaponSwitching();
        HandleFireCooldown();
        HandleInput();
    }

    void HandleWeaponSwitching()
    {
        if (availableWeapons == null || availableWeapons.Length == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && availableWeapons.Length >= 1)
        {
            currentWeaponIndex = 0;
            EquipWeapon(availableWeapons[currentWeaponIndex]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && availableWeapons.Length >= 2)
        {
            currentWeaponIndex = 1;
            EquipWeapon(availableWeapons[currentWeaponIndex]);
        }
    }

    void HandleFireCooldown()
    {
        fireTimer += Time.deltaTime;
    }

    void HandleInput()
    {
        bool wantsToShoot = false;

        switch (currentWeapon.fireMode)
        {
            case WeaponFireMode.SemiAuto:
                wantsToShoot = Input.GetMouseButtonDown(0);
                break;

            case WeaponFireMode.FullAuto:
                wantsToShoot = Input.GetMouseButton(0);
                break;
        }

        if (wantsToShoot && fireTimer >= currentWeapon.fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        if (currentWeapon.projectilePrefab == null || firePoint == null || playerCamera == null)
            return;

        Vector3 flatDirection = playerCamera.transform.forward;
        flatDirection.y = 0f;
        flatDirection.Normalize();

        Quaternion flatRotation = Quaternion.LookRotation(flatDirection);

        GameObject projectile;

        if (projectilePool != null)
        {
            projectile = projectilePool.GetObject(firePoint.position, flatRotation);
        }
        else
        {
            projectile = Instantiate(
                currentWeapon.projectilePrefab,
                firePoint.position,
                flatRotation
            );
        }

        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.damage = currentWeapon.damage;
            projectileScript.speed = currentWeapon.projectileSpeed;
            projectileScript.owner = gameObject;
        }

        SpawnMuzzleFlash();
        PlayFireSound();

        Debug.Log("Fired weapon: " + currentWeapon.weaponName);
    }

    void SpawnMuzzleFlash()
    {
        if (currentWeapon.muzzleFlashPrefab == null)
            return;

        GameObject flash = Instantiate(
            currentWeapon.muzzleFlashPrefab,
            firePoint.position,
            firePoint.rotation
        );

        flash.transform.SetParent(firePoint);
        Destroy(flash, 0.1f);
    }

    void PlayFireSound()
    {
        if (currentWeapon.fireSound == null)
            return;

        AudioSource.PlayClipAtPoint(currentWeapon.fireSound, transform.position);
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        fireTimer = currentWeapon.fireRate;

        Debug.Log("Equipped weapon: " + currentWeapon.weaponName);
    }
}
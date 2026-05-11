using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponFireMode
{
    SemiAuto,
    FullAuto
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Combat")]
    public int damage = 1;
    public float fireRate = 0.2f;
    public float projectileSpeed = 20f;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Firing")]
    public WeaponFireMode fireMode = WeaponFireMode.FullAuto;

    [Header("Visuals")]
    public GameObject muzzleFlashPrefab;

    [Header("Audio")]
    public AudioClip fireSound;
}

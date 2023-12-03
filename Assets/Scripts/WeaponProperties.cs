using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponProperties : ScriptableObject
{
    //Defines basic properties for a weapon, used in Weapon.cs
    public bool isKnife;
    public float mag = 12f;
    public float stock = 36f;
    public bool canSpray;
    public float cooldown = 0.108f;
    public float bloom = 0.11f;
    public float recoilDistance = 0.1f;
    public bool thrown = false;
    public float launchVelocity = 700f;
}

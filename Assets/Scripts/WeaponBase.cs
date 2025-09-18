using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float damage;

    public abstract System.Collections.IEnumerator AttackRoutine();

}
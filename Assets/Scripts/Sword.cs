using System.Collections;
using UnityEngine;

public class Sword : WeaponBase
{
    private static WaitForSeconds delay = new WaitForSeconds(0.1f);

    public float slashAngle = -90f;
    public float duration = 0.2f;

    private Transform weaponHolder;

    void Start()
    {
        weaponHolder = GetComponentInParent<WeaponController>().transform;
    }

    public override IEnumerator AttackRoutine()
    {
        weaponHolder.GetLocalPositionAndRotation(out var startPosition, out var startRotation);
        var endRotation = startRotation * Quaternion.Euler(0, slashAngle, 0);
        var elapsed = 0f;

        // forward swing
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.SmoothStep(0, 1, elapsed / duration);
            weaponHolder.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        // reset
        weaponHolder.SetLocalPositionAndRotation(startPosition, startRotation);

        // optional small delay before next attack
        yield return delay;
    }

}
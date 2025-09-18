using System.Collections;
using UnityEngine;

public class Jab : WeaponBase
{
    public Transform weaponHolder;
    public float jabDistance = 1f;
    public float duration = 0.3f;

    public override IEnumerator AttackRoutine()
    {
        Vector3 start = weaponHolder.localPosition;
        Vector3 end = start + Vector3.forward * jabDistance;
        float elapsed = 0f;

        // thrust forward
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            weaponHolder.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        // pull back
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            weaponHolder.localPosition = Vector3.Lerp(end, start, t);
            yield return null;
        }

        // optional delay
        yield return new WaitForSeconds(0.05f);
    }
}
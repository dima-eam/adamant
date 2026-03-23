
using UnityEngine;
public class GiantSword : Weapon
{
	public GiantSword() : base("Giant Sword", WeaponType.Sword2H, 15f, 1f, 3)
	{
		Debug.Log("Giant Sword constructor");
	}
}

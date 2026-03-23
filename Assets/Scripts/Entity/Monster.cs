using UnityEngine;
/// <summary>
///     Represents an AI-controlled monster entity.
/// </summary>
public class Monster : Actor
{
	public Monster(Transform location, Weapon weapon) : base(Attributes.Zombie(), weapon, location)
	{ }

	public bool IsDead()
	{
		return CurrentAction() == (int)Action.Dead;
	}
}

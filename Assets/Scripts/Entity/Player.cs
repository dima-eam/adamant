using UnityEngine;
/// <summary>
///     Represents the human-controlled "hero" entity.
/// </summary>
public class Player : Actor
{
	private readonly Attack _attack;
	private readonly Camera _mainCamera;

	public Player(Transform location, Weapon weapon, Attack attack, Camera mainCamera) : base(Attributes.Warrior(), weapon, location)
	{
		_attack = attack;
		_mainCamera = mainCamera;
	}

	/// <summary>
	///     Checks if player is attacking, to prevent certain interactions.
	/// </summary>
	/// <returns>true, if player is in attacking state</returns>
	public bool Attacking()
	{
		return CurrentAction() == (int)Action.Attack;
	}
	/// <summary>
	///     Starts an attack routine, which creates attack objects which collide with monsters and deal damage.
	/// </summary>
	public void SpawnAttacks()
	{
		var attack = Object.Instantiate(_attack, currentWeapon.Position, location.rotation); // TODO spawn position 
		attack.To(_direction);
	}
}

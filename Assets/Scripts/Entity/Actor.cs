using UnityEngine;
/// <summary>
///     Represents any active entity in the game world: a player/players, monsters, active NPCs, etc.
/// </summary>
public abstract class Actor // TODO move methods inside, so it's not a god class
{
	private readonly Attributes _attributes;
	protected readonly Transform location;
	// TODO fix field visibility
	private Action _currentAction = Action.Idle;
	protected Weapon currentWeapon;

	protected Vector3 _direction;

	protected Actor(Attributes attributes, Weapon currentWeapon, Transform location)
	{
		_attributes = attributes;
		this.currentWeapon = currentWeapon;
		this.location = location;
	}

	internal void MaybeAction(Action action)
	{
		_currentAction = action;
	}

	internal void RotateTo(Vector3 direction)
	{
		_direction = direction;

		var rotation = location.rotation;
		var targetRotation = Quaternion.LookRotation(direction);
		// interval controls how fast the rotation is done. The bigger the constant multiplier, the faster
		location.rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime*10f);
	}

	internal int CurrentAction()
	{
		return (int)_currentAction;
	}

	internal Vector3 CurrentPosition()
	{
		return location.position;
	}

	internal int WeaponType()
	{
		return (int)currentWeapon.type;
	}

	internal int WeaponRange()
	{
		return currentWeapon.range;
	}

	internal float MoveSpeed()
	{
		return _attributes.Speed;
	}
	
	
	internal void TakeDamage(int value)
	{
		_attributes.CurrentHealth -= value;
		if (_attributes.CurrentHealth <= 0)
		{
			MaybeAction(Action.Dead);
		}
	}
}

// Mapped to player or monster animation controller parameters and transitions
public enum Action
{
	Idle = 0,
	MoveForward = 1,
	MoveBackward = 2,
	Attack = 3,
	SpecialAttack = 4,
	Dead = -1
}

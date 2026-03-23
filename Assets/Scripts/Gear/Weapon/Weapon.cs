using Entity;
/// <summary>
/// </summary>
public abstract class Weapon : GameEntity
{
	public enum WeaponType
	{
		Unarmed = 0,
		Sword2H = 1
	}

	protected readonly float attackSpeed;
	protected readonly float damage;
	protected internal readonly int range;
	protected internal readonly WeaponType type;
	protected readonly string weaponName;

	protected Weapon(string weaponName, WeaponType type, float damage, float attackSpeed, int range)
	{
		this.weaponName = weaponName;
		this.type = type;
		this.damage = damage;
		this.attackSpeed = attackSpeed;
		this.range = range;
	}
}

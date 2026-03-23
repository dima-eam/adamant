public class Attributes
{
	internal int MaxHealth { get; private set; }
	internal int CurrentHealth { get; set; }
	internal float Speed { get; private set; }

	private Attributes(int maxHealth, int currentHealth, float speed)
	{
		MaxHealth = maxHealth;
		CurrentHealth = currentHealth;
		Speed = speed;
	}

	public static Attributes Warrior()
	{
		return new Attributes(100, 100, 5f);
	}
	public static Attributes Zombie()
	{
		return new Attributes(50, 50, 0.5f);
	}
}

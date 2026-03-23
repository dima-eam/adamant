/// <summary>
/// An interface to bridge Unity and non-Unity classes. Assumed that each implementing class does the Update()/LateUpdate() logic in Tick() method.
/// </summary>
public interface IUpdatable
{
	void Tick(float deltaTime);
}

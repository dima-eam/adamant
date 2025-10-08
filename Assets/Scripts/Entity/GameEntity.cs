using UnityEngine;
namespace Entity
{
	/// <summary>
	///     Base class for all instantiable game objects, which are part of gameplay. That is, static environment objects (walls, floor, etc.) are
	///     not game entities. Each game object has a name (not necessarily unique) and a position/rotation in the game world.
	/// </summary>
	public abstract class GameEntity : MonoBehaviour
	{
		public string Name { get; set; }

		public Vector3 Position => transform.position;

		public Quaternion Rotation => transform.rotation;
	}
}

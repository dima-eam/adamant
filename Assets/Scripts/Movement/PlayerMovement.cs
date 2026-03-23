using UnityEngine;
/// <summary>
///     <para>
///         Processes player movement as changing coordinates in the player game object and changing action value accordingly using old Unity input
///         system, available via static <see cref="Input" /> class method calls.
///     </para>
///     <para>
///         NOTE that coordinates are transformed according ot world rotation of -45 degrees (isometric view).
///     </para>
/// </summary>
public class PlayerMovement : IUpdatable
{
	// Input in local camera space, rotated by -45 degrees
	private static readonly Vector3 Forward = new(-0.35f, 0f, 0.35f);
	private static readonly Vector3 Right = new(0.71f, 0f, 0.71f);

	private readonly Player _player;
	private readonly CharacterController _controller;

	public PlayerMovement(Player player, CharacterController controller)
	{
		_player = player;
		_controller = controller;
	}

	public void Tick(float deltaTime)
	{
		MaybeWasdMove();
	}

	private void MaybeWasdMove()
	{
		if (_player.Attacking()) return;

		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");
		var input2 = new Vector2(h, v);
		var action = input2.SqrMagnitude() < 0.1f ? Action.Idle : Action.MoveForward;
		_player.MaybeAction(action);
		if (action == Action.Idle) return;

		var direction = (Forward*v + Right*h).normalized;
		_controller.Move(direction*(Time.deltaTime*_player.MoveSpeed()));
		// Debug.Log("Movement rotation: direction=" + direction);
		_player.RotateTo(direction);
	}
}

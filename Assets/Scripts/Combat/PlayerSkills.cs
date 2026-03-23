using UnityEngine;
/// <summary>
/// Processes skill usages: LMB attack, RMB attack, skills 1-5 using legacy input system. Setting player's action value to be processed by other components.
/// </summary>
public class PlayerSkills : IUpdatable
{
	private readonly Player _player;
	private readonly Camera _mainCamera;

	public PlayerSkills(Player player, Camera mainCamera)
	{
		_player = player;
		_mainCamera = mainCamera;
	}

	public void Tick(float deltaTime)
	{
		RotateToMouse();
		if (!Input.GetMouseButton(0)) return;

		_player.MaybeAction(Action.Attack);
	}

	private void RotateToMouse()
	{
		if (_player.CurrentAction() == (int)Action.MoveForward) return;
		_player.RotateTo(MouseDirection());
	}

	private Vector3 MouseDirection()
	{
		var direction = Vector3.zero;
		var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
		var ground = new Plane(Vector3.up, Vector3.zero);
		if (!ground.Raycast(ray, out var enter)) return direction;

		var hitPoint = ray.GetPoint(enter);
		// Direction should be from player to hit point, not from camera
		direction = (hitPoint - _player.CurrentPosition()).normalized;
		direction.y = 0f;
		// Debug.Log("Attack rotation: direction=" + direction);

		return direction;
	}
}

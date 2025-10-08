using UnityEngine;
/// <summary>
///     Processes player animation based on action value set by other components.
/// </summary>
public class PlayerAnimation : IUpdatable
{
	private static readonly int WeaponType = Animator.StringToHash("weaponType");
	private static readonly int Action = Animator.StringToHash("action");
	private static readonly int Attack = Animator.StringToHash("attack");

	private readonly Player _player;
	private readonly Animator _animator;

	public PlayerAnimation(Player player, Animator animator)
	{
		_player = player;
		_animator = animator;
		_animator.SetInteger(WeaponType, _player.WeaponType());
	}

	public void Tick(float deltaTime)
	{
		_animator.SetInteger(Action, _player.CurrentAction());
		_animator.SetBool(Attack, _player.Attacking());
	}
}

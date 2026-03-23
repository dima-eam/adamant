using UnityEngine;
/// <summary>
///     Processes monster animation based on action value set by other components.
/// </summary>
public class MonsterAnimation : IUpdatable
{
	private static readonly int Action = Animator.StringToHash("action");

	private readonly Actor _monster;
	private readonly Animator _animator;

	internal MonsterAnimation(Actor monster, Animator animator)
	{
		_monster = monster;
		_animator = animator;
	}

	public void Tick(float deltaTime)
	{
		_animator.SetInteger(Action, _monster.CurrentAction()); // todo set move speed and attack speed params per clip
		// animator.SetFloat("SpeedMultiplier", clipSpeedMultiplier);
	}
}

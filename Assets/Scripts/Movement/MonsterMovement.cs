using UnityEngine;
using UnityEngine.AI;
/// <summary>
///     Processes monster movement by target player, and changing action value accordingly using path-building logic.
/// </summary>
public class MonsterMovement : IUpdatable // TODO set animation speed + navmesh speed from code
{
	private const float DetectionRadius = 5f;

	private readonly Monster _monster;
	private readonly Transform _position;
	private readonly Animator _animator;
	private readonly NavMeshAgent _agent;

	private Transform _target;

	internal MonsterMovement(Monster monster, Transform position, Animator animator, NavMeshAgent agent)
	{
		_monster = monster;
		_position = position;
		_animator = animator;
		_agent = agent;
	}

	public void Tick(float deltaTime)
	{
		MaybeMove();
	}

	private void MaybeMove()
	{
		if (_monster.IsDead()) return;

		// Find player if no target yet
		if (!_target)
		{
			var player = GameObject.FindGameObjectWithTag("Player").transform;
			if (player)
			{
				_target = player.transform;
			}
		}

		// Player is not in detection radius
		if (!InDetectionRange(_target))
		{
			_monster.MaybeAction(Action.Idle);
			_agent.isStopped = true;
			return;
		}
		// todo simplify logic
		var distance = DistanceTo(_target.position);
		// Debug.Log("Distance to player: " + distance);
		if (distance <= _monster.WeaponRange()) // stop and attack
		{
			_monster.MaybeAction(Action.Attack);
			_agent.isStopped = true;
			_agent.destination = _position.position;
		}
		else if (distance <= DetectionRadius) // keep chasing
		{
			if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Freeze")) // still attacking
			{
				_monster.MaybeAction(Action.MoveForward);
				_agent.isStopped = false;
				_agent.destination = _target.position;
			}
			else // stop chasing
			{
				_monster.MaybeAction(Action.Idle);
				_agent.isStopped = true;
				_agent.destination = _position.position;
			}
		}
	}

	private bool InDetectionRange(Transform t)
	{
		return DistanceTo(t.position) <= DetectionRadius;
	}

	private float DistanceTo(Vector3 targetPos) // todo optimize redundant calls
	{
		return Vector3.Distance(_position.position, targetPos);
	}
}

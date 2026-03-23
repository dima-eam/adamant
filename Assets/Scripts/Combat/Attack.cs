using System;
using Entity;
using UnityEngine;
public abstract class Attack : GameEntity
{
	private Vector3 _direction = Vector3.zero;
	private Rigidbody _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (_direction == Vector3.zero)
		{
			return;
		}

		var newPos = _rigidbody.position + _direction.normalized*(3f*Time.fixedDeltaTime);
		_rigidbody.MovePosition(newPos);
	}

	public void To(Vector3 direction)
	{
		_direction = direction; // TODO should be closer to the object
		Debug.Log("Attack direction: " + direction);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Colliding with: " + other.gameObject.tag);
		// TODO investigate alternatives to filter out player's collision. Tag won't work because it collides with a child object
		if (other.GetComponentInParent<PlayerBootstrap>() != null) return;

		var monster = other.GetComponentInParent<MonsterBootstrap>();
		if (monster != null)
		{
			monster.TakeDamage(100);
		}

		Debug.Log("Destroy");
		Destroy(gameObject);
	}
}

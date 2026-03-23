using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Asynchronously prepares all the monster's dependencies and resources to be available at startup.
/// </summary>
public class MonsterBootstrap : MonoBehaviour
{
	private readonly List<IUpdatable> _updatables = new();

	private IUpdatable _animation;
	private IUpdatable _combat;
	private IUpdatable _movement;
	private Monster _monster;
	private Weapons _weapons;

	private async void Awake() // todo initialize from file?
	{
		try
		{
			Debug.Log("Bootstrapping monster");

			_weapons = new Weapons();
			await _weapons.InitializeAsync();

			var animator = GetComponent<Animator>();
			var hand = animator.GetBoneTransform(HumanBodyBones.RightHand).Find("B-handProp.R");
			var prefabFromCache = _weapons.PrefabFromCache(Weapons.Unarmed);
			_monster = new Monster(transform, Instantiate(prefabFromCache, hand));

			_animation = new MonsterAnimation(_monster, animator);

			var surface = FindFirstObjectByType<NavMeshSurface>();
			surface.BuildNavMesh();
			await WaitForNavMeshReadyAsync();

			var controller = GetComponent<CharacterController>();
			var agent = GetComponent<NavMeshAgent>();
			_movement = new MonsterMovement(_monster, controller.transform, animator, agent);

			_updatables.AddRange(new List<IUpdatable> { _animation, _movement });

			Debug.Log("Monster fully initialized");
		}
		catch (Exception e)
		{
			Debug.LogError("Error initializing monster: " + e.Message);
		}
	}

	internal void TakeDamage(int value)
	{
		_monster.TakeDamage(value);
	}

	private void Update()
	{
		var dt = Time.deltaTime;
		foreach (var u in _updatables)
			u.Tick(dt);
	}

	private async Task WaitForNavMeshReadyAsync()
	{
		while (NavMesh.CalculateTriangulation().vertices.Length == 0)
		{
			Debug.Log("Waiting for navmesh");
			await Task.Yield(); // let Unity update the frame
		}
	}
}

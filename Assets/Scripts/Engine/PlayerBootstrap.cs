using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asynchronously prepares all the player's dependencies and resources to be available at startup.
/// </summary>
public class PlayerBootstrap : MonoBehaviour
{
	private readonly List<IUpdatable> _updatables = new();

	private IUpdatable _animation;
	private IUpdatable _movement;
	private IUpdatable _skills;
	private Player _player;
	private Weapons _weapons;
	private Attacks _attacks;

	public void AnimationMid()
	{
		_player.SpawnAttacks();
	}

	public void AnimationEnd()
	{
		_player.MaybeAction(Action.Idle);
	}

	private async void Awake()
	{
		try
		{
			Debug.Log("Bootstrapping player");

			_weapons = new Weapons();
			await _weapons.InitializeAsync();
			_attacks = new Attacks();
			await _attacks.InitializeAsync();

			var animator = GetComponent<Animator>();
			var hand = animator.GetBoneTransform(HumanBodyBones.RightHand).Find("B-handProp.R");
			var prefabFromCache = _weapons.PrefabFromCache(Weapons.Unarmed);
			var weapon = Instantiate(prefabFromCache, hand);
			_player = new Player(transform, weapon, _attacks.PrefabFromCache(Attacks.UnarmedAttack), Camera.main);

			_animation = new PlayerAnimation(_player, animator);

			var controller = GetComponent<CharacterController>();
			_movement = new PlayerMovement(_player, controller);

			_skills = new PlayerSkills(_player, Camera.main);

			_updatables.AddRange(new List<IUpdatable> { _animation, _movement, _skills });

			Debug.Log("Player fully initialized");
		}
		catch (Exception e)
		{
			Debug.LogError("Error initializing player: " + e.Message);
		}
	}

	private void Update()
	{
		var dt = Time.deltaTime;
		foreach (var u in _updatables)
			u.Tick(dt); // TODO move animation to late update to prevent repeats
	}
}

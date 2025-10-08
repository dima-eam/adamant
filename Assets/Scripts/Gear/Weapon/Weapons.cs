using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
/// <summary>
///     Factory for all in-game weapons.
/// </summary>
public class Weapons
{
	private readonly Dictionary<string, Weapon> _prefabCache = new();

	internal static string GiantSword => "Weapon/GiantSword";
	internal static string Unarmed => "Weapon/Unarmed";

	public async Task InitializeAsync()
	{
		foreach (var key in new[] { GiantSword, Unarmed })
		{
			var handle = Addressables.LoadAssetAsync<GameObject>(key);
			var prefab = await handle.Task;
			if (prefab.GetComponent<Weapon>() == null)
			{
				Debug.LogError($"{key} has Weapon script missing");
				continue;
			}

			_prefabCache[key] = prefab.GetComponent<Weapon>();
			Debug.Log($"{key} loaded successfully!");
		}
	}

	internal Weapon PrefabFromCache(string key)
	{
		if (_prefabCache.Count == 0)
		{
			Debug.LogError("No weapons loaded!");
		}
		if (!_prefabCache.ContainsKey(key))
		{
			Debug.LogError($"{key} not found!");
		}

		return _prefabCache[key];
	}
}

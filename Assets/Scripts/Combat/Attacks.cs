using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
/// <summary>
/// A factory for all in-game attacks.
/// </summary>
public class Attacks
{
	private readonly Dictionary<string, Attack> _prefabCache = new();

	internal static string SlashAttack => "Attack/SlashAttack";
	internal static string UnarmedAttack => "Attack/UnarmedAttack";

	public async Task InitializeAsync()
	{
		foreach (var key in new[] { UnarmedAttack })
		{
			var handle = Addressables.LoadAssetAsync<GameObject>(key);
			var prefab = await handle.Task;
			if (prefab.GetComponent<Attack>() == null)
			{
				Debug.LogError($"{key} has Attack script missing");
				continue;
			}

			_prefabCache[key] = prefab.GetComponent<Attack>();
			Debug.Log($"{key} loaded successfully!");
		}
	}

	internal Attack PrefabFromCache(string key)
	{
		if (_prefabCache.Count == 0)
		{
			Debug.LogError("No attacks loaded!");
		}
		if (!_prefabCache.ContainsKey(key))
		{
			Debug.LogError($"{key} not found!");
		}

		return _prefabCache[key];
	}
}

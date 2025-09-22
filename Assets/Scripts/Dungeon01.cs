using System;
using UnityEditor;
using UnityEngine;
using World.Dungeon01;
using Random = UnityEngine.Random;

/**
 * Generates the game world using given generators, and places the player at (0, 0, 0) location.
 */
public class Dungeon01 : WithPrefabs
{
    private StraightRoom rooms;

    public void Start()
    {
        rooms = GetComponentInChildren<StraightRoom>();
        if (!rooms)
        {
            Debug.LogError("No straight room found");
            ExitGame();
        }

        Generate();
    }

    private void Generate()
    {
        var startPosition = new Vector3(0.7f, 0f, 0f);
        Instantiate(PrefabFromCache(Entrance), startPosition, Quaternion.identity);
        var roomWidth = 3;
        var roomHeight = Random.Range(4, 7);
        // offset start to entrance start
        startPosition = new Vector3(-7, 0, 10);
        rooms.Generate(startPosition, Vector3.forward, roomWidth, roomHeight);
    }

    private static void ExitGame()
    {
        // For builds (non-editor versions of Unity)
#if UNITY_EDITOR
        // In the Unity Editor, stop playing
        EditorApplication.isPlaying = false;
#else
                // In builds (game running outside the editor)
                Application.Quit();
#endif
    }
}
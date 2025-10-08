using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace World.Dungeon01
{
    public abstract class WithPrefabs : MonoBehaviour
    {
        private static Dictionary<string, GameObject> _prefabCache = new();

        protected static int TileSize = 4;

        protected static string Entrance => "Dungeon01/Entrance";
        protected static string RoomEntrance => "Dungeon01/RoomEntrance";
        protected static string TwoRoomConnectorV => "Dungeon01/TwoRoomConnectorV";
        protected static string RoomWallVR => "Dungeon01/RoomWallVR";
        protected static string RoomWallVL => "Dungeon01/RoomWallVL";
        protected static string RoomWallHL => "Dungeon01/RoomWallHL";
        protected static string RoomWallHU => "Dungeon01/RoomWallHU";
        protected static string RoomCornerLL => "Dungeon01/RoomCornerLL";
        protected static string RoomCornerLR => "Dungeon01/RoomCornerLR";
        protected static string RoomCornerUL => "Dungeon01/RoomCornerUL";
        protected static string RoomCornerUR => "Dungeon01/RoomCornerUR";
        protected static string FloorFlat4m => "Dungeon01/FloorFlat4m";
        protected static string TorchWall => "Dungeon01/TorchWall";
        protected static string Zombie => "Monsters/Zombie";

        private void Awake()
        {
            foreach (var key in new[]
                     {
                         Entrance, RoomEntrance, TwoRoomConnectorV, RoomWallVR, RoomWallVL, RoomWallHU, RoomCornerLL,
                         RoomCornerLR, RoomCornerUL, RoomCornerUR, FloorFlat4m, TorchWall, Zombie
                     })
            {
                var prefab = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
                _prefabCache[key] = prefab;
                Debug.Log($"{key} loaded successfully!");
            }
        }

        protected GameObject PrefabFromCache(string key)
        {
            return _prefabCache[key];
        }
    }
}
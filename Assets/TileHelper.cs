using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHelper : MonoBehaviour
{
    public static TileHelper INSTANCE;
    public Tilemap groundTilemap;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

    public bool IsWalkable(Vector3 pos)
    {
        return groundTilemap.HasTile(groundTilemap.WorldToCell(pos) - new Vector3Int(0, 1, 0));
    }

}

using UnityEngine;


[CreateAssetMenu(menuName = "Match 3 / Tile Type")]
public sealed class TileType : ScriptableObject
{
	public string Id;

	public int Value;

	public Sprite Sprite;
}


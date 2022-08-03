using UnityEngine;


[CreateAssetMenu(menuName = "Match 3 / Tile Type")]
public sealed class TileType : ScriptableObject
{
	public int Id;

	public int Value;

	public Sprite Sprite;
}


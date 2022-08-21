using UnityEngine;
using UnityEngine.UI;


public sealed class Tile : MonoBehaviour //& Tile Model
{
	public int X;
	public int Y;

	public Image Icon;

	public Button Button;

	private TileType _type;

	public TileType Type
	{
		get => _type;

		set
		{
			if (_type == value) return;

			_type = value;

			Icon.sprite = _type.Sprite;
		}
	}

	public TileData Data => new TileData(X, Y, _type.Id);
}


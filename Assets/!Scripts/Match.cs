
using System;

public sealed class Match
{
	public readonly int TypeId;

	public readonly int Score;

	public readonly TileData[] Tiles;

	public Match(TileData origin, TileData[] horizontal, TileData[] vertical)
	{
		TypeId = origin.TypeId;

		if (horizontal.Length >= 2 && vertical.Length >= 2)
		{
			Tiles = new TileData[horizontal.Length + vertical.Length + 1];

			Tiles[0] = origin;

			horizontal.CopyTo(Tiles, 1);

			vertical.CopyTo(Tiles, horizontal.Length + 1);

			// * Booster TOCHO
			
		}
		else if (horizontal.Length >= 2)
		{
			Tiles = new TileData[horizontal.Length + 1];

			Tiles[0] = origin;

			horizontal.CopyTo(Tiles, 1);

			// * Booster DELETE ROW
		}
		else if (vertical.Length >= 2)
		{
			Tiles = new TileData[vertical.Length + 1];

			Tiles[0] = origin;

			vertical.CopyTo(Tiles, 1);

			// * Booster DELETE COLUMN
		}
		else Tiles = null;

		Score = Tiles?.Length ?? -1;
	}
}



public readonly struct TileData
{
	public readonly int X;
	public readonly int Y;

	public readonly string TypeId;

	public TileData(int x, int y, string typeId)
	{
		X = x;
		Y = y;

		TypeId = typeId;
	}
}


using UnityEngine;

namespace MVC.Model
{
    public class TileModel //& Data Model
    {
        public int X;
        public int Y;

        public Vector2Int Position; 

        public string TypeId;

        public TileModel(int x, int y, string typeId)
        {
            X = x;
            Y = y;

            Position = new Vector2Int(x, y);

            TypeId = typeId;
        }

        public TileModel(Vector2Int position, string typeId)
        {
            Position = position;

            X = position.x;
            Y = position.y;

            TypeId = typeId;
        }
    }
}


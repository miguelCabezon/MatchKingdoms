using UnityEngine;

namespace MVC.Model
{
    [CreateAssetMenu(menuName = "Match 3 / Tile Type")]
    public sealed class TileType : ScriptableObject //& Creation of New Tiles
    {
        public string Id;

        public int Value;

        public Sprite Sprite;
    }
}


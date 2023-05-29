using UnityEngine;
using UnityEngine.UI;
using MVC.Model;

namespace MVC.View
{
    public sealed class TileView : MonoBehaviour //& Tile Model
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

        public TileModel Data => new TileModel(X, Y, _type.Id);
    }

}
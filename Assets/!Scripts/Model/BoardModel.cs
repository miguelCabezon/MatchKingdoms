// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;

// namespace MVC.Model
// {
//     public class BoardModel : MonoBehaviour //& Board Model
//     {
//         [SerializeField] public Row[] _rows;
//         [SerializeField] public TileType[] _tileTypes;

//         public TileModel[,] Matrix // Matrix filled with tile info
//         {
//             get
//             {
//                 var width = _rows.Max(row => row.Tiles.Length);
//                 var height = _rows.Length;

//                 var data = new TileModel[width, height];

//                 for (var y = 0; y < height; y++)
//                     for (var x = 0; x < width; x++)
//                         data[x, y] = GetTile(x, y).Data;

//                 return data;
//             }
//         }

//         private TileView GetTile(int x, int y) => _rows[y].Tiles[x];
//         private TileView[] GetTiles(IList<TileModel> tileData)
//         {
//             var length = tileData.Count;

//             var tiles = new TileView[length];

//             for (var i = 0; i < length; i++) tiles[i] = GetTile(tileData[i].X, tileData[i].Y);

//             return tiles;
//         }


//     }
// }

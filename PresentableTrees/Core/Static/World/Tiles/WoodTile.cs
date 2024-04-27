using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Static.World.Tiles {
				internal class WoodTile : Tile {
								public override Color color { get => Color.Brown; }

								public override TileType type => TileType.wood;

								public WoodTile(int x, int y) : base(x, y) { }
				}
}

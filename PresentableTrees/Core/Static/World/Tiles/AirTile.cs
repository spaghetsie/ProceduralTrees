using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Static.World.Tiles {
				internal class AirTile : Tile{
								public override Color color { get => Color.Transparent; }

								public override TileType type => TileType.air;

								public AirTile(int x, int y) : base(x, y) { }
				}
}

using Microsoft.Xna.Framework;
using PresentableTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Static.World {
				internal abstract class Tile {
								public int x;
								public int y;
								public float light_intensity = 0;
								public float energy = 0;

								public abstract Color color { get; }
								public abstract TileType type { get; }

								public Tile(int x, int y) {
												this.x = x;
												this.y = y;
								}

								public static Type TypeFromEnum(TileType tiletype) { 
												switch (tiletype) {
																case TileType.air:  return typeof(AirTile);
																case TileType.wood: return typeof(WoodTile);
												}
												return typeof(Nullable);
								}

				}
}

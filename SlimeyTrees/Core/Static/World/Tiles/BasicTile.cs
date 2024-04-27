using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Static.World.Tiles {
				internal class BasicTile : Tile{
								public override Color color { get => Color.Transparent; }
								public BasicTile(int x, int y) : base(x, y) { }
				}
}

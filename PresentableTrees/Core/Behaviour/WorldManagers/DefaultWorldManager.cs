using Microsoft.Xna.Framework;
using PresentableTrees.Core.Static.World;
using PresentableTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.WorldManagers {
				internal class DefaultWorldManager : WorldManager{
								public DefaultWorldManager(World world) : base(world) { }

								public override void Update(float deltaTime) {
												Random r = new Random();
												int x = r.Next(width);
												int y = r.Next(height);
												for(int i = 0; i < 100000; i++) {
																Tile current = tiles[x, y];

																if (current.type != TileType.air) { continue; }

																foreach(Tile neighbor in world.DirectNeighbours(x, y)) {
																				if (neighbor.type == TileType.wood) {
																								world.Insert(x, y, new WoodTile(x, y));
																								break;
																				}
																}
												}
								}

								public override void Init() {
												for (int x = 0; x < width; x++) {
																for (int y = 0; y < height; y++) {
																				
																				tiles[x, y] = new AirTile(x, y);
																}
												}

												Point current = new Point(32, 0);
												Random r = new Random();

												for (int i = 0; i < 64; i++) {
																world.Insert(current.X, current.Y, new WoodTile(current.X, current.Y));

																current += new Point(r.Next(-1, 2), r.Next(2));
												}
								}
				}
}

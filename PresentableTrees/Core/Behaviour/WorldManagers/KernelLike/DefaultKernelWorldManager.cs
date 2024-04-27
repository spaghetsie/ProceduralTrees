using Microsoft.Xna.Framework;
using PresentableTrees.Core.Static.World;
using PresentableTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.WorldManagers.KernelLike {
				internal class DefaultKernelWorldManager : KernelWorldManager {
								public DefaultKernelWorldManager(World world, float[,] kernel) : base(world, kernel) {

								}

								public DefaultKernelWorldManager(World world, int kernelWidth, int kernelHeight) : base(world, kernelWidth, kernelHeight) {

								}

								public override void Init() {
												for (int x = 0; x < width; x++) {
																for (int y = 0; y < height; y++) {

																				tiles[x, y] = new AirTile(x, y);
																}
												}

												Point current = new Point((int)width/2, 0);
												Random r = new Random(0);

												for (int i = 0; i < 64; i++) {
																world.Insert(current.X, current.Y, new WoodTile(current.X, current.Y));

																current += new Point(r.Next(-1, 2), r.Next(2));
												}
								}

								public override void Update(float deltaTime) {
												for (int i = 0; i < 4; i++) {
																int x_offset = 0;
																int y_offset = 0;

																switch (i) {
																				case 0:
																								 x_offset = 0;
																								 y_offset = 0;
																								break;
																				case 1:
																								 x_offset = 1;
																								 y_offset = 0;
																								break;
																				case 2:
																								 x_offset = 1;
																								 y_offset = 1;
																								break;
																				case 3:
																								x_offset = 0;
																								y_offset = 1;
																								break;
																}

																for(int x = x_offset; x < width; x+=2) {
																				for(int y = y_offset; y < height; y+=2) {
																								Tile current = tiles[x, y];

																								if (current.type != TileType.air) { continue; }

																								float chance2grow = EvaluateKernel(x, y);

																								if (chance2grow > 0 && RollChance(chance2grow)) {
																												world.Insert(x, y, new WoodTile(x, y));
																								}
																				}
																}
												}
								}

								protected override float EvaluateKernel(int x, int y) {
												float output = 0f;
												foreach (Tile tile in world.SurroundingTiles(x, y, kernelWidth, kernelHeight)) {
																if (tile.type == TileType.wood) {
																				output += kernel[kernelHeight-1-(tile.y - y + kernelHeight/2), tile.x - x + kernelWidth/2]; //tile.x - x + 1 = kernel x
																}
												}

												return output / 9;
								}
				}
}

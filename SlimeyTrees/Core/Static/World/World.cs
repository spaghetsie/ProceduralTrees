using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlimeyTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Static.World
{
				// world class wrapping up all tile array related stuff
    internal class World
    {
								// tile data
								public Tile[,] tiles;

								// world data
								public readonly int width;
								public readonly int height;

								// convenience field for checking whether a position is inside tile range
								public Rectangle _rect;
								public World(int width, int height) {
												tiles = new Tile[width, height];
												this.width = width;
												this.height = height;

												_rect = new Rectangle(0, 0, width, height);
								}

								// returns a Texture2D representation for drawing
								public Texture2D Texture2D(GraphicsDevice graphicsDevice, Func<Tile, Color> func = null) {

												if (func == null) {
																func = (Tile tile) => tile.color;
												}

												Texture2D texture = new Texture2D(graphicsDevice, width, height);
												Color[] texture_data = new Color[width*height];

												for(int x = 0; x<width; x++) {
																for (int y = 0; y < height; y++) {
																				texture_data[(height-y-1) * width + x] = func(tiles[x, y]);
																}
												}

												texture.SetData(texture_data);

												return texture;
								}

								public void Insert(int x, int y,  Tile tile) {
												tiles[x, y] = tile;
								}

								public IEnumerable<Tile> DirectNeighbours(int x, int y) {
												// Returns tiles directly touching tile[x, y] starting at top and going clockwise
												// If it is outside range it will not yield that tile

												if(_rect.Contains(x, y + 1))
																yield return tiles[x, y+1];
												if (_rect.Contains(x+1, y))
																yield return tiles[x+1, y];
												if (_rect.Contains(x, y - 1))
																yield return tiles[x, y-1];
												if (_rect.Contains(x-1, y))
																yield return tiles[x-1, y];
								}

								public IEnumerable<Tile> AllNeighbours(int x, int y) {
												// Returns tiles directly (/w sides) and indirectly (/w corner) touching tile[x, y]
												// starting at the top and going clockwise
												// If it is outside range it will not yield that tile

												if (_rect.Contains(x, y + 1))
																yield return tiles[x, y + 1];

												if (_rect.Contains(x + 1, y + 1))
																yield return tiles[x + 1, y + 1];

												if (_rect.Contains(x + 1, y))
																yield return tiles[x + 1, y];

												if (_rect.Contains(x + 1, y - 1))
																yield return tiles[x + 1, y - 1];

												if (_rect.Contains(x, y - 1))
																yield return tiles[x, y - 1];

												if (_rect.Contains(x - 1, y - 1))
																yield return tiles[x - 1, y - 1];

												if (_rect.Contains(x - 1, y))
																yield return tiles[x - 1, y];

												if (_rect.Contains(x - 1, y + 1))
																yield return tiles[x -1 , y + 1];
								}

								public IEnumerable<Tile> SurroundingTiles(int x, int y, int width, int height) {
												// Returns tiles in a rectangle of size (width, height) centered around (x, y)
												// Order should not be depended on
												// If it is outside range it will not yield that tile

												int startingX = x - (int)(width/2);
												int startingY = y - (int)(height/2);

												for(int _x = 0;  _x < width; _x++) {
																for(int _y = 0; _y < height; _y++) { 
																				if(!_rect.Contains(_x + startingX, _y + startingY)) { continue; }
																				
																				yield return tiles[_x + startingX, _y + startingY];
																}
												}
								}
				}
}

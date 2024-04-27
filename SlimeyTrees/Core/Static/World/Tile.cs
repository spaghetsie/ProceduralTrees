using Microsoft.Xna.Framework;
using SlimeyTrees.Core.Behaviour.SlimeParticle;
using SlimeyTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Static.World {
				
				// Class from which tiletypes inherit
				internal abstract class Tile {
								// position and pheromone channels (light, wood, etc.)
								public int x;
								public int y;
								public float[] pheromones;

								// convenience getters
								public float wood { get => pheromones[(int)Pheromone.wood]; set => pheromones[(int)Pheromone.wood] = value; }
								public float light { get => pheromones[(int)Pheromone.light]; set => pheromones[(int)Pheromone.light] = value; }
								public float leaves { get => pheromones[(int)Pheromone.leaves]; set => pheromones[(int)Pheromone.leaves] = value; }
								public float obstacles { get => pheromones[(int)Pheromone.obstacles]; set => pheromones[(int)Pheromone.obstacles] = value; }
								// Shoujld be overriden to return the tile's color
								public abstract Color color { get; }

								public Tile(int x, int y) {
												this.x = x;
												this.y = y;

												this.pheromones = new float[4];
								}

				}
}

using PresentableTrees.Core.Static.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour {
				internal abstract class WorldManager {
								public World world { get; }

								protected Tile[,] tiles { get => world.tiles;}
								protected int width { get => world.width; }
								protected int height { get => world.height; }

								public WorldManager(World world) {
												this.world = world;
								}

								public abstract void Update(float deltaTime);

								public abstract void Init();
								
				}
}

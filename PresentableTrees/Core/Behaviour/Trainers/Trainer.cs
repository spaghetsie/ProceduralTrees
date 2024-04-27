using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PresentableTrees.Core.Behaviour.Mutators;
using PresentableTrees.Core.Behaviour.WorldManagers;
using PresentableTrees.Core.Static.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.Trainers {
				internal abstract class Trainer {
								protected readonly int size;
								protected KernelManagerMutator mutator; //TODO: support non-kernel-like mutators
								protected KernelWorldManager[] worldManagers;

								public Trainer(int size, KernelManagerMutator mutator) {
												this.size = size;
												this.mutator = mutator;
												this.worldManagers = new KernelWorldManager[size];


								}

								public abstract void Init();

								public abstract void Reset();

								public abstract void Update(float deltaTime);

								public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
												for(int i = 0; i < size; i++) {
																WorldManager manager = worldManagers[i];

																spriteBatch.Draw(
																				manager.world.Texture2D(graphicsDevice),
																				new Rectangle(manager.world.width*(i%3)*2, manager.world.height*(i/3)*2, manager.world.width*2, manager.world.height*2),
																				Color.White
																);
												}
								}

								public abstract void RepopulateSuperior(int index);
				}
}

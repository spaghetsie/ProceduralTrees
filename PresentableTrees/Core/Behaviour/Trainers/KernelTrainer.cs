using PresentableTrees.Core.Behaviour.Mutators;
using PresentableTrees.Core.Behaviour.WorldManagers;
using PresentableTrees.Core.Behaviour.WorldManagers.KernelLike;
using PresentableTrees.Core.Static.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.Trainers {
				internal class KernelTrainer : Trainer {
								public KernelTrainer(int size, KernelManagerMutator mutator) : base(size, mutator) { }

								public override void Init() {
												for(int i = 0; i < size; i++) {
																worldManagers[i] = new DefaultKernelWorldManager(
																				new World(64, 64),
																				new float[,] { 
																								{-1, -1, -1, -1, -1 },
																								{-1, -1, -1, -1, -1 },
																								{-1, -1,  0, -1, -1 },
																								{-1,  1,  1,  1, -1 },
																								{-1, -1,  1, -1, -1 }
																				}
																);

																worldManagers[i].Init();
												}
								}

								public override void Reset() {
												for (int i = 0; i < size; i++) {
																worldManagers[i].Init();
												}
								}

								public override void Update(float deltaTime) {
												foreach(WorldManager manager in worldManagers) {
																manager.Update(deltaTime);
												}
								}

								public override void RepopulateSuperior(int index) {												
												for(int i = 0; i<size; i++) {
																if(i == index) { continue; }

																worldManagers[i] = new DefaultKernelWorldManager(worldManagers[i].world, mutator.Mutate(worldManagers[index]));
												}

												Reset();
								}
				}
}

using PresentableTrees.Core.Behaviour.WorldManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.Mutators.KernelMutators.cs {
				internal class DefaultKernelManagerMutator : KernelManagerMutator {

								public DefaultKernelManagerMutator(
												float MutationChance = 0.1f,
												float MutationIntensity = 0.1f
												): base(
																MutationChance,
																MutationIntensity
												){
												
								}
								public override float[,] Mutate(KernelWorldManager manager) {
												float[,] mutant = new float[manager.kernel.GetLength(0), manager.kernel.GetLength(1)];

												for(int x = 0; x < manager.kernel.GetLength(0); x++) {
																for(int y = 0; y < manager.kernel.GetLength(1); y++) {
																				if(!ShouldMutate()) { continue; }

																				mutant[x, y] = manager.kernel[x, y] + MutationByValue();
																}
												}

												return mutant;
								}
				}
}

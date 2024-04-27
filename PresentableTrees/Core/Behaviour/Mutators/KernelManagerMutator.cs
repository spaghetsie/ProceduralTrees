using PresentableTrees.Core.Behaviour.WorldManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.Mutators {
				internal abstract class KernelManagerMutator {
								protected float MutationChance;
								protected float MutationIntensity;
								public KernelManagerMutator(float MutationChance=0.1f, float MutationIntensity=0.1f) {
												this.MutationChance = MutationChance;
												this.MutationIntensity = MutationIntensity;
								}

								protected bool ShouldMutate() {
												return new Random().NextSingle() < MutationChance;
								}

								protected float MutationByValue() {
												float temp = MathF.Pow((new Random().NextSingle() - 0.5f) * MutationChance, 1 / 3);
												return temp;
								}

								public abstract float[,] Mutate(KernelWorldManager manager); // Returns mutated kernel
				}
}

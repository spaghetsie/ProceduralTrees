using PresentableTrees.Core.Static.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.WorldManagers {
				internal abstract class KernelWorldManager : WorldManager {
								public float[,] kernel;
								protected int kernelWidth;
								protected int kernelHeight;
								protected abstract float EvaluateKernel(int x, int y);

								protected bool RollChance(float chance) {
												Random r = new Random();

												if (r.NextSingle() >= chance) return false;

												return true;
								}
								public KernelWorldManager(World world, float[,] kernel) : base(world) {
												if (kernel == null)
																this.kernel = new float[,]{
																				{ -1f, 0f, -1f},
																				{ 0f, 0f, 0f},
																				{ -1f, 0f, -1}
																};
												else
																this.kernel = kernel;

												this.kernelWidth = kernel.GetLength(0);
												this.kernelHeight = kernel.GetLength(1);
								}

								public KernelWorldManager(World world, int kernelWidth, int kernelHeight) : base(world) {
												this.kernel = new float[kernelWidth, kernelHeight];

												this.kernelWidth = kernelWidth;
												this.kernelHeight = kernelHeight;
								}
				}
}

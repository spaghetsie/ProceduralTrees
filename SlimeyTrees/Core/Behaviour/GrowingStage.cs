using SlimeyTrees.Core.Behaviour.SlimeParticle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Behaviour
{
				// stages define the behaviour of growing stages
    internal class GrowingStage
    {
								// See ParticleProperties.cs
        public readonly ParticleProperties particleProperties;
								// Amount of particles to be used in a stage
        public readonly int particleCount;
								// How many frames should a stage last for
								public readonly int stageDuration;
								// What is the main pheromone (light, wood, etc) a stage is working with
								//   used for leaving trails, diffuse and decay
								public readonly int PheromoneTargetLayer;
								// How much should trails diffuse
								public readonly float BlurIntensity;
								// How much to subtract from tiles each frame
								public readonly float DecayIntensity;
								// Should particles respawn after dying
								public readonly bool DoRespawns;
								// Whether to wait for all particles to die before respawning
								// Ignored if DoRespawns == false
								public readonly bool RespawnAllAtOnce;
								// Should particles leave trails in <PheromoneTargetLayer>
								public readonly bool LeavesTrails;
								public GrowingStage(
																												ParticleProperties particleProperties,
																												int particleCount,
																												int stageDuration,
																												int PheromoneTargetLayer,
																												float BlurIntensity,
																												float DecayIntensity,
																												bool RespawnOn,
																												bool RespawnAllAtOnce,
																												bool LeavesTrails
								) {
												this.particleProperties = particleProperties;
												this.particleCount = particleCount;
												this.stageDuration = stageDuration;
												this.PheromoneTargetLayer = PheromoneTargetLayer;
												this.BlurIntensity = BlurIntensity;
												this.DecayIntensity = DecayIntensity;
												this.DoRespawns = RespawnOn;
												this.RespawnAllAtOnce = RespawnAllAtOnce;
												this.LeavesTrails = LeavesTrails;
								}
    }
}

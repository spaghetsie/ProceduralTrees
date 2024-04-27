using Microsoft.Xna.Framework;
using SlimeyTrees.Core.Static.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Behaviour.SlimeParticle {
				// class wrapping up particle properties for use in particle groups (stages)
				internal class ParticleProperties {
								public Func<float> speed;
								public Func<float> lifespan;

								// All functions are called on each particle instance separately

								// Function returning an array of floats to be used as PheromoneAttraction (see Particle.cs)
								public Func<float[]> PheromoneAttraction;
								// Function returning the position where particle(s) should be spawned
								public Func<Vector2> SpawnPos;
								// Function returning the angle at which particle(s) should be spawned
								public Func<float> SpawnAngle;

								public Action<Particle, Tile> OnDeath;
								public Action<Particle, Tile> OnPassThrough;

								public ParticleProperties(
																								Func<float> speed,
																								Func<float> lifespan,
																								Func<float[]> PheromoneAttraction,
																								Func<Vector2> SpawnPos,
																								Func<float> SpawnAngle,
																								Action<Particle, Tile> OnDeath,
																								Action<Particle, Tile> OnPassThrough
								) {
												this.speed = speed;
												this.lifespan = lifespan;
												this.PheromoneAttraction = PheromoneAttraction;
												this.SpawnAngle = SpawnAngle;
												this.SpawnPos = SpawnPos;
												this.OnDeath = OnDeath;
												this.OnPassThrough = OnPassThrough;
								}
				}
}

using Microsoft.Xna.Framework;
using SlimeyTrees.Core.Static.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Behaviour.SlimeParticle {
				// Particle (slime agent) class
				internal class Particle {
								public Vector2 pos;
								public float angle;
								public readonly float speed;
								// TimeToLive - death once zero
								public float TTL;

								// Action called on death
								public Action<Particle, Tile> OnDeath;
								// Action called every frame the particle moves
								// Takes the tile it is on as an argument
								public Action<Particle, Tile> OnPassThrough;

								// Array of weights (biases) indicating tendecies to go towards certain tile channels (light, wood, etc)
								public float[] PheromoneAttraction;

								public Particle(
																								Vector2 pos,
																								float speed,
																								float angle,
																								float TTL,
																								Action<Particle, Tile> OnDeath,
																								Action<Particle, Tile> OnPassThrough,
																								float[] PheromoneAttraction
								) {
												this.pos = pos;
												this.angle = angle;
												this.speed = speed;
												this.TTL = TTL;
												this.OnDeath = OnDeath;
												this.OnPassThrough = OnPassThrough;
												this.PheromoneAttraction = PheromoneAttraction;
								}
								
				}


}

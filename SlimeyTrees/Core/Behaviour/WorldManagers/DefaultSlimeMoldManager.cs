using Microsoft.Xna.Framework;
using SlimeyTrees.Core.Behaviour.SlimeParticle;
using SlimeyTrees.Core.Static.World;
using SlimeyTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Behaviour.WorldManagers {
				internal class DefaultSlimeMoldManager : SlimeWorldManager {


								public DefaultSlimeMoldManager(World world) : base(world) {
												Random r = new Random();
												
												base.growingStages = new GrowingStage[5] {
																// inital stage with no particles
																// used for drawing obstacles
																new GrowingStage(
																				null,
																				particleCount: 0,
																				stageDuration: 0,
																				PheromoneTargetLayer: 3,
																				BlurIntensity: 2f,
																				DecayIntensity: 0.0f,
																				RespawnOn: false,
																				RespawnAllAtOnce: false,
																				LeavesTrails: false
																),
																// trunk growing stage
																new GrowingStage(
																				new ParticleProperties(
																								speed: () => 1f,
																								lifespan: () => 30f,
																								PheromoneAttraction: () => new float[4] { 0.06f, 0.03f, 0, -0.3f},
																								SpawnPos: () => new Vector2(width/2f + (r.NextSingle()-0.5f) * 4, 0),
																								SpawnAngle: () => MathF.PI/2 + (r.NextSingle()-0.5f) * 0,
																								OnDeath: (Particle particle, Tile tile) => { },
																								OnPassThrough: (Particle particle, Tile tile) => { }
																				),
																				particleCount: 20,
																				stageDuration: 250,
																				PheromoneTargetLayer: 1,
																				BlurIntensity: 0.1f,
																				DecayIntensity: 0.05f,
																				RespawnOn: true,
																				RespawnAllAtOnce: true,
																				LeavesTrails: true
																),
																// major branching stage
																new GrowingStage(
																				new ParticleProperties(
																								speed: () => 1f+r.NextSingle()/100f,
																								lifespan: () => 100f,//* r.NextSingle(),
																								PheromoneAttraction: () => new float[4] { 0.4f, 0.03f, 0, -0.3f},
																								SpawnPos: () => new Vector2(width/2f + (r.NextSingle()-0.5f) * 3, 0),
																								SpawnAngle: () => MathF.PI/2 + (r.NextSingle()-0.5f) * MathF.PI/8,
																								OnDeath: (Particle particle, Tile tile) => { tile.leaves = 10; },
																								OnPassThrough: (Particle particle, Tile tile) => { }
																				),
																				particleCount: 10,
																				stageDuration:  200,
																				PheromoneTargetLayer: 1,
																				BlurIntensity: 0.01f,
																				DecayIntensity: 0.0f,
																				RespawnOn: true,
																				RespawnAllAtOnce: false,
																				LeavesTrails: true
																),
																// minor branching stage
																new GrowingStage(
																				new ParticleProperties(
																								speed: () => 1.4f * (1 - r.NextSingle()/4),
																								lifespan: () => 90f,
																								PheromoneAttraction: () => new float[4] { 0.5f, 0.08f, -1f, -0.3f},
																								SpawnPos: () => new Vector2(width/2f + (r.NextSingle()-0.5f) * 3, 0),
																								SpawnAngle: () => MathF.PI/2 + (r.NextSingle()-0.5f) * MathF.PI/8,
																								OnDeath: (Particle particle, Tile tile) => { },
																								OnPassThrough: (Particle particle, Tile tile) => {
																												if(tile.wood >0.5f) {
																																particle.TTL=10;
																												}
																								}
																				),
																				particleCount: 10,
																				stageDuration: 200,
																				PheromoneTargetLayer: 1,
																				BlurIntensity: 0.03f,
																				DecayIntensity: 0.0f,
																				RespawnOn: true,
																				RespawnAllAtOnce: false,
																				LeavesTrails: true
																),
																// foliage stage
																new GrowingStage(
																				new ParticleProperties(
																								speed: () => 0.5f + r.NextSingle(),
																								lifespan: () => 300f,
																								PheromoneAttraction: () => new float[4] { 0.2f, 0.03f, -1f, -0.3f},
																								SpawnPos: () => new Vector2(width/2, 0),
																								SpawnAngle: () => MathF.PI/2,
																								OnDeath: (Particle particle, Tile tile) => { 
																												if (tile.leaves > 0 && tile.wood < 0.1){
																																tile.leaves = 1; 
																												} 
																								},
																								OnPassThrough: (Particle particle, Tile tile) => {
																												if(tile.leaves > 0.5f ) {
																																particle.TTL = 2;
																												}

																												if(tile.wood > 0.1) {
																																particle.TTL = 5f;
																												}
																								}
																				),
																				particleCount: 1000,
																				stageDuration: 1000,
																				PheromoneTargetLayer: 2,
																				BlurIntensity: 0.5f,
																				DecayIntensity: 0.00f,
																				RespawnOn: true,
																				RespawnAllAtOnce: false,
																				LeavesTrails: false
																)
												};
								}
				}
}

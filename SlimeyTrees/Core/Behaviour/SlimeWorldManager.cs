using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlimeyTrees.Core.Behaviour.SlimeParticle;
using SlimeyTrees.Core.Static.World;
using SlimeyTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeyTrees.Core.Behaviour {
				internal abstract class SlimeWorldManager : WorldManager {
								protected Particle[] particles;
								protected int particleCount;

								protected GrowingStage[] growingStages;
								protected GrowingStage CurrentStage;
								protected int stageFrame;
								protected int stage;

								protected bool IsRunning = false;
								
								protected SlimeWorldManager(World world) : base(world) {
								}

								// Initialize all neccessary fields; Must be called before first Update
								public override void Init() {
												stageFrame = 0;
												stage = 0;
												CurrentStage = growingStages[stage];

												SetupTiles();
												SpawnParticles(CurrentStage);

												IsRunning = true;
								}

								// Essentially resets world tiles
								//   Initializes tiles
								public void SetupTiles() {
												Random r = new Random();
												for (int x = 0; x < width; x++) {
																for (int y = 0; y < height; y++) {

																				tiles[x, y] = new BasicTile(x, y);
																				tiles[x, y].light = MathF.Sqrt((y) / (float)(height)) * (1 - r.NextSingle() * 0.3f);

																				/*if (2*y+x < height*0.95 && MathF.Abs(x - 2*y) < 50) {
																								tiles[x, y].obstacles = (50-MathF.Abs(x - 2 * y))/50;
																				}*/

																				//tiles[x, y].light = (y * 4 + MathF.Abs(x - width / 2)) / (float)(height * 4 + width / 2) * (1-r.NextSingle()*0.8f);
																}
												}
								}

								// Main update method; called each frame
								public override void Update(float deltaTime) {
												if(!IsRunning) { return; } // simulation is paused

												// increment frame counter
												stageFrame += 1; 

												
												GrowingStage CurrentStage = growingStages[stage];
												// check if we need to move on to another stage
												if (stageFrame > CurrentStage.stageDuration) {
																NextStage();
												}

												// update
												UpdateTiles(deltaTime);
												UpdateParticles(deltaTime);
								}

								// Initializes particles; Called on Init and on NextStage
								public void SpawnParticles(GrowingStage stage) {
												// Clears current particles and spawn new ones
												this.particleCount = stage.particleCount;
												particles = new Particle[stage.particleCount];
												for(int i = 0; i < stage.particleCount; i++) {
																particles[i] = new Particle(
																				stage.particleProperties.SpawnPos(),
																				stage.particleProperties.speed(),
																				stage.particleProperties.SpawnAngle(),
																				stage.particleProperties.lifespan(),
																				stage.particleProperties.OnDeath,
																				stage.particleProperties.OnPassThrough,
																				stage.particleProperties.PheromoneAttraction()
																				);
												}

								}

								// cleans up current stage and sets things up for the next
								public void NextStage() {
												// check if there is another stage
												if(stage + 1 >= growingStages.Length) {
																// if not, pause
																stageFrame = 0;
																IsRunning = false;
																return;
												}

												// else switch stages
												stageFrame = 0;
												stage += 1;
												CurrentStage = growingStages[stage];
												SpawnParticles(CurrentStage);
								}

								// subtracts from pheromone layer the current stage is targeting
								//   locks value at or above one
								public void Decay(int x, int y, float deltaTime) {
												tiles[x, y].pheromones[CurrentStage.PheromoneTargetLayer] -= CurrentStage.DecayIntensity * deltaTime;
												if (tiles[x, y].pheromones[CurrentStage.PheromoneTargetLayer] <= 0) {
																tiles[x, y].pheromones[CurrentStage.PheromoneTargetLayer] = 0;
												}
								}

								// Gaussian blur kernel
								// used for pheromone diffusion
								protected float[,] BlurKernel = new float[3, 3] {
												{1, 2, 1},
												{2, 4, 2},
												{1, 2, 1}
								};
								// Sum of values from kernel above
								protected float BlurKernelSum = 16;

								// Diffuses a 3x3 grid around (x, y)
								public void Diffuse(int x, int y, float deltaTime) {
												// check if blurring is necessary
												if (CurrentStage.BlurIntensity <= 0) { return; }

												// sum up tile values * PheromoneTargetLayer weights
												float kernelSum = 0;
												foreach (Tile tile in world.SurroundingTiles(x, y, 3, 3)) {
																kernelSum += BlurKernel[2 - (tile.y - y + 1), tile.x - x + 1] * tile.pheromones[CurrentStage.PheromoneTargetLayer];
												}
												// normalize sum
												kernelSum /= BlurKernelSum;

												// diffusion intensity
												float diffuseWeight = deltaTime * CurrentStage.BlurIntensity;

												// original tile value
												float original = tiles[x, y].pheromones[CurrentStage.PheromoneTargetLayer];

												// final value calculation
												kernelSum = original * (1 - diffuseWeight) + kernelSum * diffuseWeight;

												// set kernelSum to tiles[x, y]
												tiles[x, y].pheromones[CurrentStage.PheromoneTargetLayer] = kernelSum;
								}

								// Calles Diffuse and Decay on all tiles
								//		 to avoid diffusion bleeding into tiles mid-frame
								//   tiles are updated non-sequentially
								public void UpdateTiles(float deltaTime) {
												// check if there is a need to update
												if (CurrentStage.BlurIntensity <= 0 && CurrentStage.DecayIntensity <= 0) { return; }

												// Divide grid into 4 dithered patterns
												for (int i = 0; i < 4; i++) {
																int x_offset = 0;
																int y_offset = 0;

																// current pattern offset
																switch (i) {
																				case 0:
																								x_offset = 0;
																								y_offset = 0;
																								break;
																				case 1:
																								x_offset = 1;
																								y_offset = 0;
																								break;
																				case 2:
																								x_offset = 1;
																								y_offset = 1;
																								break;
																				case 3:
																								x_offset = 0;
																								y_offset = 1;
																								break;
																}

																// update tiles in current grid pattern
																for (int x = x_offset; x < width; x += 2) {
																				for (int y = y_offset; y < height; y += 2) {
																								if (CurrentStage.BlurIntensity > 0) 
																												Diffuse(x, y, deltaTime);
																								
																								if (CurrentStage.DecayIntensity > 0)
																												Decay(x, y, deltaTime);
																				}
																}
												}
								}

								
								// Called each frame
								public void UpdateParticles(float deltaTime) {
												Random r = new Random();
												int AliveParticleCount = particleCount;

												for (int i = 0; i < particleCount; i++) {

																// fetch current particle and stage particle properties
																Particle particle = particles[i];
																ParticleProperties properties = CurrentStage.particleProperties;
																
																// null check (probably means particle has despawned)
																if (particle == null)
																				continue;

																// screen coordinates
																int x = (int)particle.pos.X;
																int y = (int)particle.pos.Y;

																// tile the particle is on
																Tile tile = tiles[x, y];


																// particle reached end of lifespan
																if (particle.TTL <= 0) {
																				AliveParticleCount--;

																				particle.OnDeath(particle, tile);
																				if (!CurrentStage.DoRespawns || CurrentStage.RespawnAllAtOnce) {
																								particle = null;
																								continue;
																				}
																				particle.TTL = CurrentStage.particleProperties.lifespan();
																				particle.pos = CurrentStage.particleProperties.SpawnPos();
																				particle.angle = CurrentStage.particleProperties.SpawnAngle();
																				continue;
																}

																

																// decrease TimeToLive
																particle.TTL -= 1.5f + (height - y) / height - tile.pheromones[CurrentStage.PheromoneTargetLayer];

																// call particle's OnPassThrough
																particle.OnPassThrough(particle, tile);

																// leave trail
																if (CurrentStage.LeavesTrails)
																				tile.pheromones[CurrentStage.PheromoneTargetLayer] = 1;

																// Steer towards pheromones with some randomness																							
																particle.angle += Sense(particle)*(r.NextSingle() / 1.1f + (1 - 1 / 1.1f));

																// change in position
																Vector2 direction = new Vector2(MathF.Cos(particle.angle), MathF.Sin(particle.angle)) * particle.speed;

																// if on edge or hit an obstacle, don't move and randomise angle
																if (!world._rect.Contains(particle.pos + direction) || TileFromVector(particle.pos + direction).obstacles > 0.5) {
																				particle.angle += (r.NextSingle()-0.5f);
																				continue;
																}

																// move particle
																particles[i].angle = particle.angle;
																particle.pos += direction;
												}

												if (CurrentStage.RespawnAllAtOnce && AliveParticleCount == 0) {
																SpawnParticles(CurrentStage);
												}
								}

								// Convenience method for getting tiles from a Vector2 position
								protected Tile TileFromVector(Vector2 pos) {
												if (!world._rect.Contains(pos)) {
																return new BasicTile(0, 0);
												}
												return tiles[(int)pos.X, (int)pos.Y];
								}

								// Returns a float indicating which way to turn
								//   Samples three tiles for the result
								protected float Sense(Particle particle) {

												// forward sensor
												Tile forward = TileFromVector(
																												new Vector2(
																																												MathF.Cos(particle.angle),
																																												MathF.Sin(particle.angle)
																												) * 2.5f + particle.pos
												);
												
												// left sensor
												Tile left = TileFromVector(
																												new Vector2(
																																												MathF.Cos(particle.angle - MathF.PI / 6),
																																												MathF.Sin(particle.angle - MathF.PI / 6)
																												) * 2.5f + particle.pos
												);

												// right sensor
												Tile right = TileFromVector(
																												new Vector2(
																																												MathF.Cos(particle.angle + MathF.PI / 6),
																																												MathF.Sin(particle.angle + MathF.PI / 6)
																												) * 2.5f + particle.pos
												);

												float turnLeft  = 0;
												float goForward = 0;
												float turnRight = 0;

												float normalizer = 0f;
												// calculate tendencies to go in respective directions\
												for (int i = 0; i < forward.pheromones.Length; i++) {
																normalizer += MathF.Abs(particle.PheromoneAttraction[i]);
																turnLeft  += left.pheromones[i]    * particle.PheromoneAttraction[i];
																goForward += forward.pheromones[i] * particle.PheromoneAttraction[i];
																turnRight += right.pheromones[i]   * particle.PheromoneAttraction[i];
												}
												


												// determine which tile has most energy\
												// dont touch; it works
												if (turnLeft > goForward) {
																if (turnLeft > turnRight) {
																				return -turnLeft / normalizer;
																}
																else if (turnLeft == turnRight) {
																				return 0;
																}
																return turnRight / normalizer;
												}
												if (turnRight > goForward) {
																return turnRight / normalizer;
												}

												return 0;

								}

								// returns a transparent texture where tiles with particles are black
								public Texture2D ParticlesTexture(GraphicsDevice graphicsDevice) {


												Texture2D texture = new Texture2D(graphicsDevice, width, height);
												Color[] texture_data = new Color[width * height];

												for (int i = 0; i < particleCount; i++) {
																int x = (int)particles[i].pos.X;
																int y = (int)particles[i].pos.Y;
																texture_data[(height - y - 1) * width + x] = Color.Black;
												}

												texture.SetData(texture_data);

												return texture;
								}
				}
}

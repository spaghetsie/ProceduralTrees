using Microsoft.Xna.Framework;
using PresentableTrees.Core.Static.World;
using PresentableTrees.Core.Static.World.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentableTrees.Core.Behaviour.WorldManagers.SlimeMoldLike {
				internal class SlimeMoldManager : WorldManager {
								private struct Particle {
												public Vector2 pos;
												public float angle;
												public readonly float speed;
												public float TTL;

												public Particle(Vector2 pos, float speed, float angle, float TTL) {
																this.pos = pos;
																this.angle = angle;
																this.speed = speed;
																this.TTL = TTL;
												}
								}

								private struct ParticleProperites {
												public float speed;
												public float lifespan;

												public float lightAttraction;
												public float energyAttraction;
												public Action<Tile> OnPassThrough;

												public ParticleProperites(
																float speed,
																float lifespan,
																float lightAtraction,
																float energyAtraction,
																Action<Tile> OnPassThrough
												) {
																this.speed = speed;
																this.lifespan = lifespan;
																this.lightAttraction = lightAtraction;
																this.energyAttraction = energyAtraction;
																this.OnPassThrough = OnPassThrough;
												}
								}

								private Particle[] particles;
								private int particleCount;
								public SlimeMoldManager(World world) : base(world) {
								}

								public override void Init() {
												Random random = new Random();
												for (int x = 0; x < width; x++) {
																for (int y = 0; y < height; y++) {

																				tiles[x, y] = new AirTile(x, y);
																				tiles[x, y].light_intensity = (y + MathF.Abs(width/2-x)) / (float)(height + width / 2);
																}
												}
												SpawnParticles(width/2, 0, MathF.PI/2, 1000);
												RecalculateKernelSum();
								}

								public void SpawnParticles(float x, float y, float angle, int count) {
												Random r = new Random();

												this.particleCount = count;
												particles = new Particle[particleCount];

												for (int i = 0; i < particles.Length; i++) {
																particles[i] = new Particle(
																				new Vector2(x, y),//new Vector2(r.NextSingle()*255, r.NextSingle() * 255),
																				0.5f,
																				angle + (r.NextSingle()-0.5f) * 3.141f / 9f,
																				120 * r.NextSingle()
																);
												}
								}

								protected void Decay(float multiplier = 1f) {
												Random r = new Random();
												for(int x = 0; x < width;x++) {
																for(int y = 0; y < height;y++) {
																				tiles[x, y].energy -= 0.005f * multiplier;

																				if (tiles[x, y].energy < 0) {
																								tiles[x, y].energy = 0;
																				}

																				if (r.NextSingle() > 0.998f) {
																								world.ChangeTileType(x, y, TileType.air);
																								tiles[x, y].light_intensity = (y + MathF.Abs(width / 2 - x)) / (float)(height + width / 2);
																				}
																}
												}
								}
								private float[,] BlurKernel = new float[3, 3] {
												{1, 2, 1},
												{2,	4, 2},
												{1, 2, 1}
								};
								private float BlurKernelSum;
								private void EditKernel(Func<float, float> func = null) {
												if (func == null) {
																func = (float value) => { return MathF.Pow(value, 2); };
												}
												BlurKernelSum = 0f;
												for(int x=0; x < 3; x++) {
																for(int y=0; y < 3; y++) {
																				BlurKernel[x, y] = func(BlurKernel[x, y]);
																				BlurKernelSum += BlurKernel[x, y];
																}
												}
								}

								private void RecalculateKernelSum() {
												BlurKernelSum = 0f;
												for (int x = 0; x < 3; x++) {
																for (int y = 0; y < 3; y++) {
																				BlurKernelSum += BlurKernel[x, y];
																}
												}
								}
								protected void Diffuse(float multiplier = 1f) {
												for (int i = 0; i < 4; i++) {
																int x_offset = 0;
																int y_offset = 0;

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

																for (int x = x_offset; x < width; x += 2) {
																				for (int y = y_offset; y < height; y += 2) {
																								float kernelSum = 0;
																								foreach (Tile tile in world.SurroundingTiles(x, y, 3, 3)) {
																												kernelSum += BlurKernel[2 - (tile.y - y + 1), tile.x - x + 1] * tile.energy;
																								}
																								kernelSum /= BlurKernelSum;
																								float diffuseWeight = 10f * 1/60 * multiplier;

																								float original = tiles[x, y].energy;

																								kernelSum = original * (1 - diffuseWeight) + kernelSum * (diffuseWeight);
																								tiles[x, y].energy = kernelSum;
																				}
																}
												}
								}

								private Tile TileFromVector(Vector2 pos) {
												if (!world._rect.Contains(pos)) {
																return new AirTile(0, 0);
												}
												return tiles[(int)pos.X, (int)pos.Y];
								}

								private float Sense(Particle particle) {
												Tile forward = TileFromVector(
																new Vector2(
																				MathF.Cos(particle.angle),
																				MathF.Sin(particle.angle)
																)*2.5f + particle.pos
												);
												Tile left = TileFromVector(
																new Vector2(
																				MathF.Cos(particle.angle - MathF.PI / 6),
																				MathF.Sin(particle.angle - MathF.PI / 6)
																)*2.5f + particle.pos
												);
												Tile right = TileFromVector(
																new Vector2(
																				MathF.Cos(particle.angle + MathF.PI / 8),
																				MathF.Sin(particle.angle + MathF.PI / 8)
																)*2.5f + particle.pos
												);

												// tendencies to go in respective directions
												float turnLeft  = left.energy    + left.light_intensity    ;
												float goForward = forward.energy + forward.light_intensity ;
												float turnRight = right.energy   + right.light_intensity   ;

												// determine which tile has most energy\
												// dont touch; it works
												if (turnLeft > goForward) {
																if(turnLeft > turnRight) {
																				return -1;// left.energy;
																}
																else if (turnLeft == turnRight) {
																				return 0;
																}
																return 1;// right.energy;
												}
												if (turnRight > goForward) {
																return 1;//right.energy;
												}

												return 0;
												
								}

								public override void Update(float deltaTime) {
												Random r	= new Random();

												// Decay trails
												
												

												for(int i=0; i<particleCount; i++) {
																particles[i].TTL -= r.NextSingle();

																Particle particle = particles[i];

																if(particle.TTL <= 0 ) {
																				particles[i].TTL = 120;
																				particles[i].pos = new Vector2(width/2, 0);
																				particles[i].angle = MathF.PI/2 + (r.NextSingle()-0.5f)/4;
																				continue;
																}

																// screen coordinates
																int x = (int)particle.pos.X;
																int y = (int)particle.pos.Y;

																Tile tile = tiles[x, y];

																if (tile.type == TileType.air) {
																				tile.energy += 1 * deltaTime;
																				tile.energy = MathF.Min(tile.energy, 1);
																}
																if (tile.energy >= 0.3) {
																				WoodTile wood = new WoodTile(x, y);
																				wood.light_intensity = tile.light_intensity;
																				tile = wood;
																				tiles[x, y] = tile;
																}
																if(tile.type == TileType.wood) {
																				particles[i].TTL +=  0.1f;
																				tile.light_intensity = 100;
																				tile.energy = 0;
																				
																}

																// Steer towards dense energy
																particle.angle += Sense(particle) * MathF.PI / 20 * (r.NextSingle()/1.1f+(1-1/1.1f) - (particle.TTL % 15) / 25f);
																
																// delta pos
																Vector2 direction = new Vector2(MathF.Cos(particle.angle), MathF.Sin(particle.angle))*particle.speed;

																// if on edge, randomise angle
																if (!world._rect.Contains(particle.pos+direction)){
																				particles[i].angle = r.NextSingle()*MathF.PI*2;
																				continue;
																}

																// move particle
																particles[i].angle = particle.angle;
																particles[i].pos += direction;
												}

												Diffuse(1);
												Decay(0.5f);
								}
				}
}

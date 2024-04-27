using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlimeyTrees.Core.Behaviour;
using SlimeyTrees.Core.Behaviour.SlimeParticle;
using SlimeyTrees.Core.Behaviour.WorldManagers;
using SlimeyTrees.Core.Static.World;
using System;
using System.Diagnostics;
using System.Linq;

namespace SlimeyTrees {
				public class Game1 : Game {
								private GraphicsDeviceManager _graphics;
								private SpriteBatch _spriteBatch;

								private World world;
								private SlimeWorldManager worldManager;

								public Game1() {
												_graphics = new GraphicsDeviceManager(this);
												Content.RootDirectory = "Content";
												IsMouseVisible = true;
								}

								protected override void Initialize() {
												// TODO: Add your initialization logic here
												Random r = new Random();

												world = new World(256, 256);
												worldManager = new DefaultSlimeMoldManager(world);
												worldManager.Init();
												

												base.Initialize();
								}

								protected override void LoadContent() {
												_spriteBatch = new SpriteBatch(GraphicsDevice);

												// TODO: use this.Content to load your game content here
								}
								 
								protected override void Update(GameTime gameTime) {
												if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
																Exit();
												float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

												KeyboardState ks = Keyboard.GetState();
												MouseState ms = Mouse.GetState();


												Random r = new Random();

												if (!ks.IsKeyDown(Keys.Space)) {
																worldManager.Update(deltaTime);
												}

												if(ms.LeftButton == ButtonState.Pressed) {
																Point world_scaled_position = new Point(
																				(int)(ms.Position.X * (world.width / 450f)),
																				(int)(ms.Position.Y * (world.width / 450f))
																);
																int x = world_scaled_position.X;
																int y = world.height - world_scaled_position.Y;

																if (world._rect.Contains(world_scaled_position)) {
																				foreach(Tile tile in world.SurroundingTiles(x, y, 3, 3)) {
																								//tile.obstacles = 1;
																				}
																}
												}

												// TODO: Add your update logic here
												//trainer.Update(deltaTime);

												base.Update(gameTime);
								}

								protected override void Draw(GameTime gameTime) {
												GraphicsDevice.Clear(Color.CornflowerBlue);

												_spriteBatch.Begin();

												// draw energy fields
												_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => Color.Brown * tile.wood * 1f
																),
																new Rectangle(0, 0, 450, 450),
																Color.White
												); ;

												// draw light intensity
												_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => Color.LightGoldenrodYellow * tile.light
																),
																new Rectangle(0, 0, 450, 450),
																Color.White
												);

												// draw tiles
												_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => tile.color
																),
																new Rectangle(0, 0, 450, 450),
																Color.White
												);

												_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => tile.leaves / 2f * Color.Green
																),
																new Rectangle(0, 0, 450, 450),
																Color.White
												);

												_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => tile.obstacles * Color.Gray
																),
																new Rectangle(0, 0, 450, 450),
																Color.White
												);

												/*_spriteBatch.Draw(
																worldManager.ParticlesTexture(GraphicsDevice),
																new Rectangle(0, 0, 450, 450),
																Color.White
												);*/


												_spriteBatch.End();

												base.Draw(gameTime);
								}
				}
}
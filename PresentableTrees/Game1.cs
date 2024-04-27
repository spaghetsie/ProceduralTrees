using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PresentableTrees.Core.Behaviour;
using PresentableTrees.Core.Behaviour.Mutators;
using PresentableTrees.Core.Behaviour.Mutators.KernelMutators.cs;
using PresentableTrees.Core.Behaviour.Trainers;
using PresentableTrees.Core.Behaviour.WorldManagers;
using PresentableTrees.Core.Behaviour.WorldManagers.KernelLike;
using PresentableTrees.Core.Behaviour.WorldManagers.SlimeMoldLike;
using PresentableTrees.Core.Static.World;
using System;
using System.Diagnostics;
using System.Linq;

namespace PresentableTrees {
				public class Game1 : Game {
								private GraphicsDeviceManager _graphics;
								private SpriteBatch _spriteBatch;

								private World world;
								private WorldManager worldManager;

								private Trainer trainer;
								private KernelManagerMutator managerMutator;

								public Game1() {
												_graphics = new GraphicsDeviceManager(this);
												Content.RootDirectory = "Content";
												IsMouseVisible = true;
								}

								protected override void Initialize() {
												// TODO: Add your initialization logic here

												world = new World(256, 256);
												/*worldManager = new DefaultKernelWorldManager(world, new float[,] {
																{ -3f, -1, -3f },
																{ -3f, 0, -3f },
																{ 0.5f, -1, 0.5f }
												});*/
												//worldManager.Init();

												managerMutator = new DefaultKernelManagerMutator(MutationChance: 0.01f, MutationIntensity: 0.0001f);
												base.Initialize();

												trainer = new KernelTrainer(9, managerMutator);
												trainer.Init();
								}

								protected override void LoadContent() {
												_spriteBatch = new SpriteBatch(GraphicsDevice);

												// TODO: use this.Content to load your game content here
								}

								private bool PressedLastFrame = false;
								private int gen = 0;
								bool frozen = true;
								protected override void Update(GameTime gameTime) {
												if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
																Exit();
												float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

												KeyboardState kw = Keyboard.GetState();
												Keys[] pressedKeys = kw.GetPressedKeys();

												if(kw.IsKeyDown(Keys.Space)) {
																frozen = false;
												}

												if(!frozen) {
																//worldManager.Update(deltaTime);
																trainer.Update(deltaTime);
												}

												if (!PressedLastFrame && pressedKeys.Length > 0) {
																int selected = (int)pressedKeys.First() - 49; // translate key press index (49 - 58) to tree index (0 - 8)
																if ( selected >= 0 && selected < 10) {
																				trainer.RepopulateSuperior(selected);
																				gen++;
																				Debug.WriteLine(gen);
																				PressedLastFrame = true;
																}
																
												}
												if(pressedKeys.Length == 0) {
																PressedLastFrame = false;
												}



												// TODO: Add your update logic here
												

												base.Update(gameTime);
								}

								protected override void Draw(GameTime gameTime) {
												GraphicsDevice.Clear(Color.CornflowerBlue);

												_spriteBatch.Begin();

												// draw energy fields
												/*_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => Color.Green * tile.energy * 1f
																),
																new Rectangle(0, 0, 450, 450),
																Color.White
												); ;

												// draw light intensity
												_spriteBatch.Draw(
																world.Texture2D(
																				GraphicsDevice,
																				(Tile tile) => Color.LightGoldenrodYellow * tile.light_intensity
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
												);*/

												trainer.Draw(_spriteBatch, GraphicsDevice);

												_spriteBatch.End();

												base.Draw(gameTime);
								}
				}
}
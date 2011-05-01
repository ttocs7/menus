using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace menus
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Menu menu;

        public enum gameState { MAIN_MENU, IN_PROGRESS, PAUSED };
        gameState state = gameState.IN_PROGRESS;

        //keyboard stuff
        Input input;

        List<string> pausedItems;
        List<string> mainMenuItems;

        int returnVal;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            input = new Input();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            

            font = Content.Load<SpriteFont>("SpriteFont1");

            //menu = new Menu(this);
            //normally I'd initialize file I/O, and read these from a file
            pausedItems = new List<string>();
            pausedItems.Add("PAUSED");
            pausedItems.Add("Continue Game");
            pausedItems.Add("Exit Game");

            mainMenuItems = new List<string>();
            mainMenuItems.Add("A Game of Blocks and Lines");
            mainMenuItems.Add("New Game");
            mainMenuItems.Add("Exit");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            KeyboardState newState = Keyboard.GetState();

            input.UpdateWithNewState(newState, gameTime);

            if (input.WasKeyPressed(Keys.Escape, false))
            {
                if (state == gameState.IN_PROGRESS)
                {
                    state = gameState.PAUSED;
                    menu = new Menu(this, pausedItems);
                }
            }

            switch (state)
            {
                case gameState.PAUSED:
                    returnVal = menu.Update(gameTime);
                    if (returnVal != 0)
                    {
                        state = gameState.IN_PROGRESS;
                        menu = null;
                    }
                    break;
                case gameState.MAIN_MENU:
                    //NYI:  main menu functionality
                    if (menu.Update(gameTime) == 1)
                    {
                        state = gameState.IN_PROGRESS;
                        menu = null;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Game stuff goes here", new Vector2(200, 200), Color.Green);
            if (returnVal != 0)
            {
                spriteBatch.DrawString(font, "The menu returned: " + returnVal, new Vector2(200, 300), Color.Orange);
            }

            spriteBatch.End();

            if (state == gameState.PAUSED)
            {
                menu.Draw(gameTime);
            }


            base.Draw(gameTime);
        }
    }
}

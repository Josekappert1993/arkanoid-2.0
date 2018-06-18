using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Arkanoid_3.Controls;

namespace Arkanoid_3.States
{
    public class MenuState : State
    {
        private List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice grapicsdevice, ContentManager content) : base(game, grapicsdevice, content)
        {
            var buttontexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var startGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 210),
                Text = "Start Game",
            };
            startGameButton.Click += StartGameButton_Click;

            var optionGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 350),
                Text = "Options",
            };
            optionGameButton.Click += OptionGameButton_Click;

            var creditGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 500),
                Text = "Credits",
            };
            creditGameButton.Click += CreditGameButton_Click;

            var quitGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 650),
                Text = "Quit Game",
            };
            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                startGameButton,
                optionGameButton,
                creditGameButton,
                quitGameButton,
            };
        }

        private void OptionGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new OptionState(_game, _graphicsDevice, _content));
        }

        private void CreditGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CreditState(_game, _graphicsDevice, _content));
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();

            foreach (var component in _components)
                component.Draw(gametime, spritebatch);

            spritebatch.End();
        }
        
        public override void Update(GameTime gametime)
        {
            foreach (var component in _components)
                component.Update(gametime);
        }
    }
}

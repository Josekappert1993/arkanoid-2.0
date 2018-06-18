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
    public class CreditState : State
    {
        private List<Component> _components;

        public CreditState(Game1 game, GraphicsDevice grapicsdevice, ContentManager content) : base(game, grapicsdevice, content)
        {
            var buttontexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var creditFont = _content.Load<SpriteFont>("Fonts/creditFont");

            var creditGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 8),
                Text = "Credits",
            };
            creditGameButton.Click += NameGameButton_Click;

            var dennisGameButton = new Button(buttontexture, creditFont)
            {
                Position = new Vector2(800, 140),
                Text = "Dennis",
            };
            dennisGameButton.Click += NameGameButton_Click;

            var rianneGameButton = new Button(buttontexture, creditFont)
            {
                Position = new Vector2(800, 260),
                Text = "Rianne",
            };
            rianneGameButton.Click += NameGameButton_Click;

            var sonerGameButton = new Button(buttontexture, creditFont)
            {
                Position = new Vector2(800, 380),
                Text = "Soner",
            };
            sonerGameButton.Click += NameGameButton_Click;

            var thierryGameButton = new Button(buttontexture, creditFont)
            {
                Position = new Vector2(800, 500),
                Text = "Thierry",
            };
            thierryGameButton.Click += NameGameButton_Click;

            var joseGameButton = new Button(buttontexture, creditFont)
            {
                Position = new Vector2(800, 620),
                Text = "Jose",
            };
            joseGameButton.Click += NameGameButton_Click;

            var justinGameButton = new Button(buttontexture, creditFont)
            {
                Position = new Vector2(800, 740),
                Text = "Justin",
            };
            justinGameButton.Click += NameGameButton_Click;

            var backGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 880),
                Text = "Back",
            };
            backGameButton.Click += BackGameButton_Click;

            _components = new List<Component>()
            {
                creditGameButton,
                dennisGameButton,
                rianneGameButton,
                sonerGameButton,
                thierryGameButton,
                joseGameButton,
                justinGameButton,
                backGameButton,
            };
        }

        private void BackGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }
        private void NameGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CreditState(_game, _graphicsDevice, _content));
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

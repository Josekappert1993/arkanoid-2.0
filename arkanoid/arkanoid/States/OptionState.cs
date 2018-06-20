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
    public class OptionState : State
    {
        private List<Component> _components;

        public OptionState(Game1 game, GraphicsDevice grapicsdevice, ContentManager content) : base(game, grapicsdevice, content)
        {
            var buttontexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var backGameButton = new Button(buttontexture, buttonFont)
            {
                Position = new Vector2(800, 650),
                Text = "Back",
            };
            backGameButton.Click += BackGameButton_Click;

            _components = new List<Component>()
            {
                backGameButton,
            };
        }

        private void BackGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
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

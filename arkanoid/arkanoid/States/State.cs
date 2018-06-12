using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid_3.States
{
    public abstract class State
    {
        #region Fields
        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected Game1 _game;

        #endregion
        #region Methods

        public abstract void Draw(GameTime gametime, SpriteBatch spritebatch);
        public abstract void Update(GameTime gametime);
        public State(Game1 game, GraphicsDevice grapicsdevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = grapicsdevice;
            _content = content;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arkanoid_3.States
{
    public class GameState : State
    {
        public GameState(Game1 game, GraphicsDevice grapicsdevice, ContentManager content) : base(game, grapicsdevice, content)
        {
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }
    }
}

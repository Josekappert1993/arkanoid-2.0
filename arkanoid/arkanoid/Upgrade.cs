using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid_3
{
    public enum Color { Black, Yellow, Blue, Green, Purple };
    public abstract class Upgrade
    {
        private int speed;
        private int size;
        Color _color;
    }
}

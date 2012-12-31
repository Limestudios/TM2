using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TM2
{
    public class GameScreen
    {
        public virtual void Initialize() { }
        public virtual void LoadContent(ContentManager Content) { }
        public virtual void Update(GameTime gametime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 centre;

        public Camera(Viewport newView)
        {
            view  = newView;
        }

        public void Update(GameTime gametime, Animation.MobileSprite character)
        {
            centre = new Vector2(character.Position.X + (character.BoundingBox.Width / 2) - 400, 
                                    character.Position.Y + character.BoundingBox.Height /2  -250);
            transform = Matrix.CreateScale(new Vector3 (1, 1, 0)) * 
                Matrix.CreateTranslation(new Vector3 (-centre.X, -centre.Y, 0));
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Graphics
{
    public class SystemSprite : Component
    {
        public SpriteType SpriteType;
        public int Priority;
        public float PixelSize;
        public string Label;
        public Color Color = Color.White;



        public SystemSprite(Entity entity) : base(entity)
        {

        }
    }


    public enum SpriteType
    {
        None,
        Body,
        Vessel,
        Misc,
    }
}

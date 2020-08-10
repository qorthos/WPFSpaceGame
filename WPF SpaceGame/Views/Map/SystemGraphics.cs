using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPFSpaceGame.General;
using System.Linq;
using System.IO;
using WPFSpaceGame.Game.Systems;
using WPFSpaceGame.Views;
using WPFSpaceGame.Game;
using WPFSpaceGame.Game.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using System.Net;

namespace WPFSpaceGame.Views.Map
{
    public class SystemGraphics : MonoGameItem
    {
        ServiceProvider serviceProvider;
        GameService gameService;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        // monogame debug thingy
        private Texture2D monoGameTexture;

        private Texture2D circleTexture;
        private Texture2D leaderATexture;
        private Texture2D leaderBTexture;

        public Double2 Offset { get; set; }
        public double Scale { get; set; }


        protected GameData GameData
        {
            get { return gameService.GameData; }
        }

        protected MapData MapData
        {
            get { return ItemData as MapData; }
        }

        protected OrbitalBody SelectedBody { get; set; }


        public SystemGraphics()
        {
            serviceProvider = ServiceProvider.Instance;
            gameService = serviceProvider.GetService<GameService>();
            gameService.NewGameDataEvent += GameService_NewGameDataEvent;
            gameService.GameTickFinishedEvent += GameService_GameTickFinishedEvent;
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            monoGameTexture = Content.Load<Texture2D>("monogame-logo");
            circleTexture = Content.Load<Texture2D>("circle");
            leaderATexture = Content.Load<Texture2D>("leaderA");
            leaderBTexture = Content.Load<Texture2D>("leaderB");
            spriteFont = Content.Load<SpriteFont>("label");


            //FileStream fileStream = new FileStream("Content/circle.png", FileMode.Open);
            //circle_Texture = Texture2D.FromStream(GraphicsDevice, fileStream);
            //fileStream.Dispose();
        }

        private void GameService_NewGameDataEvent(GameService service)
        {

        }

        private void GameService_GameTickFinishedEvent(GameService service)
        {
            SelectedBody = null; // kind of a hack to get us back to the right view...
        }

        public override void Update(GameTime gameTime)
        { 

            if (MapData.SelectedBody != SelectedBody)
            {
                if (SelectedBody == null)
                {
                    SelectedBody = MapData.SelectedBody;
                    Offset = SelectedBody.GlobalPosition;
                    Scale = DoubleHelper.Min(
                        0.45 * GraphicsDevice.Viewport.Width / SelectedBody.BoundsRadius,
                        0.45 * GraphicsDevice.Viewport.Height / SelectedBody.BoundsRadius);
                }
                else
                {
                    SelectedBody = MapData.SelectedBody;

                    var newOffset = SelectedBody.GlobalPosition;
                    var newScale = DoubleHelper.Min(
                        0.45 * GraphicsDevice.Viewport.Width / SelectedBody.BoundsRadius,
                        0.45 * GraphicsDevice.Viewport.Height / SelectedBody.BoundsRadius);

                    Tweener.CancelAll();

                    //if we're zooming in move to the new object THEN zoom in
                    //if we're zooming out, zoom out first then move
                    if (newScale > Scale)
                    {
                        Tweener.TweenTo<SystemGraphics, Double2>(this, a => ((SystemGraphics)a).Offset, newOffset, 0.25, 0.0)
                        .Easing(EasingFunctions.QuadraticInOut);

                        Tweener.TweenTo<SystemGraphics, double>(this, a => ((SystemGraphics)a).Scale, newScale, 0.25, 0.25)
                            .Easing(EasingFunctions.QuadraticInOut);
                    }
                    else
                    {
                        Tweener.TweenTo<SystemGraphics, Double2>(this, a => ((SystemGraphics)a).Offset, newOffset, 0.25, 0.25)
                            .Easing(EasingFunctions.QuadraticInOut);

                        Tweener.TweenTo<SystemGraphics, double>(this, a => ((SystemGraphics)a).Scale, newScale, 0.25, 0.0)
                            .Easing(EasingFunctions.QuadraticInOut);
                    }
                    
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);
            spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
            if (MapData.SelectedSystem != null)
            {
                foreach (OrbitalBody body in MapData.SelectedSystem.Children)
                {
                    if (body.BodyClassification == BodyClassification.Vessel)
                        DrawShip(body);
                    else
                        DrawBody(body);
                }
            }
            spriteBatch.End();
        }

        private void DrawBody(OrbitalBody body)
        {           
            var sprite = body.GetComponent<SystemSprite>();

            // the circle texture is 128 pixels.
            // by default, we want the objects drawn on screen to be 8 pixels if we're zoomed out.
            // if the zoom level results in the object being more than 8 pixels, then draw it that size.
            float spriteScale = sprite.PixelSize / 128f;
            float outlineScale = (2 + sprite.PixelSize) / 128f;

            var screenX = (body.GlobalPosition.X - Offset.X) * Scale + GraphicsDevice.Viewport.Bounds.Width / 2f;
            var screenY = (body.GlobalPosition.Y - Offset.Y) * Scale + GraphicsDevice.Viewport.Bounds.Height / 2f;
            var screenPos = new Vector2((float)screenX, (float)screenY);
            var halfSize = new Vector2(sprite.PixelSize / 2f, sprite.PixelSize / 2f);
            var outlineSize = new Vector2(sprite.PixelSize / 2f + 1, sprite.PixelSize / 2f + 1);

            // object circle
            spriteBatch.Draw(
                circleTexture,
                screenPos - halfSize,
                null,
                sprite.Color,
                0,
                Vector2.Zero,
                spriteScale,
                SpriteEffects.None, 1f - (int)body.BodyClassification / 10f);

            spriteBatch.Draw(
                circleTexture,
                screenPos - outlineSize,
                null,
                Color.Black,
                0,
                Vector2.Zero,
                outlineScale,
                SpriteEffects.None, 0.9f - (int)body.BodyClassification / 10f);

            // get some parent info            
            bool drawText = true; // (we don't draw this if we're too close to our parent)
            if (body.Parent != null)
            {
                var parentScreenX = (body.Parent.GlobalPosition.X - Offset.X) * Scale + GraphicsDevice.Viewport.Bounds.Width / 2.0;
                var parentScreenY = (body.Parent.GlobalPosition.Y - Offset.Y) * Scale + GraphicsDevice.Viewport.Bounds.Height / 2.0;
                var parentScreenPos = new Vector2((float)parentScreenX, (float)parentScreenY);

                var distance = Vector2.Distance(screenPos, parentScreenPos);

                if (distance < 16)
                    drawText = false;

                var screenRadius = (float)(body.Orbital_Radius * Scale);
                int numSides = 128;
                int sideLength = (int)(2 * Math.PI * screenRadius / numSides);
                if (sideLength > 128)
                {
                    numSides *= sideLength / 128;
                    numSides = DoubleHelper.Clamp(numSides, 128, 1024);

                    // fake in a circlular path by just drawing a line
                    Vector2 direction = screenPos - parentScreenPos;
                    direction.Normalize();
                    direction = direction.Rotate(MathHelper.PiOver2);

                    spriteBatch.DrawLine(screenPos + direction * 1000, screenPos - direction * 1000, Color.LightGray, 1, 0);
                }
                else
                {
                    // draw our orbit line as a circle
                    spriteBatch.DrawCircle(parentScreenPos, screenRadius, numSides, Color.LightGray, 1, 0);
                }
            }

            if (drawText)
            {
                DrawAngledLeader(screenPos, sprite.Label);
            }
        }

        private void DrawShip(OrbitalBody body)
        {
            var sprite = body.GetComponent<SystemSprite>();

            // the circle texture is 128 pixels.
            // by default, we want the objects drawn on screen to be 8 pixels if we're zoomed out.
            // if the zoom level results in the object being more than 8 pixels, then draw it that size.
            float spriteScale = sprite.PixelSize / 128f;
            float outlineScale = (2 + sprite.PixelSize) / 128f;

            var screenPos = GetScreenPos(body.GlobalPosition);
            var halfSize = new Vector2(sprite.PixelSize / 2f, sprite.PixelSize / 2f);
            var outlineSize = new Vector2(sprite.PixelSize / 2f + 1, sprite.PixelSize / 2f + 1);

            // object circle
            spriteBatch.Draw(
                circleTexture,
                screenPos - halfSize,
                null,
                sprite.Color,
                0,
                Vector2.Zero,
                spriteScale,
                SpriteEffects.None, 1f - (int)body.BodyClassification / 10f);

            spriteBatch.Draw(
                circleTexture,
                screenPos - outlineSize,
                null,
                Color.Black,
                0,
                Vector2.Zero,
                outlineScale,
                SpriteEffects.None, 0.9f - (int)body.BodyClassification / 10f);


            DrawStraightLeader(screenPos, sprite.Label);

            var vessel = body.GetComponent<Vessel>();
            if (vessel.NavRoute != null)
            {
                if (vessel.NavRoute.PredictedPositions.Count <= 1)
                    return;

                int startIndex = (int)((GameData.CurrentDate - vessel.NavRoute.StartDate).TotalSeconds / vessel.NavRoute.TotalTime * vessel.NavRoute.PredictedPositions.Count);

                for (int i = startIndex; i < vessel.NavRoute.PredictedPositions.Count - 1; i++)
                {
                    screenPos = GetScreenPos(vessel.NavRoute.PredictedPositions[i]);
                    var screenPos2 = GetScreenPos(vessel.NavRoute.PredictedPositions[i + 1]);

                    spriteBatch.DrawLine(screenPos, screenPos2, Color.White, 1, 1);
                }
            }
        }

        private void DrawAngledLeader(Vector2 screenPos, string label)
        {
            // leader
            spriteBatch.Draw(leaderATexture,
                screenPos - new Vector2(0, leaderATexture.Height),
                null,
                Color.White,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None, 0);

            spriteBatch.Draw(leaderBTexture,
                screenPos - new Vector2(-leaderATexture.Width, leaderATexture.Height),
                null,
                Color.White,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, label, screenPos + new Vector2(leaderATexture.Width + 1, -leaderATexture.Height + 1), 
                Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, label, screenPos + new Vector2(leaderATexture.Width + 0, -leaderATexture.Height + 0), 
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.01f);
        }

        private void DrawStraightLeader(Vector2 screenPos, string label)
        {
            spriteBatch.Draw(leaderBTexture,
                screenPos - new Vector2(-leaderATexture.Width / 2f, leaderATexture.Height / 2f),
                null,
                Color.White,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, label, screenPos + new Vector2(leaderATexture.Width + 1, -leaderATexture.Height/2f + 1), 
                Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, label, screenPos + new Vector2(leaderATexture.Width + 0, -leaderATexture.Height/2f + 0), 
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.01f);
        }

        private Vector2 GetScreenPos(Double2 worldPos)
        {
            return new Vector2(
                (float)((worldPos.X - Offset.X) * Scale + GraphicsDevice.Viewport.Bounds.Width / 2.0),
                (float)((worldPos.Y - Offset.Y) * Scale + GraphicsDevice.Viewport.Bounds.Height / 2.0));
        }
    }
}
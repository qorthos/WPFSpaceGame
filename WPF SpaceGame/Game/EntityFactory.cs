using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.Game.Graphics;
using WPFSpaceGame.Game.Systems;
using WPFSpaceGame.General;
using Microsoft.Xna.Framework;

namespace WPFSpaceGame.Game
{
    public static class EntityFactory
    {
        private static GameService gameService;
        public static GameData GameData
        {
            get 
            {
                if (gameService == null)
                    gameService = ServiceProvider.Instance.GetService<GameService>();
                return gameService.GameData;                    
            }
        }

        public static Entity CreateBody(StellarSystem stellarSystem, BodyClassification bodyClassification = BodyClassification.Star, OrbitalBody parent = null, double mass = 1000, 
            double orbital_radius = 1000, string name = "NewBody")
        {
            Entity entity = new Entity();

            var body = new OrbitalBody(entity) 
            {
                Name = name,
                StellarSystem = stellarSystem,
                BodyClassification = bodyClassification,
                Mass = mass,
                Orbital_Radius = orbital_radius,
                Parent = parent,
            };
            

            var sysSprite = new SystemSprite(entity)
            {
                SpriteType = SpriteType.Body,
                Label = name,
                Priority = 10 - (int)bodyClassification,
                Color = Color.White,
                PixelSize = 8f,
            };

            switch (bodyClassification)
            {
                case BodyClassification.Star:
                    sysSprite.Color = Color.Yellow;
                    sysSprite.PixelSize = 16f;
                    break;
            }

            GameData.AddEntity(entity);
            return entity;
        }


        public static Entity CreatePlanet()
        {
            throw new NotImplementedException();
        }


        public static Entity CreateSystem(string name)
        {
            Entity entity = new Entity();

            _ = new StellarSystem(entity)
            {
                Name = name,
            };

            GameData.AddEntity(entity);
            return entity;
        }


        public static Entity CreateVessel(StellarSystem stellarSystem, OrbitalBody parent, bool isLockedToParent, string name)
        {
            Entity entity = new Entity();

            var body = new OrbitalBody(entity)
            {
                Name = name,
                StellarSystem = stellarSystem,
                BodyClassification = BodyClassification.Vessel,
                Orbital_Radius = 0,
                Orbital_Period = 1,
                LocalVelocity = Double2.Zero,
                Parent = parent,
                IsLockedToParent = isLockedToParent,
            };


            var sysSprite = new SystemSprite(entity)
            {
                SpriteType = SpriteType.Body,
                Label = name,
                Priority = 10 - (int)BodyClassification.Vessel,
                Color = Color.CornflowerBlue,
                PixelSize = 8f,
            };

            var ship = new Vessel(entity)
            {
                Thrust_G = 0,
                MaxThrust_G = 1,
            };

            GameData.AddEntity(entity);
            return entity;
        }


        public static Entity CreateFaction(string tag, bool isPlayer, string startingSystemName, string startingBodyName)
        {
            Entity entity = new Entity();
            var bodies = GameData.GetComponents<OrbitalBody>();
            var stellarSystems = GameData.GetComponents<StellarSystem>();

            var startingSystem = stellarSystems.First(x => x.Name == startingSystemName);
            var startingBody = bodies.First(x => x.Name == startingBodyName);

            _ = new Faction(entity)
            {
                Tag = tag,
                IsPlayer = isPlayer,
                StartingSystem = startingSystem,
                StartingBody = startingBody,
            };

            GameData.AddEntity(entity);
            return entity;
        }
    }
}

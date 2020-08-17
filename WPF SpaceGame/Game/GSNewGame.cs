using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.Game.Graphics;
using WPFSpaceGame.Game.Politics;
using WPFSpaceGame.Game.Systems;

namespace WPFSpaceGame.Game
{
    public class GSNewGame : GameSystem
    {
        protected override void GameService_NewGameDataEvent(GameService service)
        {

            // create sol system
            var solSystem = EntityFactory.CreateSystem("New Sol System").GetComponent<StellarSystem>();

            // create a star
            var sol = EntityFactory.CreatePlanetoid(solSystem, BodyClassification.Star, null,
                1.989 * Math.Pow(10, 30),
                1,
                "New Sol");

            // create some planets
            var newEarth = EntityFactory.CreatePlanetoid(solSystem, BodyClassification.Planet, sol.GetComponent<OrbitalBody>(),
                6.0 * Math.Pow(10, 24),
                140 * 1000 * 1000, "New Earth");

            var newEarthPlanetoid = newEarth.GetComponent<Planetoid>();
            

                var uma = EntityFactory.CreatePlanetoid(solSystem, BodyClassification.Moon, newEarth.GetComponent<OrbitalBody>(),
                    4 * Math.Pow(10, 22),
                    350 * 1000, "Uma");

            var delos = EntityFactory.CreatePlanetoid(solSystem, BodyClassification.Planet, sol.GetComponent<OrbitalBody>(),
                2.5 * Math.Pow(10, 24),
                230 * 1000 * 1000, "Delos");

                var maggie = EntityFactory.CreatePlanetoid(solSystem, BodyClassification.Moon, delos.GetComponent<OrbitalBody>(),
                    2 * Math.Pow(10, 22),
                    400 * 1000, "Maggie");

            // create player faction
            EntityFactory.CreateFaction("PLA", true, "New Sol System", "New Earth");

            // create a test ship
            var ship = EntityFactory.CreateVessel(solSystem, newEarth.GetComponent<OrbitalBody>(), true, "USN Jebediah Kerman");
        }
    }
}

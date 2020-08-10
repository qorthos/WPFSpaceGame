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
            var solSystem = EntityFactory.CreateSystem("Sol System").GetComponent<StellarSystem>();

            // create a star
            var sol = EntityFactory.CreateBody(solSystem, BodyClassification.Star, null,
                1.989 * Math.Pow(10, 30),
                1,
                "Sol");

            // create some planets
            var earth = EntityFactory.CreateBody(solSystem, BodyClassification.Planet, sol.GetComponent<OrbitalBody>(),
                5.972 * Math.Pow(10, 24),
                151.78 * 1000 * 1000, "Earth");

            var mars = EntityFactory.CreateBody(solSystem, BodyClassification.Planet, sol.GetComponent<OrbitalBody>(),
                6.417 * Math.Pow(10, 23),
                228 * 1000 * 1000, "Mars");

            // create mun
            var moon = EntityFactory.CreateBody(solSystem, BodyClassification.Moon, earth.GetComponent<OrbitalBody>(),
                7.347 * Math.Pow(10, 22),
                384.4 * 1000, "Moon");

            // create player faction
            EntityFactory.CreateFaction("PLA", true, "Sol System", "Earth");

            // create a test ship
            var ship = EntityFactory.CreateVessel(solSystem, earth.GetComponent<OrbitalBody>(), true, "USN Jebediah Kerman");
            ship.GetComponent<Vessel>().DestinationTarget = sol.GetComponent<OrbitalBody>();
        }
    }
}

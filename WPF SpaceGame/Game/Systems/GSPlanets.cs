using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class GSPlanets : GameSystem
    {
        public GSPlanets()
        {
            AddComponentInterest<Planetoid>();
        }

        protected override void GameService_GameStartedEvent(GameService service)
        {
            // update everything

            base.GameService_GameStartedEvent(service);
        }

        public override void TickDay()
        {

            base.TickDay();
        }
    }
}

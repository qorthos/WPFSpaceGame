using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class Vessel : Component
    {
        private double thrust_G = 0;
        private double maxThrust_G = 1;
        private OrbitalBody destinationTarget;
        private NavRoute navRoute;

        // waypoint target
        // target distance
        // target relative velocity

        public double Thrust_G
        {
            get
            {
                return thrust_G;
            }

            set
            {
                thrust_G = value;
                Notify();
            }
        }

        public OrbitalBody DestinationTarget
        {
            get
            {
                return destinationTarget;
            }

            set
            {
                destinationTarget = value;
                Notify();
            }
        }

        public double MaxThrust_G
        {
            get
            {
                return maxThrust_G;
            }

            set
            {
                maxThrust_G = value;
                Notify();
            }
        }

        public double MaxThrust
        {
            get { return maxThrust_G * 9.81 / 1000.0f; }
        }

        public NavRoute NavRoute
        {
            get
            {
                return navRoute;
            }

            set
            {
                navRoute = value;
                Notify();
            }
        }

        public Vessel(Entity entity) : base(entity)
        {

        }
    }
}

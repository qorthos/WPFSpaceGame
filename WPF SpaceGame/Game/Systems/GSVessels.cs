using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class GSVessels : GameSystem
    {
        GSStellarBodies gsStellarBodies;


        public GSVessels()
        {
            AddComponentInterest<Vessel>();
        }

        public override void Initialize()
        {
            gsStellarBodies = GameService.GetGameSystem<GSStellarBodies>();
            base.Initialize();
        }

        protected override void GameService_NewGameDataEvent(GameService service)
        {
            base.GameService_NewGameDataEvent(service);
        }

        protected override void OnComponentPropertyChanged(Component component, string propertyName)
        {
            if (propertyName == nameof(Vessel.DestinationTarget))
                GetNavigation(component as Vessel);

            base.OnComponentPropertyChanged(component, propertyName);
        }


        public override void TickHour()
        {
            // update navigation
            var vessels = GameData.GetComponents<Vessel>();
            foreach (Vessel vessel in vessels)
            {
                if (vessel.NavRoute != null)
                {
                    if (vessel.NavRoute.EndDate <= GameData.CurrentDate)
                    {
                        vessel.NavRoute = null;
                        vessel.DestinationTarget = null;
                    }
                }

            }

            base.TickHour();
        }


        public override void TickDay()
        {
            base.TickDay();
        }



        private void GetNavigation(Vessel vessel)
        {
            if (vessel.DestinationTarget == null)
                return;

            // do the first estimate
            var vesselBody = vessel.GetComponent<OrbitalBody>();
            gsStellarBodies.GetBodyPositionAtTime(vesselBody, GameData.CurrentDate, out Double2 p_a, out Double2 v_a);
            gsStellarBodies.GetBodyPositionAtTime(vessel.DestinationTarget, GameData.CurrentDate, out Double2 p_b, out Double2 v_b);

            double t_est = Math.Sqrt(4 * (p_b - p_a).Length() / vessel.MaxThrust);
            Double2 primaryburn = 4 * (p_b - p_a - (v_a + v_b) * t_est / 2) / (t_est * t_est);
            Double2 normalizingBurn = (v_b - v_a) / t_est;

            NavRoute route = new NavRoute()
            {
                StartDate = GameData.CurrentDate,
                PrimaryBurn = primaryburn,
                NormalizingBurn = normalizingBurn,
                Eccentricity = 0.5,
                TotalTime = t_est,
            };

            RefinePath(vessel, vesselBody, ref route);
            route.EndDate = route.StartDate.AddSeconds(route.TotalTime);
            vessel.NavRoute = route;
            vesselBody.IsLockedToParent = false; // gotta detach from the parent to go.
            vesselBody.Parent = null;
            PredictRoute(vesselBody, route);
        }

        private void RefinePath(Vessel vessel, OrbitalBody vesselBody, ref NavRoute route)
        {
            var prevPrimaryBurn = route.PrimaryBurn;
            prevPrimaryBurn.Normalize();
            var prevNormalizingBurn = route.NormalizingBurn;
            prevNormalizingBurn.Normalize();
            if (prevNormalizingBurn.X == double.NaN)
                prevNormalizingBurn = Double2.Zero;

            gsStellarBodies.GetBodyPositionAtTime(vesselBody, GameData.CurrentDate, out Double2 p_a, out Double2 v_a);
            gsStellarBodies.GetBodyPositionAtTime(vessel.DestinationTarget, GameData.CurrentDate.AddSeconds(route.TotalTime), out Double2 p_b, out Double2 v_b); // there is some sort of error here

            var normalLength = route.NormalizingBurn.Length();
            var primaryLength = route.PrimaryBurn.Length();

            var dot = Double2.Dot(prevPrimaryBurn, prevNormalizingBurn);
            route.Eccentricity += dot / 2.0 * (normalLength / (primaryLength + normalLength)); // this might need some refining
            //route.Eccentricity = 0.5; // used this for some debugging
            var e = route.Eccentricity;
            var t = route.TotalTime;
            var t2 = t * t;

            route.PrimaryBurn = (p_b - p_a - v_a * t - route.NormalizingBurn * t2 / 2.0) / (t2 / 2.0 - Math.Pow(1 - e, 2.0) * t2);
            route.NormalizingBurn = (v_b - v_a - ((2 * e - 1) * t * route.PrimaryBurn)) / t;
            
            var burn1 = route.PrimaryBurn + route.NormalizingBurn;
            var burn2 = -route.PrimaryBurn + route.NormalizingBurn;

            var peakThrust = Math.Max(burn1.Length(), burn2.Length());

            var error = peakThrust / vessel.MaxThrust;
            route.CalcIterations++;
            if ((error > 1.0000001) || (error < 0.9999999))
            {
                route.TotalTime *= Math.Pow(error, 0.5);
                RefinePath(vessel, vesselBody, ref route);                
            }
            else
            {
                return; // fin.
            }

        }


        private void PredictRoute(OrbitalBody body, NavRoute route)
        {
            Double2 p = body.GlobalPosition;
            Double2 v = body.GlobalVelocity;
            double t = 3600;
            Double2 primaryBurn = route.PrimaryBurn + route.NormalizingBurn;
            Double2 brakingBurn = -route.PrimaryBurn + route.NormalizingBurn;

            var totalDv = body.GlobalVelocity + route.NormalizingBurn * route.TotalTime;
            var flipPoint = route.TotalTime * route.Eccentricity;

            route.PredictedPositions.Add(p);
            route.PredictedVelocities.Add(v);

            for (int i = 0; i < route.TotalTime / 3600.0; i++)
            {
                var elapsedTimeAtStart = (GameData.CurrentDate.AddSeconds(t * i) - route.StartDate).TotalSeconds;
                var elapsedTimeAtEnd = DoubleHelper.Clamp(elapsedTimeAtStart + t, elapsedTimeAtStart + t, route.TotalTime);
                var timeSpan = DoubleHelper.Clamp(elapsedTimeAtEnd - elapsedTimeAtStart, 0, route.TotalTime - elapsedTimeAtStart); // usually, but not always 3600
                

                var primaryBurnTime = flipPoint - elapsedTimeAtStart;
                primaryBurnTime = DoubleHelper.Clamp(primaryBurnTime, 0, timeSpan);
                var brakingBurnTime = elapsedTimeAtEnd - flipPoint;
                brakingBurnTime = DoubleHelper.Clamp(brakingBurnTime, 0, timeSpan);

                p += primaryBurn * Math.Pow(primaryBurnTime, 2) / 2.0 + v * primaryBurnTime;
                v += primaryBurn * primaryBurnTime;
                p += brakingBurn * Math.Pow(brakingBurnTime, 2) / 2.0 + v * brakingBurnTime;                
                v += brakingBurn * brakingBurnTime;

                route.PredictedPositions.Add(p);
                route.PredictedVelocities.Add(v);
            }
        }
    }
}

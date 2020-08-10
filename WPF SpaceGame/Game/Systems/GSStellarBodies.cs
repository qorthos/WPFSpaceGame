using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class GSStellarBodies : GameSystem
    {
        double galaxyElapsedSeconds = 0;
        DateTime galaxyStartDate;
        double g = 6.67 * Math.Pow(10, -20);
        double deltaT = 0f;

        public GSStellarBodies()
        {
            AddComponentInterest<OrbitalBody>();
        }

        protected override void GameService_NewGameDataEvent(GameService service)
        {
            galaxyStartDate = DateTime.MinValue;
            galaxyElapsedSeconds = (GameData.CurrentDate - galaxyStartDate).TotalSeconds;

            base.GameService_NewGameDataEvent(service);
        }

        protected override void GameService_GameStartedEvent(GameService service)
        {
            deltaT = 0;
            UpdateAllValues();
            deltaT = 3600;
        }

        protected override void OnComponentAdded(Component component)
        {
            if (component is OrbitalBody)
            {
                var body = component as OrbitalBody;

                UpdateBounds(body);
                UpdateOrbitalPeriod(body);
                UpdateBodyPositions(body);
            }

            base.OnComponentAdded(component);
        }

        public override void TickDay()
        {   
            base.TickDay();
        }

        public override void TickHour()
        {
            UpdateAllValues();
            base.TickHour();
        }

        private void UpdateAllValues()
        {
            galaxyElapsedSeconds = (GameData.CurrentDate - galaxyStartDate).TotalSeconds;
            var bodies = GameData.GetComponents<OrbitalBody>();

            // calculate everyone's orbital period
            foreach (OrbitalBody body in bodies)
            {
                UpdateOrbitalPeriod(body);
            }

            // update positions
            foreach (OrbitalBody body in bodies)
            {
                if (body.Parent == null) // ignore everything else so we don't repeat calcs.
                {
                    UpdateBodyPositions(body);
                }

            }

            // go through all bodies and determine their bounds from maximum child distance
            foreach (OrbitalBody body in bodies)
            {
                UpdateBounds(body);
            }
        }

        private void UpdateBounds(OrbitalBody body)
        {
            body.BoundsRadius = 0;
            foreach (OrbitalBody child in body.Children)
            {
                var childRadius = child.Orbital_Radius;
                if (childRadius > body.BoundsRadius)
                {
                    body.BoundsRadius = childRadius;
                }
            }
            if ((body.BoundsRadius == 0) && (body.Parent != null))
                body.BoundsRadius = body.Parent.BoundsRadius;

            if (body.BoundsRadius == 0)
            {
                var star = body.StellarSystem.Children.FirstOrDefault(x => x.BodyClassification == BodyClassification.Star);
                if (star != null)
                    body.BoundsRadius = star.BoundsRadius;
                else
                    body.BoundsRadius = 151000000;
            }

        }

        private void UpdateOrbitalPeriod(OrbitalBody body)
        {
            if (body.Parent == null)
            {
                body.Orbital_Period = 1;
            }
            else
            {
                body.Orbital_Period = 2 * Math.PI / (Math.Sqrt(g * body.Parent.Mass)) * Math.Pow(body.Orbital_Radius, 3.0 / 2.0);
            }
        }

        private void UpdateBodyPositions(OrbitalBody body)
        {
            if (body.IsLockedToParent == true)
            {
                body.Orbital_Period = 1;
                body.Orbital_Radius = 0;
                body.LocalVelocity = Double2.Zero;
                body.LocalPosition = Double2.Zero;
                body.GlobalPosition = body.Parent.GlobalPosition;
                body.GlobalVelocity = body.Parent.GlobalVelocity;
            }
            else if (body.BodyClassification == BodyClassification.Star)
            {
                // we stay at 0,0
            }            
            else if (body.BodyClassification != BodyClassification.Vessel)
            {
                // calc current position
                var angle = 2 * Math.PI * (galaxyElapsedSeconds % body.Orbital_Period) / body.Orbital_Period;
                Double2 unit = Double2.UnitX;
                Double2.Rotate(ref unit, -angle);
                body.LocalPosition = unit * body.Orbital_Radius;

                // calc current orbital velocity
                Double2.Rotate(ref unit, -DoubleHelper.PiOver2);
                body.LocalVelocity = unit * DoubleHelper.TwoPi * body.Orbital_Radius / body.Orbital_Period;

                if (body.Parent != null)
                {
                    body.GlobalPosition = body.Parent.GlobalPosition + body.LocalPosition;
                    body.GlobalVelocity = body.Parent.GlobalVelocity + body.LocalVelocity;
                }
                else
                {
                    body.GlobalPosition = body.LocalPosition;
                    body.GlobalVelocity = body.LocalVelocity;
                }
            }
            else if (body.BodyClassification == BodyClassification.Vessel)
            {
                UpdateVesselPosition(body);
            }

            foreach (OrbitalBody child in body.Children)
            {
                UpdateBodyPositions(child);
            }
            return;
        }

        private void UpdateVesselPosition(OrbitalBody body)
        {
            var vessel = body.GetComponent<Vessel>();
            var route = vessel.NavRoute;

            if (route == null)
            {
                body.LocalPosition += body.LocalVelocity * deltaT;
                body.GlobalPosition = body.LocalPosition;
                if (body.Parent != null) // it could be null...
                {
                    body.GlobalPosition += body.Parent.GlobalPosition;
                    body.GlobalVelocity = body.Parent.GlobalVelocity + body.LocalVelocity;
                }

                return;
            }

            if (deltaT == 0)
                return;

            var flipPoint = vessel.NavRoute.TotalTime * vessel.NavRoute.Eccentricity;
            Double2 primaryBurn = vessel.NavRoute.PrimaryBurn + vessel.NavRoute.NormalizingBurn;
            Double2 brakingBurn = -vessel.NavRoute.PrimaryBurn + vessel.NavRoute.NormalizingBurn;

            double elapsedTimeAtStart = (GameData.CurrentDate - route.StartDate).TotalSeconds - deltaT;
            double elapsedTimeAtEnd = DoubleHelper.Clamp(elapsedTimeAtStart + deltaT, 0, route.TotalTime);
            double timeSpan = DoubleHelper.Clamp(elapsedTimeAtEnd - elapsedTimeAtStart, 0, route.TotalTime - elapsedTimeAtStart); // usually, but not always 3600

            double primaryBurnTime = flipPoint - elapsedTimeAtStart;
            primaryBurnTime = DoubleHelper.Clamp(primaryBurnTime, 0, timeSpan);
            double brakingBurnTime = elapsedTimeAtEnd - flipPoint;
            brakingBurnTime = DoubleHelper.Clamp(brakingBurnTime, 0, timeSpan);

            body.LocalPosition += primaryBurn * Math.Pow(primaryBurnTime, 2) / 2.0 + body.GlobalVelocity * primaryBurnTime;
            body.LocalVelocity += primaryBurn * primaryBurnTime;
            body.LocalPosition += brakingBurn * Math.Pow(brakingBurnTime, 2) / 2.0 + body.GlobalVelocity * brakingBurnTime;
            body.LocalVelocity += brakingBurn * brakingBurnTime;

            body.GlobalPosition = body.LocalPosition;
            body.GlobalVelocity = body.LocalVelocity;
            if (body.Parent != null) // it should be null...
            {
                throw new NotImplementedException(); // this is bad, we can't handle this scenario cuz of gravity.
            }

            if (elapsedTimeAtEnd == route.TotalTime)
            {
                // arrival
                body.Parent = vessel.DestinationTarget;
                body.IsLockedToParent = true;
                body.Orbital_Period = 1;
                body.Orbital_Radius = 0;
                body.LocalVelocity = Double2.Zero;
                body.LocalPosition = Double2.Zero;
                body.GlobalPosition = body.Parent.GlobalPosition;
            }

        }

        public void GetBodyPositionAtTime(OrbitalBody body, DateTime dateTime, out Double2 position, out Double2 velocity)
        {
            List<OrbitalBody> bodyChain = new List<OrbitalBody>();
            OrbitalBody currentBody = body;
            var futureElapsedSeconds = galaxyElapsedSeconds + (dateTime - GameData.CurrentDate).TotalSeconds;

            while (currentBody.Parent != null)
            {
                bodyChain.Add(currentBody);
                currentBody = currentBody.Parent;
            }
            
            Double2[] globalPositionArray = new Double2[bodyChain.Count];
            velocity = Double2.Zero;

            for (int i = bodyChain.Count - 1; i >= 0; i--)
            {
                // calc current position
                var angle = 2 * Math.PI * (futureElapsedSeconds % bodyChain[i].Orbital_Period) / bodyChain[i].Orbital_Period;
                Double2 unit = Double2.UnitX;
                Double2.Rotate(ref unit, -angle);
                var localPosition = unit * bodyChain[i].Orbital_Radius;

                if (i == bodyChain.Count - 1)
                {
                    globalPositionArray[i] = localPosition;
                }
                else
                {
                    globalPositionArray[i] = localPosition + globalPositionArray[i + 1];
                }

                // calc current orbital velocity
                Double2.Rotate(ref unit, -DoubleHelper.PiOver2);
                velocity += unit * DoubleHelper.TwoPi * bodyChain[i].Orbital_Radius / bodyChain[i].Orbital_Period;
            }

            if (bodyChain.Count == 0)
            {
                position = body.GlobalPosition;
                velocity = body.GlobalVelocity;
            }
            else
            {
                position = globalPositionArray[0];
            }
        }

    }
}

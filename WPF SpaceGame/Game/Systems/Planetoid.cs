using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class Planetoid : Component
    {
        private ObservableCollection<Region> regions = new ObservableCollection<Region>();
        private Atmosphere atmosphere;

        double radius = 1; // km
        double iceCoverage; // percentage
        double albedo; // how much does the planet reflect light
        double solarIncidence; // w/m^2
        double polarTemperatureVariation; // absolute value

        public ObservableCollection<Region> Regions
        {
            get
            {
                return regions;
            }

            set
            {
                regions = value;
            }
        }

        public Atmosphere Atmosphere
        {
            get
            {
                return atmosphere;
            }

            set
            {
                atmosphere = value;
                Notify();
            }
        }

        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
                Notify();
            }
        }

        public double IceCoverage
        {
            get
            {
                return iceCoverage;
            }

            set
            {
                iceCoverage = value;
                Notify();
            }
        }

        public double Albedo
        {
            get
            {
                return albedo;
            }

            set
            {
                albedo = value;
                Notify();
            }
        }

        public double SolarIncidence
        {
            get
            {
                return solarIncidence;
            }

            set
            {
                solarIncidence = value;
                Notify();
            }
        }

        public double PolarTemperatureVariation
        {
            get
            {
                return polarTemperatureVariation;
            }

            set
            {
                polarTemperatureVariation = value;
                Notify();
            }
        }

        public Planetoid(Entity entity) : base(entity)
        {
            atmosphere = new Atmosphere();
        }
    }
}

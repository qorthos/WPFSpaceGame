using WPFSpaceGame.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class NavRoute : ObservableObject
    {
        private DateTime startDate;
        private DateTime endDate;
        private double timeSpan;
        private Double2 primaryBurn;
        private Double2 normalizingBurn;
        private double eccentricity;
        private List<Double2> predictedPositions = new List<Double2>();
        private List<Double2> predictedVelocities = new List<Double2>();
        private int calcIterations = 0;


        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                Notify();
            }
        }

        public Double2 PrimaryBurn
        {
            get
            {
                return primaryBurn;
            }

            set
            {
                primaryBurn = value;
                Notify();
            }
        }

        public Double2 NormalizingBurn
        {
            get
            {
                return normalizingBurn;
            }

            set
            {
                normalizingBurn = value;
                Notify();
            }
        }

        public double Eccentricity
        {
            get
            {
                return eccentricity;
            }

            set
            {
                eccentricity = value;
                Notify();
            }
        }

        public double TotalTime
        {
            get
            {
                return timeSpan;
            }

            set
            {
                timeSpan = value;
                Notify();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                Notify();
            }
        }

        public List<Double2> PredictedPositions
        {
            get
            {
                return predictedPositions;
            }

            set
            {
                predictedPositions = value;
                Notify();
            }
        }

        public List<Double2> PredictedVelocities
        {
            get
            {
                return predictedVelocities;
            }

            set
            {
                predictedVelocities = value;
                Notify();
            }
        }

        public int CalcIterations
        {
            get
            {
                return calcIterations;
            }

            set
            {
                calcIterations = value;
                Notify();
            }
        }
    }
}

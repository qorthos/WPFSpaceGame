using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game.Systems
{
    public class Atmosphere : ObservableObject
    {
        double totalAtms;
        double greenhouseFactor; //0-1

        public double TotalAtms
        {
            get
            {
                return totalAtms;
            }

            set
            {
                totalAtms = value;
                Notify();
            }
        }

        public double GreenhouseFactor
        {
            get
            {
                return greenhouseFactor;
            }

            set
            {
                greenhouseFactor = value;
                Notify();
            }
        }
    }
}

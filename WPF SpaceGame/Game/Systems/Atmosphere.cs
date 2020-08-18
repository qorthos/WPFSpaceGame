using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;
using WPFSpaceGame.Views.Map;

namespace WPFSpaceGame.Game.Systems
{
    public class Atmosphere : ObservableObject
    {
        double totalAtms;
        double greenhouseFactor; //0-1
        ObservableCollection<Gas> gasses = new ObservableCollection<Gas>();

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

        public ObservableCollection<Gas> Gasses
        {
            get
            {
                return gasses;
            }
        }

        public bool IsEmpty
        {
            get { return gasses.Count == 0 ? true : false; }
        }

        public void AddGas(GasDef gasDef, double qty)
        {
            var select = gasses.SingleOrDefault(x => x.GasDef == gasDef);
            if (select == null)
            {
                select = new Gas(gasDef);
                gasses.Add(select);
            }

            select.Quantity.BaseValue += qty;
            select.Quantity.BaseValue = DoubleHelper.Clamp(select.Quantity.BaseValue, 0, 100);

        }
    }
}

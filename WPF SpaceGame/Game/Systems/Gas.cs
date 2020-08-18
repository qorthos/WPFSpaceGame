using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game.Systems
{
    public class Gas
    {
        public GasDef GasDef { get; set; }

        public Modifier Quantity { get; set; } // atmos
        public Modifier GreenhouseImpactModifier { get; set; }

        public Gas(GasDef def)
        {
            this.GasDef = def;
            Quantity = new Modifier(def.Name + " Quantity", 0, 100);

            GreenhouseImpactModifier = new Modifier(def.Name + " Greenhouse Impact", 0.0, 1.0, ModifierMathEnum.Additive, def.GreenHouseFunction, def.GreenHouseScale);
            GreenhouseImpactModifier.AddModifier(Quantity);
        }
    }


    public class GasDef : Definition
    {
        public ForcingFunctionEnum GreenHouseFunction { get; set; }
        public double GreenHouseScale { get; set; }
    }
}

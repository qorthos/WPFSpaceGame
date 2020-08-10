using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game
{
    public class Ownership : Component
    {
        [NonSerialized] private Faction faction;
        private string factionTag;


        public Faction Faction
        {
            get
            {
                return faction;
            }
            set
            {
                faction = value;
                factionTag = faction.Tag;
            }
        }


        public string FactionTag
        {
            get
            {
                return factionTag;
            }
        }


        public Ownership(Entity entity) : base(entity)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.Game.Systems;

namespace WPFSpaceGame.Game
{
    /// <summary>
    /// Top level object that defines a player or AI
    /// </summary>
    public class Faction : Component
    {
        public string Tag;
        public bool IsPlayer;
        public StellarSystem StartingSystem;
        public OrbitalBody StartingBody;

        [NonSerialized] Guid startingSystemID;
        [NonSerialized] Guid startingBodyID; 

        public Faction(Entity entity) : base(entity)
        {
            
        }


        protected override void Build()
        {
            base.Build();
            StartingSystem = GameData.GetEntity(startingSystemID).GetComponent<StellarSystem>();
            StartingBody = GameData.GetEntity(startingBodyID).GetComponent<OrbitalBody>();

        }

        protected override void Save()
        {
            base.Save();
            startingSystemID = StartingSystem.Entity.Id;
            startingBodyID = StartingBody.Entity.Id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WPFSpaceGame.Game.Systems
{
    public class StellarSystem : Component
    {

        private string name;
        [NonSerialized] private ObservableCollection<OrbitalBody> children = new ObservableCollection<OrbitalBody>();
        List<Guid> childrenIDs = new List<Guid>();
        
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                Notify();
            }
        }

        public ObservableCollection<OrbitalBody> Children
        {
            get
            {
                return children;
            }
        }

        public StellarSystem(Entity entity) : base(entity)
        {

        }

        protected override void Save()
        {
            childrenIDs.Clear();
            for (int i = 0; i < Children.Count; i++)
            {
                childrenIDs.Add(Children[i].Entity.Id);
            }

            base.Save();
        }

        protected override void Build()
        {
            Children.Clear();
            for (int i = 0; i < childrenIDs.Count; i++)
            {
                Children.Add(GameData.GetEntity(childrenIDs[i]).GetComponent<OrbitalBody>());
            }
            childrenIDs.Clear();

            base.Build();
        }
    }
}

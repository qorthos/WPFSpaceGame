using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace WPFSpaceGame.Game.Systems
{
    public class OrbitalBody : Component
    {
        private string name;
        private OrbitalBody parent;
        Guid parentID;
        bool isLockedToParent = false;

        [NonSerialized] private ObservableCollection<OrbitalBody> children = new ObservableCollection<OrbitalBody>();
        List<Guid> childrenIDs = new List<Guid>();

        private StellarSystem stellarSystem;
        [NonSerialized] Guid stellarSystemID;
        private Double2 globalPosition;
        private Double2 localPosition;

        private Double2 globalVelocity;
        private Double2 localVelocity;

        private BodyClassification bodyClassification;
        private double boundsRadius;

        private double mass;
        private double orbital_Radius;
        private double orbital_Period;
        

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

        public StellarSystem StellarSystem
        {
            get
            {
                return stellarSystem;
            }

            set
            {
                if (stellarSystem != null)
                    stellarSystem.Children.Remove(this);

                stellarSystem = value;
                Notify();

                stellarSystem.Children.Add(this);
            }
        }

        public OrbitalBody Parent
        {
            get { return parent; }
            set 
            {
                var oldParent = parent;                
                parent = value;

                if (oldParent != null)
                {
                    oldParent.Children.Remove(this);
                    LocalPosition += oldParent.GlobalPosition;
                    LocalVelocity += oldParent.GlobalVelocity;
                    
                }
                if (parent != null)
                {
                    parent.Children.Add(this);
                    LocalPosition -= parent.GlobalPosition;
                    LocalVelocity -= parent.GlobalVelocity;
                }

                Notify();
            }
        }

        public bool IsLockedToParent
        {
            get
            {
                return isLockedToParent;
            }

            set
            {
                isLockedToParent = value;
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

        public Double2 GlobalPosition
        {
            get
            {
                return globalPosition;
            }
            internal set
            {
                globalPosition = value;
                Notify();
            }
        }

        public Double2 LocalPosition
        {
            get
            {
                return localPosition;
            }

            set
            {
                localPosition = value;
                Notify();
            }
        }

        public Double2 LocalVelocity
        {
            get
            {
                return localVelocity;
            }

            set
            {
                localVelocity = value;
                Notify();
                RaisePropertyChanged(nameof(Orbital_Speed));
            }
        }


        public Double2 GlobalVelocity
        {
            get
            {
                return globalVelocity;
            }

            set
            {
                globalVelocity = value;
                Notify();
            }
        }

        public BodyClassification BodyClassification
        {
            get
            {
                return bodyClassification;
            }

            set
            {
                bodyClassification = value;
                Notify();
            }
        }

        public double BoundsRadius
        {
            get
            {
                return boundsRadius;
            }

            set
            {
                boundsRadius = value;
                Notify();
            }
        }

        public double Mass
        {
            get
            {
                return mass;
            }

            set
            {
                mass = value;
                Notify();
            }
        }

        public double Orbital_Radius
        {
            get
            {
                return orbital_Radius;
            }

            set
            {
                orbital_Radius = value;
                Notify();
            }
        }

        public double Orbital_Period
        {
            get
            {
                return orbital_Period;
            }

            set
            {
                orbital_Period = value;
                Notify();
            }
        }
        
        public double Orbital_Speed
        {
            get
            {
                return LocalVelocity.Length();
            }
        }

        public OrbitalBody(Entity entity) : base(entity)
        {

        }

        protected override void Save()
        {
            childrenIDs.Clear();
            for (int i = 0; i < Children.Count; i++)
            {
                childrenIDs.Add(Children[i].Entity.Id);
            }

            parentID = parent.Entity.Id;

            stellarSystemID = stellarSystem.Entity.Id;

            base.Save();
        }

        protected override void Build()
        {
            parent = null;
            if (parentID != null)
            {
                parent = GameData.GetEntity(parentID).GetComponent<OrbitalBody>();
            }

            Children.Clear();
            for (int i = 0; i < childrenIDs.Count; i++)
            {
                Children.Add(GameData.GetEntity(childrenIDs[i]).GetComponent<OrbitalBody>());
            }
            childrenIDs.Clear();

            StellarSystem = (StellarSystem)GameData.GetComponents<StellarSystem>().Where(x => x.Entity.Id == stellarSystemID);

            base.Build();
        }

        public override string ToString()
        {
            return name;
        }
    }

    public enum BodyClassification : int
    {
        Star = 0,
        Planet = 1,
        Moon = 2,
        Asteroid = 3,
        Debris = 4,
        Vessel = 5,
    }
}

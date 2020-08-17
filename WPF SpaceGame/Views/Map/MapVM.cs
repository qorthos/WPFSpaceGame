using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFSpaceGame.Game;
using WPFSpaceGame.Game.Systems;
using WPFSpaceGame.Game.Graphics;
using WPFSpaceGame.General;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;
using MonoGame.Extended.Tweening;

namespace WPFSpaceGame.Views.Map
{
    public class MapData
    {
        public StellarSystem SelectedSystem;
        public OrbitalBody SelectedBody;
    }


    public class MapVM : ViewModel
    {
        private ViewModel topBarContent;
        private MonoGameItem graphicsItem;
        private MapData graphicsItemData;


        private StellarSystem selectedSystem;
        private ObservableCollection<StellarSystem> systems = new ObservableCollection<StellarSystem>();
        private ObservableCollection<OrbitalBody> mapBodies = new ObservableCollection<OrbitalBody>();
        private OrbitalBody selectedMapBody;

        private ViewModel selectedBodyVM;


        public ViewModel TopBarContent
        {
            get
            {
                return topBarContent;
            }

            set
            {
                topBarContent = value;
                Notify();
            }
        }

        public MonoGameItem GraphicsItem
        {
            get
            {
                return graphicsItem;
            }

            set
            {
                graphicsItem = value;
                Notify();
            }
        }

        public object GraphicsItemData
        {
            get { return graphicsItemData; }
            set { graphicsItemData = (MapData)value; Notify(); }
        }

        protected MapData MapData
        {
            get { return graphicsItemData as MapData; }
        }
                
        public StellarSystem SelectedSystem
        {
            get
            {
                return selectedSystem;
            }

            set
            {
                selectedSystem = value;
                Notify();

                MapData.SelectedSystem = value;   

                MapBodies.Clear();
                foreach (OrbitalBody childBody in SelectedSystem.Children)
                {
                    if (childBody.Parent != null)
                        continue; // just get top level ones

                    AddMapBody(childBody);
                }

                SelectedMapBody = MapBodies[0];
            }
        }

        public ObservableCollection<StellarSystem> Systems
        {
            get
            {
                return systems;
            }
        }

        public ObservableCollection<OrbitalBody> MapBodies
        {
            get
            {
                return mapBodies;
            }
        }

        public OrbitalBody SelectedMapBody
        {
            get
            {
                return selectedMapBody;
            }

            set
            {
                selectedMapBody = value;
                Notify();
                MapData.SelectedBody = value;

                if (SelectedMapBody.BodyClassification == BodyClassification.Vessel)
                {
                    SelectedBodyVM = VMManager.GetVM<SelectedVesselVM>();
                    (SelectedBodyVM as SelectedVesselVM).SelectedBody = SelectedMapBody;
                }
                else
                {
                    SelectedBodyVM = VMManager.GetVM<SelectedBodyVM>();
                    (SelectedBodyVM as SelectedBodyVM).Body = SelectedMapBody;
                }
            }
        }

        public ViewModel SelectedBodyVM
        {
            get
            {
                return selectedBodyVM;
            }

            set
            {
                selectedBodyVM = value;
                Notify();
            }
        }

        public MapVM()
            : base(true)
        {
            GraphicsItemData = new MapData();
            OnNewGame();
            TopBarContent = VMManager.GetVM<TopBarVM>();
        }

        protected override void OnNewGame()
        {
            var playerFaction = GameData.GetComponents<Faction>().First(x => x.IsPlayer == true);
            SelectedSystem = playerFaction.StartingSystem;

            systems.Clear();
            var sys = GameData.GetComponents<StellarSystem>();
            foreach (StellarSystem system in sys)
            {
                Systems.Add(system);
            }
        }

        public override void Focused()
        {
            if (GraphicsItem == null)
                GraphicsItem = new SystemGraphics();
            base.Focused();
        }

        private void AddMapBody(OrbitalBody newBody)
        {
            MapBodies.Add(newBody);
            foreach (OrbitalBody child in newBody.Children)
            {
                AddMapBody(child);
            }
        }
    }


    
}

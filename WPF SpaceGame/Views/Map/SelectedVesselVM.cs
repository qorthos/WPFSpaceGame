using WPFSpaceGame.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.Game;
using WPFSpaceGame.Game.Systems;
using MonoGame.Extended.Collections;

namespace WPFSpaceGame.Views.Map
{
    public class SelectedVesselVM : ViewModel
    {
        private OrbitalBody body;
        private Vessel vessel;
        private ObservableCollection<OrbitalBody> orbitalBodies = new ObservableCollection<OrbitalBody>();
        private OrbitalBody newDestination;

        public RelayCommand SetDestination { get; private set; }


        public OrbitalBody Body
        {
            get
            {
                return body;
            }

            set
            {
                body = value;
                Notify();

                
                var bodies = GameData.GetComponents<OrbitalBody>();                
                orbitalBodies.Clear();
                NewDestination = bodies.First();
                foreach (OrbitalBody body in bodies)
                {
                    if (body == Body)
                        continue;

                    orbitalBodies.Add(body);
                }

                Vessel = Body.GetComponent<Vessel>();
            }
        }

        public Vessel Vessel
        {
            get
            {
                return vessel;
            }

            set
            {
                vessel = value;
                Notify();
            }
        }

        public ObservableCollection<OrbitalBody> OrbitalBodies
        {
            get
            {
                return orbitalBodies;
            }

            set
            {
                OrbitalBodies = value;
            }
        }

        public OrbitalBody NewDestination
        {
            get
            {
                return newDestination;
            }

            set
            {
                Notify();
                newDestination = value;
            }
        }


        public SelectedVesselVM() : base(true)
        {
            SetDestination = new RelayCommand(x => ExecuteSetDestination());
        }


        private void ExecuteSetDestination()
        {
            Vessel.DestinationTarget = NewDestination;
        }
    }
}

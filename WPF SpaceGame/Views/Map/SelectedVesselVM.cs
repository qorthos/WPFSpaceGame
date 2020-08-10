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
        private OrbitalBody selectedBody;
        private Vessel selectedVessel;
        private ObservableCollection<OrbitalBody> destinations = new ObservableCollection<OrbitalBody>();
        private OrbitalBody newDestination;

        public RelayCommand SetDestination { get; private set; }

        public OrbitalBody SelectedBody
        {
            get
            {
                return selectedBody;
            }

            set
            {
                selectedBody = value;
                Notify();

                // load up available destinations.
                destinations.Clear();
                var stellarSystem = selectedBody.StellarSystem;
                foreach (OrbitalBody child in stellarSystem.Children)
                {
                    if (child.Parent == null) // we only want the top level objects in the system.
                        AddDestination(child);
                }

                SelectedVessel = SelectedBody.GetComponent<Vessel>();
            }
        }

        public Vessel SelectedVessel
        {
            get
            {
                return selectedVessel;
            }

            set
            {
                selectedVessel = value;
                Notify();
            }
        }

        public ObservableCollection<OrbitalBody> Destinations
        {
            get
            {
                return destinations;
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
            SelectedVessel.DestinationTarget = NewDestination;
        }

        private void AddDestination(OrbitalBody destinationBody)
        {
            if (destinationBody == selectedBody)
            {
                // do nothing
            }
            else if ((destinationBody == selectedBody.Parent) && (selectedBody.IsLockedToParent == true))
            {
                // do nothing
            }
            else
            {
                destinations.Add(destinationBody);
            }

            foreach (OrbitalBody child in destinationBody.Children)
            {
                AddDestination(child);
            }
        }
    }
}

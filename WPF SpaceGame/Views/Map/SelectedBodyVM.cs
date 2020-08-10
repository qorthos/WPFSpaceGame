using WPFSpaceGame.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.Game.Systems;

namespace WPFSpaceGame.Views.Map
{
    public class SelectedBodyVM : ViewModel
    {
        private OrbitalBody body;

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
            }
        }

        public SelectedBodyVM() : base(true)
        {

        }


    }
}

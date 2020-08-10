using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game
{
    public class GameSettings : ObservableObject
    {
        private string _playerEmpireName;

        public string PlayerEmpireName
        {
            get
            {
                return _playerEmpireName;
            }

            set
            {
                _playerEmpireName = value;
                Notify();
            }
        }
    }
}

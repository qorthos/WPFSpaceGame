using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Views
{
    public class TitleVM : ViewModel
    {
        private RelayCommand _CMDStartNewGame;


        public RelayCommand CMDStartNewGame
        {
            get
            {
                return _CMDStartNewGame;
            }
        }


        public TitleVM()
            : base(true)
        {
            _CMDStartNewGame = new RelayCommand(x => StartNewGame());
        }



        private void StartNewGame()
        {
            VMManager.TransitionToVM<NewGameSettingsVM>();
        }

    }
}

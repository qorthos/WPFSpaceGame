using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;
using WPFSpaceGame.Game;

namespace WPFSpaceGame.Views
{
    public class NewGameSettingsVM : ViewModel
    {
        private RelayCommand _CMDStartNewGame;

        GameSettings _gameSettings;


        public RelayCommand CMDStartNewGame
        {
            get
            {
                return _CMDStartNewGame;
            }
        }


        public GameSettings GameSettings
        {
            get
            {
                return _gameSettings;
            }

            set
            {
                _gameSettings = value;
                Notify();
            }
        }


        public NewGameSettingsVM()
            : base(addToVMManager : true)
        {
            _CMDStartNewGame = new RelayCommand(x => StartNewGame());
            GameSettings = new GameSettings()
            {
                PlayerEmpireName = "WPFSpaceGame of Man",
            };
        }


        private void StartNewGame()
        {
            // create the game data stuff
            GameService.StartNewGame(GameSettings);

            // change vm to the default screen
            VMManager.TransitionToVM<Views.Map.MapVM>();
        }

    }
}

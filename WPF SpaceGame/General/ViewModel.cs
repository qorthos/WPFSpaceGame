using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.Game;
using MahApps.Metro.Controls.Dialogs;

namespace WPFSpaceGame.General
{
    public abstract class ViewModel : ObservableObject
    {
        protected VMManager VMManager;
        protected ServiceProvider ServiceProvider;
        protected GameService GameService;

        protected IDialogCoordinator DialogCoordinator;


        public GameData GameData
        {
            get { return GameService.GameData; }
        }


        public ViewModel(bool addToVMManager)
        {
            DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;
            ServiceProvider = ServiceProvider.Instance;
            VMManager = ServiceProvider.GetService<VMManager>();
            GameService = ServiceProvider.GetService<GameService>();
            GameService.NewGameLoadedEvent += GameService_NewGameLoadedEvent;
            GameService.GameTickFinishedEvent += GameService_GameTickFinishedEvent;

            if (addToVMManager)
                VMManager.AddVM(this);
        }

        

        private void GameService_NewGameLoadedEvent(GameService service)
        {
            OnNewGame();
            Notify(nameof(GameData));
        }


        private void GameService_GameTickFinishedEvent(GameService service)
        {
            OnTickFinished();
        }


        protected virtual void OnNewGame()
        {

        }


        protected virtual void OnTickFinished()
        {

        }


        public virtual void Focused()
        {

        }

    }
}

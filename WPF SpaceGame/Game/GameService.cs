using WPFSpaceGame.Game.Systems;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game
{

    public delegate void NewGameDataEvent(GameService service);
    public delegate void NewGameLoadedEvent(GameService service);
    public delegate void EntityAddedEvent(Entity newEntity);
    public delegate void EntityRemovedEvent(Entity removedEntity);
    public delegate void GameTickFinished(GameService service);

    public class GameService : ObservableObject
    {
        // CURRENT GAME DATA
        GameData gameData;

        // GAME SYSTEMS
        List<GameSystem> gameSystems = new List<GameSystem>();
        public DefLibrary DefLibrary = new DefLibrary();
        
        public event NewGameDataEvent NewGameDataEvent;
        public event NewGameLoadedEvent NewGameLoadedEvent;
        public event GameTickFinished GameTickFinishedEvent;
        public event EntityAddedEvent EntityAddedEvent;
        public event EntityRemovedEvent EntityRemovedEvent;
        

        public GameData GameData
        {
            get
            {
                return gameData;
            }

            protected set
            {
                gameData = value;
                NewGameDataEvent?.Invoke(this);
                NewGameLoadedEvent?.Invoke(this);
                Notify();
            }
        }


        public GameService()
        {

        }


        public void StartNewGame(GameSettings gameSettings)
        {
            GameData = new GameData(gameSettings);
        }


        public void OnNewEntity(Entity newEntity)
        {
            EntityAddedEvent?.Invoke(newEntity);
        }


        public void OnRemovedEntity(Entity removedEntity)
        {
            EntityRemovedEvent?.Invoke(removedEntity);
        }


        public void AddGameSystem(GameSystem gs)
        {
            gameSystems.Add(gs);
        }


        public void Initialize()
        {
            foreach (GameSystem gs in gameSystems)
                gs.Initialize();
        }


        public T GetGameSystem<T>() where T : GameSystem
        {
            for (int i = 0; i < gameSystems.Count; i++)
            {
                if (gameSystems[i] is T)
                    return (T)gameSystems[i];
            }

            return null;
        }


        public void TickDay()
        {
            if (gameData == null)
                return;

            for (int i = 0; i < 24; i++)
            {
                GameData.CurrentDate = GameData.CurrentDate.AddHours(1);

                foreach (GameSystem gs in gameSystems)
                {
                    gs.TickHour();
                }
            }

            foreach (GameSystem gs in gameSystems)
            {
                gs.TickDay();
            }

            GameTickFinishedEvent?.Invoke(this);
        }

    }
}

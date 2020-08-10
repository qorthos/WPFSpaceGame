using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game
{
    public class GameData : ObservableObject
    {
        GameService gs;

        // game creation settings
        GameSettings gameSettings;

        // datetime
        DateTime _gameDate;

        // entities
        HashSet<Entity> entities = new HashSet<Entity>();
        [NonSerialized] Dictionary<Guid, Entity> entityLib = new Dictionary<Guid, Entity>();

        // components
        Dictionary<Type, HashSet<Component>> componentLib = new Dictionary<Type, HashSet<Component>>();


        public DateTime CurrentDate
        {
            get
            {
                return _gameDate;
            }

            set
            {
                _gameDate = value;
                Notify();
            }
        }


        public GameSettings GameSettings
        {
            get
            {
                return gameSettings;
            }

            set
            {
                gameSettings = value;
            }
        }


        public GameData(GameSettings gameSettings)
        {
            gs = ServiceProvider.Instance.GetService<GameService>();

            
            this.gameSettings = gameSettings;
            _gameDate = new DateTime(2120, 1, 1, 1, 1, 1);            
        }


        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
            entityLib.Add(entity.Id, entity);

            foreach (Component comp in entity.Components)
            {
                Type type = comp.GetType();
                if (componentLib.ContainsKey(type) == false)
                    componentLib.Add(type, new HashSet<Component>());

                componentLib[type].Add(comp);
            }

            gs.OnNewEntity(entity);
        }


        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
            entityLib.Remove(entity.Id);

            foreach (Component comp in entity.Components)
            {
                Type type = comp.GetType();

                componentLib[type].Remove(comp);
            }
        }


        public Entity GetEntity(Guid id)
        {
            return entityLib[id];
        }


        public Faction GetFaction(string factionTag)
        {
            return (Faction)componentLib[typeof(Faction)].First(x => ((Faction)x).Tag == factionTag);
        }


        public IEnumerable<T> GetComponents<T>() where T: Component
        {
            return componentLib[typeof(T)].Cast<T>();
        }
    }
}

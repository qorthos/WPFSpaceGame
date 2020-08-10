using WPFSpaceGame.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game
{
    public class GameSystem
    {
        List<Type> interestedComponents = new List<Type>();
        HashSet<Entity> watchedEntities = new HashSet<Entity>();

        protected GameService GameService;

        protected GameData GameData
        {
            get { return GameService.GameData; }
        }

        public GameSystem()
        {
            GameService = ServiceProvider.Instance.GetService<GameService>();
            GameService.AddGameSystem(this);

            GameService.EntityAddedEvent += GameService_EntityAddedEvent;
            GameService.EntityRemovedEvent += GameService_EntityRemovedEvent;
            GameService.NewGameDataEvent += GameService_NewGameDataEvent;
            GameService.NewGameLoadedEvent += GameService_GameStartedEvent;
        }
                
        public virtual void Initialize()
        {

        }

        protected void AddComponentInterest<T>() where T : Component
        {
            interestedComponents.Add(typeof(T));
        }
            
        protected virtual void GameService_NewGameDataEvent(GameService service)
        {
            
        }

        protected virtual void GameService_GameStartedEvent(GameService service)
        {

        }

        protected virtual void GameService_EntityRemovedEvent(Entity removedEntity)
        {

            if (watchedEntities.Contains(removedEntity))
            {
                foreach (Component component in removedEntity.Components)
                {
                    if (interestedComponents.Contains(component.GetType()))
                    {
                        component.PropertyChanged -= Component_PropertyChanged;
                    }
                }

                watchedEntities.Remove(removedEntity);
            }
        }

        protected virtual void GameService_EntityAddedEvent(Entity newEntity)
        {
            bool doOnce = true;
            foreach (Component component in newEntity.Components)
            {
                if (interestedComponents.Contains(component.GetType()))
                {
                    component.PropertyChanged += Component_PropertyChanged;
                    OnComponentAdded(component);

                    if (doOnce)
                        watchedEntities.Add(newEntity);
                    doOnce = false;
                    
                }
            }
        }

        private void Component_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnComponentPropertyChanged(sender as Component, e.PropertyName);
        }

        protected virtual void OnComponentPropertyChanged(Component component, string propertyName)
        {

        }


        protected virtual void OnComponentAdded(Component component)
        {

        }

        public virtual void TickDay()
        {

        }

        public virtual void TickHour()
        {

        }
    }
}

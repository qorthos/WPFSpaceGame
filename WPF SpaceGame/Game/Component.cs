using WPFSpaceGame.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game
{
    public class Component : ObservableObject
    {
        [NonSerialized] Entity entity;
        Guid entityID; // only used in saving

        [NonSerialized] protected GameService GameService;
        
        public Entity Entity
        {
            get
            {
                return entity;
            }
        }

        protected GameData GameData
        {
            get { return GameService.GameData; }
        }

        public Component(Entity entity)
        {
            GameService = ServiceProvider.Instance.GetService<GameService>();
            this.entity = entity;
            entity.Components.Add(this);
        }


        public void BuildFromSave()
        {            
            entity = GameService.GameData.GetEntity(entityID);
            Build();
        }


        public void SetSaveData()
        {
            this.entityID = entity.Id;
            Save();
        }


        public T GetComponent<T>() where T : Component
        {
            return Entity.GetComponent<T>();
        }


        protected virtual void Build()
        {

        }


        protected virtual void Save()
        {

        }
    }
}

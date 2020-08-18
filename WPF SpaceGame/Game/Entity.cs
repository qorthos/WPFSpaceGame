using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game
{
    [Serializable]
    public class Entity : ObservableObject
    {
        private Guid id;
        List<Component> components = new List<Component>();

        public Guid Id
        {
            get
            {
                return id;
            }
        }

        public List<Component> Components
        {
            get
            {
                return components;
            }
        }

        public Entity()
        {
            id = Guid.NewGuid();
        }

        public T GetComponent<T>() where T: Component
        {
            return (T)components.First(x => x is T);
        }
    }
}

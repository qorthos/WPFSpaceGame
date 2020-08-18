using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game
{
    public class DefLibrary
    {
        Dictionary<Type, Dictionary<string, Definition>> lib = new Dictionary<Type, Dictionary<string, Definition>>();

        public void AddDef<T>(T def) where T : Definition
        {
            Type type = typeof(T);
            if (lib.ContainsKey(type) == false)
            {
                lib.Add(type, new Dictionary<string, Definition>());
            }

            lib[type].Add(def.Name, def);
        }
        

        public T GetDef<T>(string name) where T : Definition
        {
            Type type = typeof(T);
            return (T)lib[type][name];
        }


        public void Clear()
        {
            lib.Clear();
        }

    }
}

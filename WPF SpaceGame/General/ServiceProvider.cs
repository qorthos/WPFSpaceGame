using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WPFSpaceGame.General
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services;

        static ServiceProvider instance;
        public static ServiceProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceProvider();

                return instance;
            }
        }


        private ServiceProvider()
        {
            _services = new Dictionary<Type, object>();
        }


        public void AddService(Type type, object provider)
        {
            _services.Add(type, provider);
        }


        public object GetService(Type type)
        {
            if (_services.TryGetValue(type, out var service))
                return service;

            return null;
        }


        public void RemoveService(Type type)
        {
            _services.Remove(type);
        }


        public void AddService<T>(T service)
        {
            AddService(typeof(T), service);
        }


        public T GetService<T>() where T : class
        {
            var service = GetService(typeof(T));
            return (T) service;
        }
    }
}

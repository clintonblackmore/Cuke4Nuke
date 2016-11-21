using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuke4Nuke.Core
{
    public class ObjectFactory
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        List<Type> _classes = new List<Type>();
        Dictionary<Type, object> map = new Dictionary<Type, object>();

        // We will recursively create objects to pass to constructors to create things
        // If we've gone this deep, it is hopeless!
        private const int maxDepth = 20;

        public void AddClass(Type type)
        {
            if (!_classes.Contains(type))
            {
                _classes.Add(type);
                foreach (ConstructorInfo ci in type.GetConstructors())
                {
                    foreach (ParameterInfo pi in ci.GetParameters())
                    {
                        AddClass(pi.ParameterType);
                    }
                }
            }
        }

        public void CreateObjects()
        {
            foreach (Type type in _classes)
            {
                object instance = CreateObject(type, 0);
                if (instance == null)
                {
                    Debug.LogError("Couldn't create object");
                }
                map.Add(type, instance);
            }
        }

        public object GetObject(Type type)
        {
            object instance = null;

            if (map.ContainsKey(type))
            {
                instance = map[type];
            }
            else
            {
                Debug.LogWarning("Type not in dict");
            }

            //Debug.LogFormat("ObjectFactory.GetObject({0}) returning ({1})", type, instance);
            if (instance == null)
            {
                Debug.LogError("Instance is null");
            }
            return instance;
        }

        public void DisposeObjects()
        {
            map.Clear();
        }

        private void ShowMappings()
        {
            Debug.Log("Dictionary contains");
            foreach (Type key in map.Keys)
            {
                object value = map[key];
                Debug.LogFormat(" - {0} -> {1}", key, value);
            }
        }

        private object CreateObject(Type type, int depth)
        {
            if (depth >= maxDepth) return null;

            // We want to create this 'thing' but it may need some parameters
            // Go through all the constructors,
            // creating default objects for each of the parameters (recursively)
            // until we find one we can call
            foreach (ConstructorInfo ci in type.GetConstructors())
            {
                ParameterInfo[] parameters = ci.GetParameters();
                int numParams = parameters.Length;
                object[] args = new object[numParams];
                bool badParameter = false;

                for (int i = 0; badParameter == false && i < numParams; ++i)
                {
                    args[i] = CreateObject(parameters[i].ParameterType, depth + 1);
                    badParameter = (args[i] == null);
                }

                if (!badParameter)
                {
                    object instance = Activator.CreateInstance(type, args);
                    if (instance != null)
                    {
                        return instance;
                    }
                }
            }
            return null;
        }
    }
}

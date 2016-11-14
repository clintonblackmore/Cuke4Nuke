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
        //Dictionary<Type, String> map = new Dictionary<Type, String>();

        public void AddClass(Type type)
        {
            Debug.LogFormat("AddClass called with type {0}", type);

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
            Debug.LogWarning("ObjectFactory.CreateObjects()");
        //    foreach (Type type in _classes)
        //    {
        //        map.Add(type, type.ToString());
        //    }
        }

        public object GetObject(Type type)
        {
            Debug.LogWarning("ObjectFactory.GetObject()");

                //    if (map.ContainsValue(type))
        //    {
        //        return map[type];
        //    }

            return null;
        }

        public void DisposeObjects()
        {
            Debug.LogWarning("ObjectFactory.DisposeObjects()");

        //    map.Clear();
        }
    }
}

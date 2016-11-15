using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;


namespace Cuke4Nuke.Core
{
    public class Loader
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly ObjectFactory _objectFactory;

        public Loader(ObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public virtual Repository Load()
        {
            var repository = new Repository();
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (IsDesiredAssembly(assembly))
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        foreach (var method in type.GetMethods(StepDefinition.MethodFlags))
                        {
                            if (StepDefinition.IsValidMethod(method))
                            {
                                repository.StepDefinitions.Add(new StepDefinition(method));
                                _objectFactory.AddClass(method.ReflectedType);
                            }
                            if (BeforeHook.IsValidMethod(method))
                            {
                                repository.BeforeHooks.Add(new BeforeHook(method));
                                _objectFactory.AddClass(method.ReflectedType);
                            }
                            if (AfterHook.IsValidMethod(method))
                            {
                                repository.AfterHooks.Add(new AfterHook(method));
                                _objectFactory.AddClass(method.ReflectedType);
                            }
                        }
                    }
                }
            }

            return repository;
        }

        public bool IsDesiredAssembly(Assembly assembly)
        {
            // Unity loads scores of assemblies;
            // While testing this, it loaded 45 for this project!
            
            // The assemblies we don't care about have names like: 
            // Boo.Lang.Parser, System.Xml.Linq, and Newtonsoft.Json

            // Our code is in assemblies with names like this:
            // Assembly-CSharp, Assembly-CSharp-Editor, Assembly-UnityScript
            // depending on the language, and if it is editor-only code or not

            return assembly.GetName().Name.StartsWith("Assembly-");
        }
    }
}
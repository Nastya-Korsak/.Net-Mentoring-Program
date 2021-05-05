using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Task1
{
    public class Container
    {
        private readonly Dictionary<Type, Func<object>> types = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> typesWithInstances = new Dictionary<Type, object>();

        public void AddAssembly(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            
        }

        public void AddType(Type type)
        {
            AddType(type, type);
        }

        public void AddType(Type type, Type baseType)
        {
            if (baseType is null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!(type.BaseType == baseType) && !(type == baseType) && (!type.GetInterfaces().Contains(baseType)))
            {
                throw new Exception();
            }

            types.Add(baseType, () => GetInstanceWithImportPublicFields(type));
        }

        public T Get<T>()
        {
            var result = GetInstance(typeof(T));

            if (result == (object)default(T))
            {
                throw new Exception("Type was not added to container");
            }

            return (T)result;
        }

        private object GetInstance(Type type)
        {
            if (typesWithInstances.TryGetValue(type, out var instance))
            {
                return instance;
            };

            if (types.TryGetValue(type, out var instanceFunc))
            {
                return instanceFunc.Invoke();
            };

            return GetDefaultValue(type);
        }

        private object GetInstanceWithImportConstructor(Type type)
        {
            object[] parameters;
            var constructors = type.GetConstructors().Single();

            parameters = constructors
                .GetParameters()
                .Select(p => GetInstance(p.ParameterType))
                .ToArray();

            var instance = Activator.CreateInstance(type, parameters);

            typesWithInstances.Add(type, instance);

            return instance;
        }

        private object GetInstanceWithImportPublicFields(Type type)
        {
            var instance = GetInstanceWithImportConstructor(type);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.GetCustomAttributes().Select(a => a.GetType().Name).ToList().Contains("ImportAttribute"))
                {
                    property.SetValue(instance, GetInstance(property.PropertyType));
                }
            }

            return instance;
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }
    }
}

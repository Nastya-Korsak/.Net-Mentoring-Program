using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Task1.DoNotChange;

namespace Task1
{
    public class Container
    {
        private readonly Dictionary<Type, Func<object>> _types = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> _typesWithInstances = new Dictionary<Type, object>();
        private readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();

        public void AddAssembly(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (_assemblies.Contains(assembly))
            {
                throw new DuplicateWaitObjectException(nameof(assembly), "This assembly is already added");
            }

            _assemblies.Add(assembly);

            var types = assembly
                .DefinedTypes
                .GroupBy(type => CheckTypeOnExportAttribute(type) ? "ExportAttribute" :
                (CheckTypeOnImportConstructorAttribute(type) || CheckTypeOnImportAttribute(type)) ? "ImportAttribute" : string.Empty)
                .ToList();

            var typesWithExportAttribute = types
                .Single(t => t.Key == "ExportAttribute")
                .GroupBy(t => ((ExportAttribute)t.GetCustomAttribute(typeof(ExportAttribute))).Contract == null);

            typesWithExportAttribute
                .Single(t => t.Key == true)
                .ToList()
                .ForEach(t => AddType(t));

            typesWithExportAttribute
                .Single(t => t.Key == false)
                .ToList()
                .ForEach(t => AddType(t, ((ExportAttribute)t.GetCustomAttribute(typeof(ExportAttribute))).Contract));

            types
                .Single(t => t.Key == "ImportAttribute")
                .ToList()
                .ForEach(t => AddType(t));
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

            if (type.IsAbstract || type.IsInterface)
            {
                throw new ArgumentException("Type is abstract class or interface", nameof(type));
            }

            if (!(type.BaseType == baseType) && !(type == baseType) && (!type.GetInterfaces().Contains(baseType)))
            {
                throw new InvalidCastException("Base type is not tied with type");
            }

            _types.Add(baseType, () => GetInstanceWithFilledPublicFieldsThatHaveAttribute(type));

            if (baseType != type)
            {
                _types.Add(type, () => GetInstanceWithFilledPublicFieldsThatHaveAttribute(type));
            }
        }

        public T Get<T>()
        {
            var result = GetInstance(typeof(T));

            if (result == (object)default(T))
            {
                throw new InvalidOperationException("Type was not added to container");
            }

            return (T)result;
        }

        private object GetInstance(Type type)
        {
            if (_typesWithInstances.TryGetValue(type, out var instance))
            {
                return instance;
            }

            if (_types.TryGetValue(type, out var instanceFunc))
            {
                return instanceFunc.Invoke();
            }

            // Exception has been added and return has been commented because of
            // Test 'AddAssembly_AssemblyWithNotEnoughDependencies_ThrowsError' in class 'ContainerTestsComplex'
            throw new KeyNotFoundException("Type was not added to container");

            // return GetDefaultValue(type);
        }

        private object GetInstanceWithFilledPublicFieldsThatHaveAttribute(Type type)
        {
            var instance = CreateInstance(type);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.CustomAttributes.Any(a => a.AttributeType == typeof(ImportAttribute)))
                {
                    property.SetValue(instance, GetInstance(property.PropertyType));
                }
            }

            return instance;
        }

        private object CreateInstance(Type type)
        {
            object[] parameters;
            var constructors = type.GetConstructors().Single();

            parameters = constructors
                .GetParameters()
                .Select(p => GetInstance(p.ParameterType))
                .ToArray();

            var instance = Activator.CreateInstance(type, parameters);

            _typesWithInstances.Add(type, instance);

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

        private bool CheckTypeOnExportAttribute(Type type) => type.CustomAttributes.Any(a => a.AttributeType == typeof(ExportAttribute));

        private bool CheckTypeOnImportConstructorAttribute(Type type) => type.CustomAttributes.Any(a => a.AttributeType == typeof(ImportConstructorAttribute));

        private bool CheckTypeOnImportAttribute(Type type) => type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(property => property.CustomAttributes.Any(a => a.AttributeType == typeof(ImportAttribute)));
    }
}

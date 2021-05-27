using System;
using System.Reflection;

namespace Task2
{
    public static class ObjectExtensions
    {
        public static void SetReadOnlyProperty(this object obj, string propertyName, object newValue)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            }

            SetReadOnlyField(obj, $"<{propertyName}>k__BackingField", newValue);
        }

        public static void SetReadOnlyField(this object obj, string filedName, object newValue)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (string.IsNullOrEmpty(filedName))
            {
                throw new ArgumentException($"'{nameof(filedName)}' cannot be null or empty.", nameof(filedName));
            }

            if (newValue is null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            var type = obj.GetType();
            FieldInfo field = null;

            while (type != null)
            {
                field = type.GetField(filedName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                if (field != null)
                {
                    break;
                }

                type = type.BaseType;
            }

            if (field == null)
            {
                throw new InvalidOperationException($"Field '{filedName}' is not found");
            }

            field.SetValue(obj, Convert.ChangeType(newValue, field.FieldType));
        }
    }
}

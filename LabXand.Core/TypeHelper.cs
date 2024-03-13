using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Ardalis.GuardClauses;

namespace LabXand.Core
{
    public static class TypeHelper
    {
        public delegate T Creator<out T>();

        public static bool IsUnitializedValue(object value)
        {
            if (value == null)
            {
                return true;
            }
            object unitializedValue = CreateUnitializedValue(value.GetType());
            return value.Equals(unitializedValue);
        }

        public static object CreateUnitializedValue(Type type)
        {
            Guard.Against.Null(type);

            if (type.IsGenericTypeDefinition)
                throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.", nameof(type));

            if (type.IsClass || type.IsInterface || type == typeof(void))
                return null;
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            throw new ArgumentException("Type {0} cannot be instantiated.", "type");
        }

        public static Type GetElementType(Type type)
        {
            if (!type.IsPredefinedSimpleType())
            {
                if (type.HasElementType)
                {
                    return GetElementType(type.GetElementType());
                }
                if (type.IsPredefinedGenericType())
                {
                    return GetElementType(type.GetGenericArguments()[0]);
                }
                Type type2 = type.FindIEnumerable();
                if (type2 != null)
                {
                    Type type3 = type2.GetGenericArguments()[0];
                    return GetElementType(type3);
                }
            }
            return type;
        }

        public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
        {
            Guard.Against.Null(dictionaryType);

            Type genericDictionaryType;
            if (dictionaryType.IsSubClass(typeof(IDictionary<,>), out genericDictionaryType))
            {
                if (genericDictionaryType.IsGenericTypeDefinition)
                    throw new Exception($"Type {dictionaryType} is not a dictionary.");

                Type[] dictionaryGenericArguments = genericDictionaryType.GetGenericArguments();

                keyType = dictionaryGenericArguments[0];
                valueType = dictionaryGenericArguments[1];
                return;
            }
            if (!typeof(IDictionary).IsAssignableFrom(dictionaryType))
                throw new Exception($"Type {dictionaryType} is not a dictionary.");
            keyType = null;
            valueType = null;
        }

        public static Type GetDictionaryValueType(Type dictionaryType)
        {
            Type keyType;
            Type valueType;
            GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);

            return valueType;
        }

        public static Type GetDictionaryKeyType(Type dictionaryType)
        {
            Type keyType;
            Type valueType;
            GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);

            return keyType;
        }

        public static IList<string> GetPropertyNames(object target)
        {
            return target.GetType().GetProperties().Select(z => z.Name).ToList();
        }

        public static IEnumerable<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
        {
            return typeof(T).GetFieldsAndProperties(bindingAttr);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
        {
            return GetAttribute<T>(attributeProvider, true);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(attributeProvider, inherit);
            return attributes.FirstOrDefault();
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            Guard.Against.Null(attributeProvider, "attributeProvider");

            return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
        }

        /// <summary>
        /// Gets the value of a property through reflection.
        /// </summary>
        /// <param name="from">The <see cref="object"/> to get the value from.</param>
        /// <param name="propertyName">The name of the property to extract the value for.</param>
        /// <returns>The value of the property.</returns>
        public static object GetPropertyValue(object from, string propertyName)
        {
            Guard.Against.Null(from, "value");
            var propertyInfo = from.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            return propertyInfo.GetValue(from, null);
        }

        public static IEnumerable GetCollectionPropertyValue(object from, string propertyName)
        {
            Guard.Against.Null(from);
            var propertyInfo = from.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            return (IEnumerable)propertyInfo.GetValue(from, null);
        }

        public static bool TryAction<T>(Creator<T> creator, out T output)
        {
            Guard.Against.Null(creator);

            try
            {
                output = creator();
                return true;
            }
            catch
            {
                output = default;
                return false;
            }
        }

        public static bool TryGetDescription(object value, out string description)
        {
            return TryAction(() => GetDescription(value), out description);
        }

        public static string GetDescription(object o)
        {
            Guard.Against.Null(o, "o");

            ICustomAttributeProvider attributeProvider = o as ICustomAttributeProvider;
            Type valueType = o.GetType();

            // object passed in isn't an attribute provider
            // if value is an enum value, get value field member, otherwise get values type
            if (attributeProvider == null)
            {

                if (valueType.IsEnum)
                    attributeProvider = valueType.GetField(o.ToString());
                else
                    attributeProvider = valueType;
            }

            DescriptionAttribute descriptionAttribute = GetAttribute<DescriptionAttribute>(attributeProvider);

            if (descriptionAttribute != null)
                return descriptionAttribute.Description;
            return nameof(valueType);
        }

        public static string GetClassDescription<T>() => GetDescription(typeof(T));

        public static string GetClassDescription(Type type) 
        {
            var descriptionAttribute = type.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute is null ? type.Name : descriptionAttribute.Description;
        }
        public static IList<string> GetDescriptions(IList values)
        {
            Guard.Against.Null(values, "values");

            string[] descriptions = new string[values.Count];

            for (int i = 0; i < values.Count; i++)
            {
                descriptions[i] = GetDescription(values[i]);
            }

            return descriptions;
        }


        public static Type GetSequenceType(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        public static Type GetMemberType(MemberInfo mi)
        {
            FieldInfo fi = mi as FieldInfo;
            if (fi != null)
                return fi.FieldType;
            PropertyInfo pi = mi as PropertyInfo;
            if (pi != null)
                return pi.PropertyType;
            EventInfo ei = mi as EventInfo;
            if (ei != null)
                return ei.EventHandlerType;
            return null;
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

        private static void SetPropertyValue(object inputObject, object value, PropertyInfo property)
        {
            if (property != null && property.CanWrite)
            {
                try
                {
                    property.SetValue(inputObject, value, null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static void SetPropertyValues(object[] inputObjects, string propertyName, object value)
        {
            PropertyInfo property = null;
            foreach (object item in inputObjects)
            {
                if (property == null)
                    property = GetPropertyInfo(item, propertyName);
                SetPropertyValue(item, value, property);
            }
        }

        public static void SetPropertyValue(object inputObject, string propertyName, object value)
        {
            PropertyInfo property = GetPropertyInfo(inputObject, propertyName);
            if (property != null)
                SetPropertyValue(inputObject, value, property);
            else
                throw new Exception(string.Format("{0} data type don't have {1} property.", inputObject.GetType().Name, propertyName));

        }

        private static PropertyInfo GetPropertyInfo(object inputObject, string propertyName)
        {
            if (inputObject != null)
                return inputObject.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return null;
        }

        public static TResult InvokeMethod<TResult, T>(Type type, T instance, string methodName, Type[] types, object[] args)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, types, null);
            if (method != null)
            {
                try
                {
                    var t = (TResult)method.Invoke(instance, args);
                    return t;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    throw;
                }
            }
            else
                throw new Exception(string.Format("{0} does not containe {1} method.", type.Name, methodName));
        }

        public static object InvokeGenericMethod(Type type, string methodName, object obj, object[] arguments, params Type[] genericTypes)
        {
            MethodInfo method = type.GetMethod(methodName);
            return method.MakeGenericMethod(genericTypes).Invoke(obj, arguments);
        }

        public static object InvokeGenericMethod(Type type, string methodName, object obj, Type[] argumentTypes, object[] arguments, params Type[] genericTypes)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null);
            return method.MakeGenericMethod(genericTypes).Invoke(obj, arguments);
        }

        public static bool IsPropertyACollection(object inputObject, string propertyName)
        {
            PropertyInfo prop = GetPropertyInfo(inputObject, propertyName);
            if (prop != null)
                return IsPropertyACollection(prop);
            return false;
        }
        public static bool IsPropertyACollection(PropertyInfo property)
        {
            return property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        public static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
        public static MethodInfo GetEnumerableAnyMethod(Type genericType)
        {
            return typeof(Enumerable).GetMethods().Where(m => m.Name == "Any").FirstOrDefault(m => m.GetParameters().Count() == 2).MakeGenericMethod(genericType);
        }
    }
}
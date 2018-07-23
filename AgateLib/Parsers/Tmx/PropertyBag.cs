using AgateLib.Display;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Parsers.Tmx
{
    public class PropertyBag : IDictionary<string, string>
    {
        private readonly Dictionary<string, string> properties
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public int Count => properties.Count;

        public string this[string key]
        {
            get => properties[key];
            set => properties[key] = value;
        }

        public ICollection<string> Keys => properties.Keys;

        public ICollection<string> Values => properties.Values;

        public bool TryReadProperty(string propertyName, Action<string> storeValue)
        {
            if (properties.ContainsKey(propertyName))
            {
                storeValue(properties[propertyName]);
                return true;
            }

            return false;
        }

        public bool TryReadProperty<T>(string propertyName, Action<T> storeValue, Func<string, T> typeCoercer = null)
        {
            if (properties.ContainsKey(propertyName))
            {
                typeCoercer = typeCoercer ?? (x => (T)Convert.ChangeType(x, typeof(T)));

                T value;
                var text = properties[propertyName];

                try
                {
                    value = typeCoercer(text);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to read property value '{propertyName}': {e.Message}");
                    return false;
                }

                storeValue(value);
                return true;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)properties).GetEnumerator();
        }

        public void Clear()
        {
            properties.Clear();
        }

        public void Add(string key, string value)
        {
            properties.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return properties.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return properties.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return properties.TryGetValue(key, out value);
        }

        public bool TryGetValue(string key, out bool value)
        {
            value = false;

            if (properties.TryGetValue(key, out string s))
            {
                return bool.TryParse(s, out value);
            }

            return false;
        }

        public bool TryGetValue(string key, out int value)
        {
            value = 0;

            if (properties.TryGetValue(key, out string s))
            {
                return int.TryParse(s, out value);
            }

            return false;
        }

        public bool TryGetValue(string key, out float value)
        {
            value = 0;

            if (properties.TryGetValue(key, out string s))
            {
                return float.TryParse(s, out value);
            }

            return false;
        }

        public bool TryGetValue(string key, out Color value)
        {
            value = default(Color);

            if (properties.TryGetValue(key, out string s))
            {
                return ColorX.TryParse(s, out value);
            }

            return false;
        }

        public string ValueOrDefault(string key)
        {
            string result;

            if (!properties.TryGetValue(key, out result))
                result = null;

            return result;
        }

        public T ValueOrDefault<T>(string key, Func<string, T> typeCoercer = null)
        {
            string result;

            if (!properties.TryGetValue(key, out result))
                return default(T);

            if (typeCoercer == null)
                return (T)Convert.ChangeType(result, typeof(T));
            else
                return typeCoercer(result);
        }

        public T? NullableValueOrDefault<T>(string key, Func<string, T> typeCoercer = null) where T : struct
        {
            try
            {
                string result;

                if (!properties.TryGetValue(key, out result))
                    return null;

                if (typeCoercer == null)
                    return (T)Convert.ChangeType(result, typeof(T));
                else
                    return typeCoercer(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Failed to parse value for {key}." + e.ToString());

                return null;
            }
        }

        public T ValueOrDefault<T>(string key, T defaultValue, Func<string, T> typeCoercer = null)
            where T : struct
        {
            string result;

            if (!properties.TryGetValue(key, out result))
                return defaultValue;

            if (typeCoercer == null)
                return (T)Convert.ChangeType(result, typeof(T));
            else
                return typeCoercer(result);
        }

        public T EnumValueOrDefault<T>(string key, T defaultValue, bool ignoreCase = true) where T : struct
        {
            return ValueOrDefault(
                key, defaultValue,
                text => (T)Enum.Parse(typeof(T), text, ignoreCase));
        }

        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
            => ((ICollection<KeyValuePair<string, string>>)properties).Add(item);

        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
            => properties.Contains(item);

        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<string, string>>)properties).CopyTo(array, arrayIndex);

        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
            => ((ICollection<KeyValuePair<string, string>>)properties).Remove(item);

        bool ICollection<KeyValuePair<string, string>>.IsReadOnly
            => ((ICollection<KeyValuePair<string, string>>)properties).IsReadOnly;
    }
}

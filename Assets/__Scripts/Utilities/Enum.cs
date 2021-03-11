using UnityEngine;

namespace Utilities
{
    public static class Enum
    {
        /// <summary>
        /// Gets a random value from the specified enum.
        /// <para/>
        /// Ref: https://stackoverflow.com/a/3132139
        /// </summary>
        public static T RandomValue<T>()
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(Random.Range(0, v.Length));
        }
    }
}

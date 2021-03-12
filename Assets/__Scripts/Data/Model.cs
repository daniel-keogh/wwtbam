using System;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Abstract base class for all models. Just contains an override of ToString()
    /// that prints the JSON representation of the object.
    /// </summary>
    [Serializable]
    public abstract class Model
    {
        public override string ToString() => JsonUtility.ToJson(this, true);
    }
}

using System;
using UnityEngine;

namespace GG.UnityCMS
{
    public class DataAttribute : Attribute
    {
        public Type linkedType;

        public DataAttribute(Type type)
        {
            this.linkedType = type;
        }
    }
}
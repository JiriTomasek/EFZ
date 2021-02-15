using System;

namespace EFZ.Core.NavigationProperty
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class NavigationPropertyAttribute : Attribute
    {
        public NavigationPropertyAttribute()
        {
        }
    }
}
/*
 * User: Thorsten Lorenz
 * Date: 6/24/2009
 * 
*/
using System;

namespace Tools.Extensions
{
    /// <summary>
    /// Contains static methods that substitute for objects methods like ToString, 
    /// but work even if the passed objects are null
    /// </summary>
    public static class NullSafe
    {
        public static string ToStringNullSafe(this object obj)
        {
            return (obj != null) ? obj.ToString() : "NULL" ;
        }
        
        static class ObjectWasNull {}
        public static Type GetTypeNullSafe(this object obj)
        {
            return (obj != null) ? obj.GetType() : typeof(ObjectWasNull);
        }
    }
}
using System;

namespace Tools.Extensions
{
    public static class ArrayComparer
    {
        public static bool EqualsArray(this Array array1, Array array2)
        {
            return ArraysAreEqual(array1, array2);
        }
        
        static bool ArraysAreEqual (Array array1, Array array2)
        {
            if (ReferenceEquals(array1, array2))
                return true;
            
            if((array1 == null && array2 != null) || array1 != null && array2 == null)
                return false;
            
            if(array1.Length != array2.Length)
                return false;


            for(int i = 0; i < array1.Length; i++)
            {
                if(array1.GetValue(i).ToStringNullSafe() != array2.GetValue(i).ToStringNullSafe())
                    return false;
            }
            return true;
        }
    }
}
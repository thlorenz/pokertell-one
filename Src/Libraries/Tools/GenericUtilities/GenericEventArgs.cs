//Date: 5/7/2009
using System;

namespace Tools.GenericUtilities
{
    public class GenericEventArgs<T> : EventArgs
    {
        public GenericEventArgs(T value)
        {
            this.Value = value;
        }
		
        public T Value {get; protected set;}
    }

    public class GenericEventArgs<T1, T2> : EventArgs
    {
        public GenericEventArgs(T1 arg1, T2 arg2)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
        }
	    
        public T1 Arg1 {get; protected set;}
        public T2 Arg2 {get; protected set;}
    }
}
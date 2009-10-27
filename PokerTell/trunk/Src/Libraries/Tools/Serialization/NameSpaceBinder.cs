namespace Tools.Serialization
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Binder that binds from the original namespace to the current one and is applied during binary deserialization
    /// </summary>
    public class NameSpaceBinder : SerializationBinder
    {
        readonly string _originalNameSpace;

        readonly string _currentNameSpace;

        public NameSpaceBinder(string originalNameSpace, string currentNameSpace)
        {
            _originalNameSpace = originalNameSpace;
            _currentNameSpace = currentNameSpace;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName.Contains(_originalNameSpace))
            {
                typeName = typeName.Replace(_originalNameSpace, _currentNameSpace);
            }

            return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
        }
    }
}
namespace Tools.Validation
{
    using Interfaces;

    public class CollectionValidator : ICollectionValidator
    {
        /// <summary>
        /// Corrects the given index to the closest valid value if the given index is out of bound.
        /// </summary>
        /// <param name="index">The desired index</param>
        /// <param name="itemsCount">Number of items in collection</param>
        /// <returns>A valid index. When itemsCount is items cound is 0 it returns 0.</returns>
        public int GetValidIndexForCollection(int index, int itemsCount)
        {
            int validIndex = index;
       
            if (index >= itemsCount)
                validIndex = itemsCount - 1;

            if (validIndex < 0)
                validIndex = 0;
            
            return validIndex;
        }
    }
}
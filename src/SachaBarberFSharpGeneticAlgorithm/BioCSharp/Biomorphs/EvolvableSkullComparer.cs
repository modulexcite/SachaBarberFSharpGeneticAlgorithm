using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioCSharp.Biomorphs
{
    public class EvolvableSkullComparer : IComparer<EvolvableSkullViewModel>
    {
        /// <summary>
        /// Compares two object of a collection
        /// </summary>
        /// <param name="x">Object x of collection</param>
        /// <param name="y">Object y of collection</param>
        /// <returns>A value between -1 and 1, which allows the collection
        /// to be sorted in reference to these 2 objects</returns>
        public int Compare(EvolvableSkullViewModel x, EvolvableSkullViewModel y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                // If x is null and y is not null, y
                // is greater. 
                return -1;
            }
            // If x is not null,and y is null, x is greater 
            if (y == null)
            {
                return 1;
            }
            
            //Both not null so, compare the 2 EvolvableSkullViewModel using the fitness function
            return x.Fitness.CompareTo(y.Fitness);
        }
    }
}

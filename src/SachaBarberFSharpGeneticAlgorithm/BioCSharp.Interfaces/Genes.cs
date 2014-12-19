using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioCSharp.Interfaces
{
    public static class Genes
    {
        // the individual genes are at fixed array positions. So simply declare this nice
        //description names. To hold the array positions, so the array positions may be
        //referenced to later as GenotypeWidths[LEFT_EYE_GENE] etc etc
        public static int FACE_GENE = 0;
        public static int LEFT_EYE_GENE = 1;
        public static int RIGHT_EYE_GENE = 2;
        public static int NOSE_GENE = 3;
        public static int LEFTBONE_TOP_GENE = 4;
        public static int RIGHTBONE_TOP_GENE = 5;
        public static int TEETH_GENE = 6;
        public static int LEFTBONE_BOTTOM_GENE = 7;
        public static int RIGHTBONE_BOTTOM_GENE = 8;
    }
}

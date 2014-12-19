using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using BioCSharp.Interfaces;

namespace BioCSharp.Biomorphs
{
    public class RandomInitialGeneSequence : IInitialGeneSequence
    {
        public double FaceGene { get { return GetRandomValue(); }}
        public double LeftEyeGene { get { return GetRandomValue(); } }
        public double RightEyeGene { get { return GetRandomValue(); } }
        public double NoseGene { get { return GetRandomValue(); } }
        public double LeftBoneTopGene { get { return GetRandomValue(); } }
        public double RightBoneTopGene { get { return GetRandomValue(); } }
        public double TeethGene { get { return GetRandomValue(); } }
        public double LeftBoneBottomGene { get { return GetRandomValue(); } }
        public double RightBoneBottomGene { get { return GetRandomValue(); } }

        private double GetRandomValue()
        {
            return EvolvableSkullViewModel.RAND.NextDouble();
        }
    }
}

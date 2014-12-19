using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioCSharp.Interfaces
{
    public interface IInitialGeneSequence
    {
        double FaceGene { get; }
        double LeftEyeGene { get; }
        double RightEyeGene { get; }
        double NoseGene { get; }
        double LeftBoneTopGene { get; }
        double RightBoneTopGene { get; }
        double TeethGene { get; }
        double LeftBoneBottomGene { get; }
        double RightBoneBottomGene { get; }

    }
}

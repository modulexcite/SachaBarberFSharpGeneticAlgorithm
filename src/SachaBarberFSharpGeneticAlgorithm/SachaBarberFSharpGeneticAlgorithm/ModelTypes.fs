namespace SachaBarberFSharpGeneticAlgorithm

[<AutoOpen>]
module ModelTypes = 

    open System
    open BioCSharp.Interfaces


    //Person record
    type Person = 
        {   
            FaceGene:double; 
            LeftEyeGene:double; 
            RightEyeGene:double; 
            NoseGene:double; 
            LeftBoneTopGene:double; 
            RightBoneTopGene:double; 
            TeethGene:double; 
            LeftBoneBottomGene:double; 
            RightBoneBottomGene:double; 
        }

        interface BioCSharp.Interfaces.IInitialGeneSequence with
            member x.FaceGene = 
                x.FaceGene
            member x.LeftEyeGene = 
                x.LeftEyeGene
            member x.RightEyeGene = 
                x.RightEyeGene
            member x.NoseGene = 
                x.NoseGene
            member x.LeftBoneTopGene = 
                x.LeftBoneTopGene
            member x.RightBoneTopGene = 
                x.RightBoneTopGene
            member x.TeethGene = 
                x.TeethGene
            member x.LeftBoneBottomGene = 
                x.LeftBoneBottomGene
            member x.RightBoneBottomGene = 
                x.RightBoneBottomGene


        override this.ToString() = String.Format("FaceGene : {0}, LeftEyeGene: {1}, RightEyeGene: {2}, NoseGene: {3}, LeftBoneTopGene: {4}, RightBoneTopGene: {5}, TeethGene: {6}, LeftBoneBottomGene: {7}, RightBoneBottomGene: {8}", 
                this.FaceGene, 
                this.LeftEyeGene, 
                this.RightEyeGene, 
                this.NoseGene, 
                this.LeftBoneTopGene, 
                this.RightBoneTopGene, 
                this.TeethGene, 
                this.LeftBoneBottomGene, 
                this.RightBoneBottomGene)
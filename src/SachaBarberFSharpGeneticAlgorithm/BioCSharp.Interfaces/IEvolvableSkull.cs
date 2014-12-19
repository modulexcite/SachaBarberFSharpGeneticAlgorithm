

using System;
using System.Windows.Media;

namespace BioCSharp.Interfaces
{
    public interface IEvolvableSkull
    {
        /// <summary>
        /// Creates initial gene sequences according to the <c>IInitialGeneSequence</c> implementation
        /// provided
        /// </summary>
        void CreateGenes(IInitialGeneSequence initialGeneSequence);

        /// <summary>
        /// Sets the current organisms values to that of the mate
        /// </summary>
        /// <param name="mate">the mate to use</param>
        void SetToOther(IEvolvableSkull mate);

        /// <summary>
        /// Clones the current organism
        /// </summary>
        /// <returns>IEvolvableSkull</returns> which is direct clone of the current organism
        IEvolvableSkull Clone();

        /// <summary>
        /// Takes a evolvable object and uses its genes and combines them
        /// with the current organisms to create a new organism
        /// </summary>
        /// <param name="mate">the mate to use</param>
        /// <returns>IEvolvableSkull</returns> of the combined pair or organisms
        IEvolvableSkull MakeChild(IEvolvableSkull mate);

        /// <summary>
        /// Gets/sets the overall fitness level for the organism
        /// </summary>
        int Fitness { get; set; }

        void ScoreOrganism();

        Guid Id { get; }
        Color[] GenotypeColours { get; }
        int[] GenotypeWidths { get; }
        int[] GenotypeHeights { get; }
        int GenotypeLength { get; }
        double GenotypeMutationRate { get; }
        Color[] AvailableEyeColours { get; }
        Color[] AvailableColours { get; }
        IMainWindowViewModel Parent { get;  }

        bool IsSelected { get; set; }

    }
}

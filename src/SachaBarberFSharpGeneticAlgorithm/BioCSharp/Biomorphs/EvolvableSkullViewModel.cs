using System;
using System.Windows.Media;

using BioCSharp.Interfaces;

namespace BioCSharp.Biomorphs
{
    public class EvolvableSkullViewModel : INPCBase, IEvolvableSkull
    {
        private int fitness = 0;
        private bool isSelected = false;

        public static Random RAND = new Random();
        public static int WIDTH = 150;
        public static int HEIGHT = 150;


        public EvolvableSkullViewModel(IMainWindowViewModel parent, Guid id)
        {
            this.Id = id;

            // number of genes used in the genes array
            this.GenotypeLength = 9; 
            this.GenotypeMutationRate = 0.2; 
            this.GenotypeColours = new Color[9];
            this.GenotypeWidths = new int[9];
            this.GenotypeHeights = new int[9];

            Color[] colours = { Colors.Orange, Colors.Yellow, 
                                Colors.Green, Colors.Cyan, Colors.Magenta, 
                                Colors.CornflowerBlue, Colors.Red, Colors.Chartreuse 
                              };

            Color[] eyeColours = { Colors.Orange, Colors.Yellow, 
                                   Colors.Green, Colors.Cyan, Colors.Magenta, 
                                   Colors.CornflowerBlue, Colors.Red, Colors.Chartreuse 
                                 };

            this.AvailableColours = colours;
            this.AvailableEyeColours = eyeColours;
            this.Parent = parent;
        }



        public void CreateGenes(IInitialGeneSequence initialGeneSequence)
        {
            this.CreateGeneSequence(initialGeneSequence, true);
        }


 
        public void SetToOther(IEvolvableSkull mate)
        {
            this.GenotypeColours = mate.GenotypeColours;
            this.GenotypeWidths = mate.GenotypeWidths;
            this.GenotypeHeights = mate.GenotypeHeights;
            this.AvailableEyeColours = mate.AvailableEyeColours;
            this.AvailableColours = mate.AvailableColours;
        }

        public IEvolvableSkull Clone()
        {
            var theClone = new EvolvableSkullViewModel(this.Parent, Guid.NewGuid());
            theClone.GenotypeLength = this.GenotypeLength;
            theClone.GenotypeMutationRate = this.GenotypeMutationRate;

            for (int i = 0; i < this.GenotypeColours.Length; i++)
                theClone.GenotypeColours[i] = this.GenotypeColours[i];

            for (int i = 0; i < this.GenotypeWidths.Length; i++)
                theClone.GenotypeWidths[i] = this.GenotypeWidths[i];

            for (int i = 0; i < this.GenotypeHeights.Length; i++)
                theClone.GenotypeHeights[i] = this.GenotypeHeights[i];

            for (int i = 0; i < this.AvailableColours.Length; i++)
                theClone.AvailableColours[i] = this.AvailableColours[i];

            for (int i = 0; i < this.AvailableEyeColours.Length; i++)
                theClone.AvailableEyeColours[i] = this.AvailableEyeColours[i];

            return theClone;
        }

        public IEvolvableSkull MakeChild(IEvolvableSkull mate)
        {
            
            int g;

            // Create a new random child
            var child = new EvolvableSkullViewModel(this.Parent, Guid.NewGuid());

            // Set the genes to be a combination of each of the 2 parents.
            // We'll do this by using crossover and then adding mutations.

            // Find crossover point
            var crossoverPoint = (int)(RAND.NextDouble() * this.GenotypeLength);

            // The first genes are from this parent.
            for (g = 0; g < crossoverPoint; g++)
            {

                child.GenotypeColours[g] = this.GenotypeColours[g];
                child.GenotypeWidths[g] = this.GenotypeWidths[g];
                child.GenotypeHeights[g] = this.GenotypeHeights[g];
            }

            // The rest of the genes are from the mate.
            for (g = crossoverPoint; g < GenotypeLength; g++)
            {
                child.GenotypeColours[g] = mate.GenotypeColours[g];
                child.GenotypeWidths[g] = mate.GenotypeWidths[g];
                child.GenotypeHeights[g] = mate.GenotypeHeights[g];
            }

            child.CreateGeneSequence(new RandomInitialGeneSequence(), false);

            // return the new Crossover, Mutated child organism 
            return child;
        }


        
        
        public void ScoreOrganism()
        {
           
                int totalArea = 0;
                int totalScore = 0;
                const int preferredEyeBonus = 200;
                const int preferredColorBonus = 100000;

                // prefer yellow organisms
                Color prefferedCritterColor = AvailableColours[1];
                // prefer blue eyed organisms
                Color preferredEyeColor = AvailableEyeColours[0];


                // Now calculate the area by looking at the genes.

                // get the total area of the organism, based on the product of each of it's individual genes.
                for (int g = 0; g < this.GenotypeWidths.Length; g++)
                {
                    totalArea += this.GenotypeWidths[g] * this.GenotypeHeights[g];
                }

                // Calculate the total
                totalScore += totalArea;

                // check to see if the organism has the preffered eye color, if it does give the 
                // organism a higher "Fitness" score.
                if (this.GenotypeColours[Genes.LEFT_EYE_GENE].Equals(preferredEyeColor) &&
                    this.GenotypeColours[Genes.RIGHT_EYE_GENE].Equals(preferredEyeColor))
                {
                    totalScore += preferredEyeBonus;
                }

                // check to see if the organism has the preffered overall color, if it does give the 
                // organism a higher "Fitness" score.
                int correctColors = 0;
                for (int g = 0; g < this.GenotypeColours.Length; g++)
                {
                    if (this.GenotypeColours[g].Equals(prefferedCritterColor))
                    {
                        correctColors++;
                    }
                }
                totalScore += correctColors*preferredColorBonus;
                Fitness = totalScore;
            
        }

        public int Fitness
        {
            get { return this.fitness; }
            set
            {
                this.fitness = value;
                base.RaisePropertyChanged(() => Fitness);
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                base.RaisePropertyChanged(() => IsSelected);
            }
        }

        public Color[] GenotypeColours { get; private set; }
        public int[] GenotypeWidths { get; private set; }
        public int[] GenotypeHeights { get; private set; }
        public int GenotypeLength { get; private set; }
        public double GenotypeMutationRate { get; private set; }
        public Color[] AvailableEyeColours { get; private set; }
        public Color[] AvailableColours { get; private set; }
        //public bool IsSelected { get; set; }
        public IMainWindowViewModel Parent { get; private set; }
        public Guid Id { get; private set; }

        public override string ToString()
        {
            return string.Format("Fitness: {0}, IsSelected: {1}", Fitness, IsSelected);
        }

        private void CreateGeneSequence(IInitialGeneSequence initialGeneSequence, bool mutate)
        {
            int xSize = WIDTH;
            int ySize = HEIGHT;

            // Set up inital MAX gene heights - centered initially using ySize
            // provided. And MIN is fixed as a constant value for this excercise.
            int maxGeneH = ySize / 2;
            int minGeneH = 20;

            // Set up inital MAX gene widths - centered initially using xSize
            // provided. And MIN is fixed as a constant value for this excercise.
            int maxGeneW = xSize / 3;
            int minGeneW = 20;

            // Set up inital MIN / MAX gene width for eyes using the size
            // of the overall face as limit. 
            int maxGeneEyeW = GenotypeWidths[Genes.FACE_GENE] / 20;
            int minGeneEyeW = 15;

            // Set up inital MIN / MAX gene height for nose using the size
            // of the overall face as limit. 
            int maxGeneNoseH = GenotypeHeights[Genes.FACE_GENE] / 15;
            int minGeneNoseH = 5;

            // Set up inital MIN / MAX gene width for nose using the size
            // of the overall face as limit. 
            int maxGeneNoseW = GenotypeWidths[Genes.FACE_GENE] / 4;
            int minGeneNoseW = GenotypeWidths[Genes.FACE_GENE] / 3;

            // If we are being asked to mutate OR there is a random mutation
            // play god with the genes by changing some of the values within the
            // gene structures.

            // GENE[0] = face rectangle
            if (mutate || (RAND.NextDouble() < GenotypeMutationRate))
            {
                this.GenotypeColours[Genes.FACE_GENE] = AvailableColours[(int)(RAND.NextDouble() * AvailableColours.Length)];
                GenotypeWidths[Genes.FACE_GENE] = (int)((initialGeneSequence.FaceGene * (maxGeneW - minGeneW)) + minGeneW);
                GenotypeHeights[Genes.FACE_GENE] = (int)((initialGeneSequence.FaceGene * (maxGeneH - minGeneH)) + minGeneH);
            }

            // GENE[1] = left eye circle
            if (mutate || (RAND.NextDouble() < GenotypeMutationRate))
            {
                this.GenotypeColours[Genes.LEFT_EYE_GENE] = AvailableEyeColours[(int)(initialGeneSequence.LeftEyeGene * AvailableEyeColours.Length)];
                GenotypeWidths[Genes.LEFT_EYE_GENE] = (int)((initialGeneSequence.LeftEyeGene * (maxGeneEyeW - minGeneEyeW)) + minGeneEyeW);
                // make eye semetrical - round
                GenotypeHeights[Genes.LEFT_EYE_GENE] = GenotypeWidths[Genes.LEFT_EYE_GENE];
            }

            // GENE[2] = right eye circle
            if (mutate || (RAND.NextDouble() < GenotypeMutationRate))
            {
                // make sure that the eyes are both the same color  
                this.GenotypeColours[Genes.RIGHT_EYE_GENE] = this.GenotypeColours[Genes.LEFT_EYE_GENE];
                GenotypeWidths[Genes.RIGHT_EYE_GENE] = (int)((initialGeneSequence.RightEyeGene * (maxGeneEyeW - minGeneEyeW)) + minGeneEyeW);
                // make eye semetrical - round
                GenotypeHeights[Genes.RIGHT_EYE_GENE] = GenotypeWidths[Genes.RIGHT_EYE_GENE];
            }


            // GENE[3] = nose rectangle
            if (mutate || (RAND.NextDouble() < GenotypeMutationRate))
            {
                GenotypeWidths[Genes.NOSE_GENE] = (int)((initialGeneSequence.NoseGene * (maxGeneNoseW - minGeneNoseW)) + minGeneNoseW);
                GenotypeHeights[Genes.NOSE_GENE] = (int)((initialGeneSequence.NoseGene * (maxGeneNoseH - minGeneNoseH)) + minGeneNoseH);
            }


            // GENE[4] = left top bone rectangle
            if (mutate || (RAND.NextDouble() < GenotypeMutationRate))
            {
                this.GenotypeColours[Genes.LEFTBONE_TOP_GENE] = AvailableColours[(int)(initialGeneSequence.LeftBoneTopGene * AvailableColours.Length)];
                // Ensure that right bone can only grow to 1/8 height of the face rectangle
                maxGeneH = GenotypeHeights[Genes.FACE_GENE] / 20;
                GenotypeWidths[Genes.LEFTBONE_TOP_GENE] = (int)((initialGeneSequence.LeftBoneTopGene * (maxGeneW - minGeneW)) + minGeneW);
                GenotypeHeights[Genes.LEFTBONE_TOP_GENE] = (int)((initialGeneSequence.LeftBoneTopGene * (maxGeneH - minGeneH)) + minGeneH);
            }

            // GENE[5] = right top bone rectangle
            // Color is the same as top left bone gene. As it is really the same bone.    
            // But make the right top bone rectangle the same width/height as the left top bone rectangle.
            // I made them different to begin with, but preffered the look of my organism, with uniform
            // bone width, heights. I am not going for the Elephant man Skull type here. Maybe next time.
            this.GenotypeColours[Genes.RIGHTBONE_TOP_GENE] = this.GenotypeColours[Genes.LEFTBONE_TOP_GENE];
            GenotypeWidths[Genes.RIGHTBONE_TOP_GENE] = GenotypeWidths[Genes.LEFTBONE_TOP_GENE];
            GenotypeHeights[Genes.RIGHTBONE_TOP_GENE] = GenotypeHeights[Genes.LEFTBONE_TOP_GENE];

            // GENE[6] = teeth rectangle
            if (mutate || (RAND.NextDouble() < GenotypeMutationRate))
            {
                this.GenotypeColours[Genes.TEETH_GENE] = AvailableColours[(int)(initialGeneSequence.TeethGene * AvailableColours.Length)];
                // Ensure that teeth can only grow to 1/2 width of the face rectangle
                maxGeneW = GenotypeWidths[Genes.FACE_GENE] / 2;
                // Ensure that teeth can only grow to 1/2 height of the total ySize parameter
                maxGeneH = ySize / 2;
                GenotypeWidths[Genes.TEETH_GENE] = (int)((initialGeneSequence.TeethGene * (maxGeneW - minGeneW)) + minGeneW);
                GenotypeHeights[Genes.TEETH_GENE] = (int)((initialGeneSequence.TeethGene * (maxGeneH - minGeneH)) + minGeneH);
            }

            // GENE[7] = left bottom bone rectangle
            // Color is allowed to be random.
            // But make the left bottom bone rectangle the same width/height as the left top bone rectangle
            // could have made them different, but that would involve switching different values when
            // drawing the shape to get the centering on the screen correct. This will do.
            this.GenotypeColours[Genes.LEFTBONE_BOTTOM_GENE] = AvailableColours[(int)(initialGeneSequence.LeftBoneBottomGene * AvailableColours.Length)];
            GenotypeWidths[Genes.LEFTBONE_BOTTOM_GENE] = GenotypeWidths[Genes.LEFTBONE_TOP_GENE];
            GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE] = GenotypeHeights[Genes.LEFTBONE_TOP_GENE];

            // GENE[8] = right bottom bone rectangle
            // Color is the same as bottom left bone gene. As it is really the same bone.
            // But make the right bottom bone rectangle the same width/height as the right top bone rectangle
            // could have made them different, but that would involve switching different values when
            // drawing the shape to get the centering on the screen correct. This will do.
            this.GenotypeColours[Genes.RIGHTBONE_BOTTOM_GENE] = this.GenotypeColours[Genes.LEFTBONE_BOTTOM_GENE];
            GenotypeWidths[Genes.RIGHTBONE_BOTTOM_GENE] = GenotypeWidths[Genes.LEFTBONE_BOTTOM_GENE];
            GenotypeHeights[Genes.RIGHTBONE_BOTTOM_GENE] = GenotypeHeights[Genes.LEFTBONE_BOTTOM_GENE];
        }

    }
}

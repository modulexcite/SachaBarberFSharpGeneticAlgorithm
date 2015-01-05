using BioCSharp.Interfaces;

namespace BioCSharp.Interfaces
{
    public interface IMainWindowViewModel
    {
        void SortPopulationByFitness();
        void NewPopulationFromDominant(IEvolvableSkull chosenDominantOrganism);
    }
}

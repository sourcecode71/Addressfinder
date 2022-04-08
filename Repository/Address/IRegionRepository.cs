using static Addressfinder.Shared.Address.StateList;

namespace Addressfinder.Repository.Address
{
    public interface IRegionRepository
    {
        public  IEnumerable<State> ListOfStates();
    }
}

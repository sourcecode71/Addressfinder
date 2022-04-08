using Addressfinder.Shared.Address;
using static Addressfinder.Shared.Address.StateList;

namespace Addressfinder.Repository.Address
{
    public class RegionRepository : IRegionRepository
    {
        public IEnumerable<State> ListOfStates()
        {
            //TODO: This value will come from the data
            var states = StateList.CreateStateList();
            return states.ToList();
        }
    }
}

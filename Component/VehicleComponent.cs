using Domain;
using Manager;
using Service;

namespace Component
{
    public class VehicleComponent : BaseComponent<Vehicle, VehicleManager>, VehicleService
    {
    }
}

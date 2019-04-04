using Domain;
using Manager;
using Service;

namespace Component
{
    public class ProvinceComponent : BaseComponent<Province, ProvinceManger>, ProvinceService
    {
    }
}

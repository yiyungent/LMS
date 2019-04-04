using Domain;
using Manager;
using Service;

namespace Component
{
    public class DeliveryTypeComponent : BaseComponent<DeliveryType, DeliveryTypeManager>, DeliveryTypeService
    {
    }
}

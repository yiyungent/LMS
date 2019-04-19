using Domain;
using Manager;
using Service;

namespace Component
{
    public class DeliveryFormComponent : BaseComponent<DeliveryForm, DeliveryFormManager>, DeliveryFormService
    {
    }
}

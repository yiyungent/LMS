using Domain;
using Manager;
using Service;

namespace Component
{
    public class TransportOrderComponent : BaseComponent<TransportOrder, TransportOrderManager>, TransportOrderService
    {
    }
}

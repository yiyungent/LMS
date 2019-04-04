using Domain;
using Manager;
using Service;

namespace Component
{
    public class TransportOrderItemComponent : BaseComponent<TransportOrderItem, TransportOrderItemManager>, TransportOrderItemService
    {
    }
}

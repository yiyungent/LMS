using Domain;
using Manager;
using Service;

namespace Component
{
    public class BillingItemComponent : BaseComponent<BillingItem, BillingItemManager>, BillingItemService
    {
    }
}

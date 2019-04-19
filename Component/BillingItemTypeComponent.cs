using Domain;
using Manager;
using Service;

namespace Component
{
    public class BillingItemTypeComponent : BaseComponent<BillingItemType, BillingItemTypeManager>, BillingItemTypeService
    {
    }
}

using Domain;
using Manager;
using Service;

namespace Component
{
    public class BillingComponent : BaseComponent<Billing, BillingManager>, BillingService
    {
    }
}

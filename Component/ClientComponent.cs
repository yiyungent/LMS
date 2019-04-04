using Domain;
using Manager;
using Service;

namespace Component
{
    public class ClientComponent : BaseComponent<Client, ClientManager>, ClientService
    {
    }
}

using Domain;
using Manager;
using Service;

namespace Component
{
    public class DriverComponent : BaseComponent<Driver, DriverManager>, DriverService
    {
    }
}

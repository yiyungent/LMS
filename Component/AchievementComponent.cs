using Domain;
using Manager;
using Service;

namespace Component
{
    public class AchievementComponent : BaseComponent<Achievement, AchievementManager>, AchievementService
    {
    }
}

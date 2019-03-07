using Domain;
using Manager;
using Service;


namespace Component
{
    public class SysUserComponent : BaseComponent<SysUser, SysUserManager>, SysUserService
    {
    }
}

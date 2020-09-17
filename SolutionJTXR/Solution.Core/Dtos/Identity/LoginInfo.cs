using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Dtos.Identity
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户密码
        /// </summary>
        public string Password { get; set; }

        ///// <summary>
        ///// 获取或设置 验证码
        ///// </summary>
        //public string VerifyCode { get; set; }

        ///// <summary>
        ///// 获取或设置 记住登录
        ///// </summary>
        //public bool Remember { get; set; }

        ///// <summary>
        ///// 获取或设置 返回地址
        ///// </summary>
        //public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 修改密码dto
    /// </summary>
    public class ModifyPasswordDto : LoginInfo
    {
        /// <summary>
        /// 获取或设置 用户新密码
        /// </summary>
        public string NewPassword { get; set; }
    }


}

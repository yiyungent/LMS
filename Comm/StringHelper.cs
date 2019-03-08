using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;

namespace Comm
{
    public class StringHelper
    {
        /// <summary>
        /// MD5加密字符串
        /// </summary>
        public static string EncodeMD5(string crude)
        {
            // 实例化MD5构造器
            MD5 md5 = MD5.Create();
            // MD5加密
            byte[] target = md5.ComputeHash(Encoding.UTF8.GetBytes(crude));
            // 字节数组 ——> 字符串
            string ret = string.Empty;
            for (int i = 0; i < target.Length; i++)
            {
                ret = ret + target[i].ToString("X2");
            }
            return ret;
        }
    }
}

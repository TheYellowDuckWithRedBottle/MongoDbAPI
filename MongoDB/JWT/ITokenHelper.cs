using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.JWT
{
    public interface ITokenHelper
    {
        InToken CreateToken<T>(T user) where T : class;
        InToken CreateToken(Dictionary<string, string> keyValuePairs);
        /// <summary>
        /// Token验证
        /// </summary>
        /// <param name="encodeJwt">token</param>
        /// <param name="validatePayLoad">自定义各类验证； 是否包含那种申明，或者申明的值</param>
        /// <returns></returns>
        bool ValiToken(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad = null);
        /// <summary>
        /// 带返回状态的Token验证
        /// </summary>
        /// <param name="encodeJwt">token</param>
        /// <param name="validatePayLoad">自定义各类验证； 是否包含那种申明，或者申明的值</param>
        /// <param name="action"></param>
        /// <returns></returns>
        TokenType ValiTokenState(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad, Action<Dictionary<string, string>> action);
    }
}

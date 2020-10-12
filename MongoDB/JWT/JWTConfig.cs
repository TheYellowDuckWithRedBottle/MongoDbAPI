using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.JWT
{
    public class JWTConfig
    {
        //Token发布者
        public string Issuer { get; set; }
        //Token接受者
        public string Audience { get; set; }
        //密钥
        public string IssuerSigningKey { get; set; }
        //过期时间
        public int AccessTokenExpiresMinutes { get; set; }
    }
}

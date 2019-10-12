using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onsoft.Models
{
    public class UserModel :User
    {
        public string[] UserModule { get; set; }
    }
}
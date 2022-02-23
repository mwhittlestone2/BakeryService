using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryService.Model
{
    public class LoginUser
    {
        public const string TABLENAME = "LoginUser";

        //public const string ID  = "Id";
        public const string EMAIL = "Email";
        public const string PASSWORD = "UserPassword";

        //public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        internal static LoginUser MapReader(SqlDataReader reader)
        {
            LoginUser user = new LoginUser();
            //user.Id = ((Guid)reader[ID]).ToString();
            user.Email = (string)reader[EMAIL];
            user.Password = (string)reader[PASSWORD];

            return user;
        }
    }
}

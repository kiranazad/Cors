using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace CorsDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorsFixController : Controller
    {
        //STEP -3 Add CORS only for perticular method in the controller.
        [Route("loginInWithIlsCredentials")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<LoginResult>> LoginAsync([FromBody] LoginRequest request)
        {
            var data = GetAccessToken();
            return Ok(new LoginResult
            {
                AccessToken = data.AccessToken,
                AccessTokenExpiresIn = data.AccessTokenExpiresIn,
                RefreshToken = data.RefreshToken
            });
        }

        [EnableCors("Policy1")]
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "TESTING";
        }
        private static string GetConnectionString()
        {
            String connString = "User Id=appuser;Password=hillary;Data Source=(DESCRIPTION=(ADDRESS= (PROTOCOL=TCP) (HOST=172.16.99.64)  (PORT=1521) )(CONNECT_DATA= (SID = TEAMJQA) (server = shared)))";
            return connString;
        }

        private static LoginResult GetAccessToken()
        {
            string connectionString = GetConnectionString();
            var loginResult = new LoginResult();
            using (OracleConnection connection = new OracleConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                string sql = "SELECT * FROM INV.ILSAUTH";
                command.CommandText = sql;

                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    loginResult = new LoginResult { 
                         AccessToken = (string)reader["ACCESSTOKEN"],
                         AccessTokenExpiresIn = Convert.ToDouble(reader["EXPIRESIN"]),
                         RefreshToken = (string)reader["REFRESHTOKEN"]
                    };
                }
            }
            return loginResult;
        }
    }
    
    public class LoginResult
    {
        public string AccessToken { get; set; }
        /// <summary>
        /// The number of seconds in which the access token expires
        /// </summary>
        public double AccessTokenExpiresIn { get; set; }

        public string RefreshToken { get; set; }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace WebAppjwt.Controllers
{
    public class ValuesController : ApiController
    {
        //[system.web.http.httpget]
        //public string generatetoken()
        //{
        //    var tokenhandler = new jwtsecuritytokenhandler();
        //    var key = encoding.ascii.getbytes("ui8sfghjkuytrfjjjj898hhhgjh77ujjh6666");
        //    var tokendescriptor = new securitytokendescriptor
        //    {
        //        subject = new claimsidentity(new claim[]
        //        {
        //            new claim(claimtypes.name, "furqan"),

        //        }),
        //        expires = datetime.utcnow.addminutes(1),
        //        signingcredentials = new signingcredentials(new symmetricsecuritykey(key), securityalgorithms.hmacsha256signature)
        //    };

        //    var token = tokenhandler.createtoken(tokendescriptor);
        //    return tokenhandler.writetoken(token);
        //}
        public static void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }
        public static string SignData(string data, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] signature = rsa.SignData(dataBytes, new SHA256CryptoServiceProvider());
                return Convert.ToBase64String(signature);
            }
        }
        public static bool VerifySignature(string data, string signature, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] signatureBytes = Convert.FromBase64String(signature);
                return rsa.VerifyData(dataBytes, new SHA256CryptoServiceProvider(), signatureBytes);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult AuthenticatedAction(string data, string signature)
        {
            string publicKey = "priVatekey678907656789999";
            bool isVerified = VerifySignature(data, signature, publicKey);


            if (VerifySignature(data, signature, publicKey))
            {
                
                return new HttpStatusCodeResult(200, "Authentication successful");
            }
            else
            {
                
                return new HttpStatusCodeResult(200, "Authentication not successful");
            }
        }




        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    
    }
}
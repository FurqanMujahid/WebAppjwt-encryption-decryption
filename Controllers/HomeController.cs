using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAppjwt.Controllers
{
    public class HomeController : Controller
    {

        public System.Web.Mvc.ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        //private readonly string secretKey = "your_secret_key_here";

        //[System.Web.Http.HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    if (username == "admin" && password == "admin")
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = System.Text.Encoding.ASCII.GetBytes(secretKey);
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //            new Claim(ClaimTypes.Name, username)
        //            }),
        //            Expires = DateTime.UtcNow.AddHours(1),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //        };
        //        var token = tokenHandler.CreateToken(tokenDescriptor);
        //        var tokenString = tokenHandler.WriteToken(token);
        //        return ViewBag("founded");
        //    }
        //    else
        //    {
        //        return ViewBag("not found");
        //    }

        //} 


        public class EncryptionHelper
        {
            private static UnicodeEncoding encoder1 = new UnicodeEncoding();

            public static byte[] Encrypt(string data, RSAParameters publicKey)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(publicKey);
                    return rsa.Encrypt(encoder1.GetBytes(data), true);
                }
            }

            public static string Decrypt(byte[] data, RSAParameters privateKey)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(privateKey);
                    var decryptedData = rsa.Decrypt(data, true);
                    return encoder1.GetString(decryptedData);
                }
            }
        }

        
            public System.Web.Mvc.ActionResult EncryptData(string plainText)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    string publicKeyXml = rsa.ToXmlString(false);
                    var publicKey = rsa.ExportParameters(false);

                    byte[] encryptedData = EncryptionHelper.Encrypt(plainText, publicKey);

                    return Content(Convert.ToBase64String(encryptedData));
                }
            }

            public System.Web.Mvc.ActionResult DecryptData(string encryptedText)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    string privateKeyXml = rsa.ToXmlString(true);
                    var privateKey = rsa.ExportParameters(true);

                    byte[] encryptedData = Convert.FromBase64String(encryptedText);

                    string decryptedText = EncryptionHelper.Decrypt(encryptedData, privateKey);

                    return Content(decryptedText);
                }
            }
        


    }
}

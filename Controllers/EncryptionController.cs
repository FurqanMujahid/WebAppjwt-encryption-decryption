using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAppjwt.Controllers
{
    public class EncryptionHelper
    {
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public static byte[] Encrypt(string data, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(_encoder.GetBytes(data), true);
            }
        }

        public static string Decrypt(byte[] data, RSAParameters privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                var decryptedData = rsa.Decrypt(data, true);
                return _encoder.GetString(decryptedData);
            }
        }
    }
    public class EncryptionController : Controller
    {
        // GET: Encryption
            public ActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public ActionResult EncryptData(string plainText)
            {
                var cspParams = new CspParameters
                {
                    Flags = CspProviderFlags.UseMachineKeyStore
                };
                using (var rsa = new RSACryptoServiceProvider(cspParams))
                {
                    var publicKey = rsa.ExportParameters(false);
                    byte[] encryptedData = EncryptionHelper.Encrypt(plainText, publicKey);
                    ViewBag.EncryptedData = Convert.ToBase64String(encryptedData);
                    ViewBag.PublicKeyXml = rsa.ToXmlString(false);
                    return View("Index");
                }
            }

         
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DecryptData(byte[] encryptedText, string privateKeyXml)
        {
            try
            {
                var cspParams = new CspParameters
                {
                    KeyContainerName = "MyKeyContainer",
                    Flags = CspProviderFlags.UseMachineKeyStore
                };


                using (var rsa = new RSACryptoServiceProvider(cspParams))
                {
                    
                   // rsa.FromXmlString(publicKeyXml);
                   // var privateKey = rsa.ExportParameters(true);

                   
                    //byte[] encryptedData = Convert.FromBase64String(encryptedText);

                    
                    //string decryptedText = rsa.Decrypt(encryptedData, true);

                    //ViewBag.DecryptedText = decryptedText;





                    // Import the private key
                    rsa.FromXmlString(privateKeyXml);

                    // Decrypt the data using the private key
                    byte[] decryptedData = rsa.DecryptValue(encryptedText);

                    // Convert the decrypted data back to its original form
                    //string originalData = Convert.ToBase64String(decryptedData);
                    string decryptedString = Encoding.UTF8.GetString(decryptedData);
                    // Display the decrypted data
                    Console.WriteLine("Decrypted data: " + decryptedString);



                    return View("Index");
                }
            }
            catch (Exception ex)
            {
               
                ViewBag.Error = "An error occurred: " + ex.Message;
                return View("Index");
            }
        }


        // GET: Encryption/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Encryption/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Encryption/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Encryption/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Encryption/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Encryption/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Encryption/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

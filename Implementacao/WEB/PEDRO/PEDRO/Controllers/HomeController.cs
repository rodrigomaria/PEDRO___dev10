using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PEDRO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private string outputFile;
        private string inputFile;

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Erro()
        {
            return View();
        }

        private string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        
        public ActionResult Decriptar()
        {
            return View();
        }
        
        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recuperar(int? id)
        {
            return HttpNotFound();
        }
        
        [HttpPost]
        public ActionResult Decriptar(HttpPostedFileBase file, string userKey)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), fileName);
                    outputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), "decriptado");

                    using (RijndaelManaged aes = new RijndaelManaged())
                    {
                        byte[] key = ASCIIEncoding.UTF8.GetBytes(userKey);

                        byte[] IV = ASCIIEncoding.UTF8.GetBytes(userKey);

                        using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                        {
                            using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                            {
                                using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                                {
                                    using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                    {
                                        int data;
                                        while ((data = cs.ReadByte()) != -1)
                                            fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }

                    TempData["Sucesso"] = "Arquivo decriptado com sucesso!";
                    return RedirectToAction("Decriptar");
                }
                catch (Exception ex)
                {
                    TempData["Erro"] = "Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                            "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;

                    return RedirectToAction("Erro");
                }
            }
            else
            {
                TempData["Erro"] = "Selecione um arquivo.";
                return RedirectToAction("Erro");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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

        [Authorize]
        public ActionResult Encriptar()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Encriptar(HttpPostedFileBase file, string userKey)
        {
            if(userKey.Length < 8 || userKey.Length > 16)
            {
                TempData["Erro"] = "A senha do arquivo deve ter no mínimo 8 e no máximo 16 caracteres";
                return RedirectToAction("Erro");
            }
            else
            {
                for(int i = userKey.Length; i < 16; i++) { userKey = string.Concat(userKey, "0"); }
            }

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                inputFile = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(inputFile);
                outputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), "volatil");

                try
                {
                    using (RijndaelManaged aes = new RijndaelManaged())
                    {
                        byte[] key = ASCIIEncoding.UTF8.GetBytes(userKey);

                        byte[] IV = ASCIIEncoding.UTF8.GetBytes(userKey);

                        using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                                {
                                    using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                    {
                                        int data;
                                        while ((data = fsIn.ReadByte()) != -1)
                                            cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }

                    TempData["Sucesso"] = "Arquivo encriptado com sucesso!";
                    return RedirectToAction("Dividir");
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

        [Authorize]
        public ActionResult Decriptar()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dividir()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Dividir(int numDeArqs)
        {
            // pasta padrão
            inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), "volatil");
            
            //particiona arquivo por partes
            byte[] todosOsBytes = System.IO.File.ReadAllBytes(inputFile);
            int x = 0;
            int fatia = todosOsBytes.Length / numDeArqs;
            int sobra = todosOsBytes.Length % numDeArqs;

            for (int i = 0; i < numDeArqs; i++)
            {
                using (FileStream file = new FileStream(inputFile + "pt" + i, FileMode.Create))
                {
                    if (i > 0)
                    {
                        file.Write(todosOsBytes, x, fatia);
                        x += fatia;
                    }
                    else
                    {
                        file.Write(todosOsBytes, x, fatia + sobra);
                        x += fatia + sobra;
                    }
                }

            }
            
            return View("Recuperar");
        }

        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recuperar(int? id)
        {
            using (FileStream recu = new FileStream(Path.Combine(Server.MapPath("~/App_Data/downloads"), "recu"), FileMode.Create))
            {
                for (int i = 0; i < 4; i++)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/App_Data/downloads"), "volatilpt" + i));
                    recu.Write(bytes, 0, bytes.Length);
                }
            }

            return View("Decriptar");
        }

        [Authorize]
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
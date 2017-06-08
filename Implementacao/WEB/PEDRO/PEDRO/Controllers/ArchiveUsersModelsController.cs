using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PEDRO.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PEDRO.DriveUtils;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Drive.v2;
using Google.Apis.Util.Store;
using Google.Apis.Services;

namespace PEDRO.Controllers
{
    [Authorize]
    public class ArchiveUsersModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NomeSortParm = sortOrder == "nomeDoArquivo_asc" ? "nomeDoArquivo_desc" : "nomeDoArquivo_asc";
            ViewBag.TipoSortParm = sortOrder == "tipoArquivo_asc" ? "tipoArquivo_desc" : "tipoArquivo_asc";
            ViewBag.TamanhoSortParm = sortOrder == "tamanhoArquivo_asc" ? "tamanhoArquivo_desc" : "tamanhoArquivo_asc";
            ViewBag.DataSortParm = sortOrder == "dataUpload_asc" ? "dataUpload_desc" : "dataUpload_asc";

            var archives = from s in db.ArchiveUsersModels select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                archives = archives.Where(s => s.nomeDoArquivo.Contains(searchString)
                                       || s.tipoArquivo.Contains(searchString)
                                       || s.tamanhoArquivo.ToString().Contains(searchString)
                                       || s.dataUpload.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "nomeDoArquivo_asc":
                    archives = archives.OrderBy(s => s.nomeDoArquivo);
                    break;
                case "nomeDoArquivo_desc":
                    archives = archives.OrderByDescending(s => s.nomeDoArquivo);
                    break;
                case "tipoArquivo_asc":
                    archives = archives.OrderBy(s => s.tipoArquivo);
                    break;
                case "tipoArquivo_desc":
                    archives = archives.OrderByDescending(s => s.tipoArquivo);
                    break;
                case "tamanhoArquivo_asc":
                    archives = archives.OrderBy(s => s.tamanhoArquivo);
                    break;
                case "tamanhoArquivo_desc":
                    archives = archives.OrderByDescending(s => s.tamanhoArquivo);
                    break;
                case "dataUpload_asc":
                    archives = archives.OrderBy(s => s.dataUpload);
                    break;
                case "dataUpload_desc":
                    archives = archives.OrderByDescending(s => s.dataUpload);
                    break;
                default:
                    archives = archives.OrderBy(s => s.nomeDoArquivo);
                    break;
            }
            string id = User.Identity.GetUserId();
            return View(archives.Where(am => am.user.Id == id).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            if (archiveUsersModels == null)
            {
                return HttpNotFound();
            }
            return View(archiveUsersModels);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, string userKey)
        {
            if (file != null && file.ContentLength > 0 && ModelState.IsValid)
            {
                if (!VerificaNuvens())
                {
                    TempData["Erro"] = "Você deve ter no mínimo um serviço de nuvens cadastrado.";
                    return RedirectToAction("Erro", "Home");
                }

                if (userKey.Length < 8 || userKey.Length > 16)
                {
                    TempData["Erro"] = "A senha do arquivo deve ter no mínimo 8 e no máximo 16 caracteres.";
                    return RedirectToAction("Erro", "Home");
                }
                else
                {
                    for (int i = userKey.Length; i < 16; i++) { userKey = string.Concat(userKey, "0"); }
                }

                ArchiveUsersModels archive = new ArchiveUsersModels
                {
                    nomeDoArquivo = file.FileName,
                    tamanhoArquivo = (file.ContentLength) / 1024,
                    tipoArquivo = file.ContentType,
                    dataUpload = DateTime.Now,
                    hashFileName = CreateNameFileSALT(file.FileName),
                    user = db.Users.Find(User.Identity.GetUserId())
                };

                archive.hashFileName = archive.hashFileName.Replace('/', 'q');
                archive.hashFileName = archive.hashFileName.Replace('+', 'z');
                string fileName = Path.GetFileName(file.FileName);
                string inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), fileName);
                file.SaveAs(inputFile);

                try { Encriptar(inputFile, userKey, archive.hashFileName); }
                catch (Exception ex)
                {
                    TempData["Erro"] = "Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                        "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;

                    return RedirectToAction("Erro", "Home");
                }

                db.ArchiveUsersModels.Add(archive);
                db.SaveChanges();

                Dividir(archive.hashFileName);
                TempData["Sucesso"] = "Arquivo adicionado com sucesso!";
                System.IO.File.Delete(inputFile);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Erro"] = "Selecione um arquivo.";
                return RedirectToAction("Erro", "Home");
            }
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            if (archiveUsersModels == null)
            {
                return HttpNotFound();
            }
            return View(archiveUsersModels);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] ArchiveUsersModels archiveUsersModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archiveUsersModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(archiveUsersModels);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            if (archiveUsersModels == null)
            {
                return HttpNotFound();
            }
            return View(archiveUsersModels);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            db.ArchiveUsersModels.Remove(archiveUsersModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string CreateNameFileSALT(string filename)
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(filename, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new Byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            filename = Convert.ToBase64String(hashBytes);
            return filename;
        }

        private void Encriptar(string filePath, string userKey, string hashFileName)
        {
            var outputFile = Server.MapPath("~/App_Data/downloads/") + hashFileName;
            
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
                            using (FileStream fsIn = new FileStream(filePath, FileMode.Open))
                            {
                                int data;
                                while ((data = fsIn.ReadByte()) != -1)
                                    cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private string Encriptar(string fileName)
        {
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                byte[] key = ASCIIEncoding.UTF8.GetBytes("chavedosbrothers");
                byte[] IV = ASCIIEncoding.UTF8.GetBytes("chavedosbrothers");

                byte[] cryptoPass = Encoding.Default.GetBytes(fileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(cryptoPass, 0, cryptoPass.Length);
                            cs.Close();
                        }
                        fileName = Encoding.Default.GetString(ms.ToArray());
                    }
                }
            }

            return fileName;
        }

        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recuperar(int? id, string userKey)
        {
            if(id != null)
            {
                if (userKey.Length < 8 || userKey.Length > 16)
                {
                    TempData["Erro"] = "A senha do arquivo deve ter no mínimo 8 e no máximo 16 caracteres.";
                    return RedirectToAction("Erro", "Home");
                }
                else
                {
                    for (int i = userKey.Length; i < 16; i++) { userKey = string.Concat(userKey, "0"); }

                    var archive = db.ArchiveUsersModels.Find(id);
                    Download("817702798476-6p6jvc7mp4ehprtknj0v01ngmv8d6sks.apps.googleusercontent.com", "DYSG6s8EYCfCwbkr8Oq5_j7V", archive.hashFileName);

                    try
                    {
                        Decriptar(userKey, id);

                        TempData["Sucesso"] = "Arquivo decriptado com sucesso!";
                        return RedirectToAction("Index");
                    }
                    catch(Exception ex)
                    {
                        TempData["Erro"] = "Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                            "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;

                        return RedirectToAction("Erro", "Home");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private int CloudCount()
        {
            int c = 0;
            foreach(var item in db.CloudModels.ToList())
            {
                if (item.user.Id == User.Identity.GetUserId())
                    c++;
            }

            return c;
        }

        private bool VerificaNuvens()
        {
            var nuvens = db.CloudModels.ToList();
            foreach(var item in nuvens)
            {
                if(item.user.Id == User.Identity.GetUserId())
                    return true;
            }

            return false;
        }

        private void Dividir(string fileNameHash)
        {    
            int numDeArqs = CloudCount();

            // pasta padrão
            var inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), fileNameHash);

            //particiona arquivo por partes
            byte[] todosOsBytes = System.IO.File.ReadAllBytes(inputFile);
            int x = 0;
            int fatia = todosOsBytes.Length / numDeArqs;
            int sobra = todosOsBytes.Length % numDeArqs;

            for (int i = 0; i < numDeArqs; i++)
            {
                string path = inputFile + "pt" + i;
                string nome = "";
                using (FileStream file = new FileStream(path, FileMode.Create))
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
                    nome = fileNameHash + i;
                }
                
                GoogleDriveUtil drive = new GoogleDriveUtil("817702798476-6p6jvc7mp4ehprtknj0v01ngmv8d6sks.apps.googleusercontent.com", "DYSG6s8EYCfCwbkr8Oq5_j7V");
                drive.upload(path, nome);

                System.IO.File.Delete(path);
            }
            System.IO.File.Delete(inputFile);
        }

        private void Download(string clientId, string clientSecret, string fileNameHash)
        {
            string fileId = "";

            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile },
            Environment.UserName,
            CancellationToken.None,
            new FileDataStore("Daimto.GoogleDrive.Auth.Store")).Result;

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Pedro"
            });

            //lista
            int clouds = CloudCount();
            FilesResource.ListRequest listResquest = service.Files.List();
            IList<Google.Apis.Drive.v2.Data.File> files = listResquest.Execute().Items;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    for(int i = 0; i < clouds; i++)
                    {
                        if (file.Title.Equals(fileNameHash + i))
                        {
                            fileId = file.Id;
                            var request = service.Files.Get(fileId);
                            var stream = new System.IO.MemoryStream();
                            request.Download(stream);

                            using (FileStream f = new FileStream(Path.Combine(Server.MapPath("~/App_Data/downloads/"), fileNameHash + i), FileMode.Create))
                            {
                                stream.WriteTo(f);
                            }
                            break;
                        }
                    }
                }
            }
            
            using (FileStream recu = new FileStream(Path.Combine(Server.MapPath("~/App_Data/downloads"), "recu"), FileMode.Create))
            {
                for (int i = 0; i < clouds; i++)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/App_Data/downloads"), fileNameHash + i));
                    recu.Write(bytes, 0, bytes.Length);
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/App_Data/downloads"), fileNameHash + i));
                }
            }
        }

        public void Decriptar(string userKey, int? id)
        {
            var nome = db.ArchiveUsersModels.Find(id).nomeDoArquivo;
            var inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), "recu");
            var outputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), nome);

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

            System.IO.File.Delete(inputFile);
        }
    }
}
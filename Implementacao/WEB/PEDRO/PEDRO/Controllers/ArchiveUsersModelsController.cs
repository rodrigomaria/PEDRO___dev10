﻿using System;
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
using CG.Web.MegaApiClient;

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
                                       || s.extensao.Contains(searchString)
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
                    archives = archives.OrderBy(s => s.extensao);
                    break;
                case "tipoArquivo_desc":
                    archives = archives.OrderByDescending(s => s.extensao);
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

            //List<ArchiveUsersModels> lista = new List<ArchiveUsersModels>();
            //foreach(var arq in db.ArchiveUsersModels.ToList())
            //{
            //    if(id == arq.user.Id) { lista.Add(arq); }
            //}
            
            //foreach(var s in db.SharedArchiveModels.ToList())
            //{
            //    if (id == s.usuario_id) { lista.Add(db.ArchiveUsersModels.Find(s.arquivo_id)); }
            //}

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
                    nomeDoArquivo = Path.GetFileNameWithoutExtension(file.FileName),
                    tamanhoArquivo = (file.ContentLength) / 1024,
                    extensao = Path.GetExtension(file.FileName),
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
        public ActionResult Edit([Bind(Include = "id,nomeDoArquivo,dataUpload,hashFileName,servicosNuvem,tamanhoArquivo,extensao")] ArchiveUsersModels archiveUsersModels)
        {
            archiveUsersModels.user = db.Users.Find(User.Identity.GetUserId());
            db.Entry(archiveUsersModels).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
            ArchiveUsersModels archive = db.ArchiveUsersModels.Find(id);
            string fileId = "";

            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = "817702798476-6p6jvc7mp4ehprtknj0v01ngmv8d6sks.apps.googleusercontent.com",
                ClientSecret = "DYSG6s8EYCfCwbkr8Oq5_j7V"
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

            MegaUtil mega = new MegaUtil("", "");
            
            FilesResource.ListRequest listResquest = service.Files.List();
            listResquest.Q = "'root' in parents and trashed = false";
            IList<Google.Apis.Drive.v2.Data.File> files = listResquest.Execute().Items;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Title.Equals(archive.hashFileName + "0"))
                    {
                        fileId = file.Id;
                        service.Files.Delete(fileId).Execute();
                    }
                }

                mega.DeleteFile(archive.hashFileName + "1");
            }

            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            db.ArchiveUsersModels.Remove(archiveUsersModels);

            foreach(var item in db.SharedArchiveModels)
            {
                if (item.arquivo_id == id)
                    db.SharedArchiveModels.Remove(item);
            }

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
                    
                    try
                    {
                        Download("817702798476-6p6jvc7mp4ehprtknj0v01ngmv8d6sks.apps.googleusercontent.com", "DYSG6s8EYCfCwbkr8Oq5_j7V", db.ArchiveUsersModels.Find(id).hashFileName);
                    }
                    catch(Exception ex)
                    {
                        TempData["Erro"] = "Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                            "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;

                        return RedirectToAction("Erro", "Home");
                    }
                    

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
        
        private void Dividir(string fileNameHash)
        {
            // pasta padrão
            var inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), fileNameHash);

            //particiona arquivo por partes
            byte[] todosOsBytes = System.IO.File.ReadAllBytes(inputFile);
            int x = 0;
            int fatia = todosOsBytes.Length / 2;
            int sobra = todosOsBytes.Length % 2;

            for (int i = 0; i < 2; i++)
            {
                string path = inputFile + i;
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

                if (i == 0)
                {
                    GoogleDriveUtil drive = new GoogleDriveUtil("817702798476-6p6jvc7mp4ehprtknj0v01ngmv8d6sks.apps.googleusercontent.com", "DYSG6s8EYCfCwbkr8Oq5_j7V");
                    drive.upload(path, nome);
                }
                else
                {
                    MegaUtil mega = new MegaUtil("", "");
                    mega.upload(path, nome);
                }
                
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

            MegaApiClient client = new MegaApiClient();
            client.Login(
                "nuvem.pedro@gmail.com",
                "pedroehdemais");

            var nodes = client.GetNodes();
            INode root = nodes.Single(n => n.Type == NodeType.Root);

            //lista
            FilesResource.ListRequest listResquest = service.Files.List();
            listResquest.Q = "'root' in parents and trashed = false";
            IList<Google.Apis.Drive.v2.Data.File> files = listResquest.Execute().Items;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Title.Equals(fileNameHash + "0"))
                    {
                        fileId = file.Id;
                        var request = service.Files.Get(fileId);
                        var stream = new System.IO.MemoryStream();
                        request.Download(stream);

                        using (FileStream f = new FileStream(Path.Combine(Server.MapPath("~/App_Data/downloads/"), fileNameHash + "0"), FileMode.Create))
                        {
                            stream.WriteTo(f);
                        }
                        break;
                    }
                }

                INode myFile = nodes.Single(n => n.Name == fileNameHash + "1");
                client.DownloadFile(myFile, Path.Combine(Server.MapPath("~/App_Data/downloads/" + fileNameHash + "1")));
            }
            
            using (FileStream recu = new FileStream(Path.Combine(Server.MapPath("~/App_Data/downloads"), "recu"), FileMode.Create))
            {
                for (int i = 0; i < 2; i++)
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
            var extensao = db.ArchiveUsersModels.Find(id).extensao;
            var inputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), "recu");
            var outputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), nome + extensao);

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

        public ActionResult Share(int? id)
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Share(string email, int arquivoId)
        {
            if (email == null || email.Equals(""))
            {
                TempData["Erro"] = "Insira um e-mail de usuário.";
                return RedirectToAction("Erro", "Home");
            }

            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction("Erro", "Home");
            }

            SharedArchiveModel shared = new SharedArchiveModel
            {
                proprietario_id = User.Identity.GetUserId(),
                arquivo_id = arquivoId,
                usuario_id = user.Id
            };

            db.SharedArchiveModels.Add(shared);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Unshare(int? id)
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Unshare(int arquivoId)
        {
            var shared = db.SharedArchiveModels.Where(s => s.arquivo_id == arquivoId);

            foreach(var item in shared)
            {
                db.SharedArchiveModels.Remove(item);
            }
            
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
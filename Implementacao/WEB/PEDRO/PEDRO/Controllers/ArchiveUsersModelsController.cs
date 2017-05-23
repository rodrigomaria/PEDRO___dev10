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

namespace PEDRO.Controllers
{
    [Authorize]
    public class ArchiveUsersModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            return View(db.ArchiveUsersModels.Where(am => am.user.Id == id).ToList());
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

                ArchiveUsersModels archiveUsersModels = new ArchiveUsersModels
                {
                    nomeDoArquivo = file.FileName,
                    tamanhoArquivo = file.ContentLength,
                    tipoArquivo = file.ContentType,
                    dataUpload = DateTime.Now,
                    user = db.Users.Find(User.Identity.GetUserId())
                };

                string fileName = Path.GetFileName(file.FileName);
                string inputFile = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(inputFile);

                try { Encriptar(inputFile, userKey); }
                catch (Exception ex)
                {
                    TempData["Erro"] = "Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                        "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;

                    return RedirectToAction("Erro", "Home");
                }

                db.ArchiveUsersModels.Add(archiveUsersModels);
                db.SaveChanges();

                Dividir();
                TempData["Sucesso"] = "Arquivo adicionado com sucesso!";
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

        private void Encriptar(string filePath, string userKey)
        {
            var outputFile = Path.Combine(Server.MapPath("~/App_Data/downloads"), "volatil");
            
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

        private int CloudCount()
        {
            int c = 0;
            foreach(var item in db.CloudModels)
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

        private void Dividir()
        {
            int numDeArqs = CloudCount();

            // pasta padrão
            var path = Path.Combine(Server.MapPath("~/App_Data/downloads"), "volatil");

            //particiona arquivo por partes
            byte[] todosOsBytes = System.IO.File.ReadAllBytes(path);
            int x = 0;
            int fatia = todosOsBytes.Length / numDeArqs;
            int sobra = todosOsBytes.Length % numDeArqs;

            for (int i = 0; i < numDeArqs; i++)
            {
                using (FileStream file = new FileStream(path + "pt" + i, FileMode.Create))
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
        }
    }
}
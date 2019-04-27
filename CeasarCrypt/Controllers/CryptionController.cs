using CeasarCrypt.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CeasarCrypt.Controllers
{
    public class CryptionController : Controller
    {
        private string GetNeededText(HttpPostedFileBase file, string text)
        {
            if (file != null)
            {
                var fileName = file.FileName;

                if (fileName.EndsWith(".txt"))
                {
                    return TextService.GetTextFromTxt(file.InputStream);
                }

                if (fileName.EndsWith(".docx"))
                {
                    return TextService.GetTextFromDocx(file.InputStream);
                }
            }
            else if (!string.IsNullOrEmpty(text))
            {
                return text;
            }

            return null;
        }

        public ActionResult Cryption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cryption(HttpPostedFileBase file, string text, int key)
        {
            if (file == null && text.Length < 1)
                return RedirectToAction("Cryption", "Cryption");

            text = GetNeededText(file, text);
            if (text == null) return RedirectToAction("Cryption");
            string fileName = file?.FileName ?? "Noname.txt";

            ViewBag.Key = key;
            ViewBag.Text = Crypt.Encryption(key, text);
            ViewBag.FileName = fileName;

            return View("~/Views/Result/Cryption.cshtml");
        }

        public ActionResult Decryption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Decryption(HttpPostedFileBase file, string text, int key)
        {
            if (file == null && text.Length < 1)
                return RedirectToAction("Decryption");

            text = GetNeededText(file, text);
            if (text == null) return RedirectToAction("Decryption");
            string fileName = file?.FileName ?? "Noname.txt";

            ViewBag.Key = key;
            ViewBag.Text = Crypt.Decryption(key, text);
            ViewBag.FileName = fileName;

            return View("~/Views/Result/Decryption.cshtml");
        }

        public ActionResult FindKey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindKey(HttpPostedFileBase file, string text)
        {
            if (file == null && text.Length < 1)
                return RedirectToAction("FindKey");

            text = GetNeededText(file, text);
            if (text == null) return RedirectToAction("FindKey");
            string fileName = file?.FileName ?? "Noname.txt";

            int[] keys = Crypt.GetKeys(text);
            string[] texts = keys.Select(key => Crypt.Decryption(key, text)).ToArray();
            
            ViewBag.FileName = fileName;
            ViewBag.Keys = keys;
            ViewBag.Texts = texts;

            return View("~/Views/Result/FindKey.cshtml");
        }

        public ActionResult Download(string text, string fileName)
        {
            string appType;
            byte[] bytes;
            if (fileName.EndsWith(".txt"))
            {
                appType = "text/plain";
                bytes = TextService.GetTxtFileWithText(text);
            }
            else if (fileName.EndsWith(".docx"))
            {
                appType = "application/msword";
                bytes = TextService.GetDocFileWithText(text);
            }
            else
            {
                return null;
            }

            return File(bytes, appType, fileName);
        }
    }
}
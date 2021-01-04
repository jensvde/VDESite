using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VandenEynde.Helpers;

namespace VandenEynde.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Database/Upload/")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpGet("/Database/Download/")]
        public IActionResult Download()
        {
            return View();
        }

        [HttpGet("/Database/DbExport/")]
        public async Task<ActionResult> DbExport(string Database)
        {
            await $"/home/{Environment.UserName}/mysql.sh --export {Database} /home/{Environment.UserName}/{Database}.db".Bash();
            string filePath = "/home/" + Environment.UserName + "/" + Database + ".db";
            string fileName = Database + "_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".db";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", fileName);
        }

        [HttpGet("/Database/Reboot/")]
        public async Task<IActionResult> RebootAsync()
        {
            await $"/usr/bin/shutdown -ar now".Bash();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> UploadDbUsers(IFormFile formFile)
        {
            long size = formFile.Length;
            string filePath = "";

            if (formFile.Length > 0)
            {
                // full path to file in temp location
                filePath = "/home/" + Environment.UserName + "/db_users.db"; //we are using Temp file name just for the example. Add your own file path.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            await $"/home/{Environment.UserName}/mysql.sh --import db_users /home/{Environment.UserName}/db_users.db".Bash();
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return Ok(new { size, filePath });
            return RedirectToAction("IndexAdmin", "Database");

        }
        public async Task<IActionResult> UploadDb(IFormFile formFile)
        {
            long size = formFile.Length;
            string filePath = "";

            if (formFile.Length > 0)
            {
                // full path to file in temp location
                filePath = "/home/" + Environment.UserName + "/db.db"; //we are using Temp file name just for the example. Add your own file path.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            await $"/home/{Environment.UserName}/mysql.sh --import db /home/{Environment.UserName}/db.db".Bash();
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //return Ok(new { size, filePath});
            return RedirectToAction("IndexAdmin", "Database");
        }
    }
}

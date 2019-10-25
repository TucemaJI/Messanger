using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Messenger.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;


namespace Messenger.Data.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContext db;

        public HomeController(ILogger<HomeController> logger, UserContext context)
        {
            _logger = logger;
            db = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<Message> messages = db.Messages.Where(x => x.User.Name == User.Identity.Name);

            return View(messages);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostMessage(string content)
        {
            var message = new Message();
            message.Name = content;
            message.UserId = db.Users.Where(x => x.Name == User.Identity.Name).FirstOrDefault().Id;
            message.MessageType = MessageType.Text;
            db.Messages.Add(message);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult PostUpload(List<IFormFile> files)
        {
            if (files != null)
            {
                var file = files.FirstOrDefault();
                var message = new Message();
                using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                {
                    message.Content = reader.ReadToEnd();
                }
                message.Name = file.FileName;
                message.UserId = db.Users.Where(x => x.Name == User.Identity.Name).FirstOrDefault().Id;
                if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".img"))
                {
                    message.MessageType = MessageType.Image;
                }
                else { message.MessageType = MessageType.File; }
                db.Messages.Add(message);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }

        public static Stream GenerateStreamFromString(string s)
{
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write(s);
    writer.Flush();
    stream.Position = 0;
    return stream;
}

        [HttpGet]
        public ActionResult GetImage(long messageId)
        {
            var message = db.Messages.Find(messageId);
                byte[] byteArray = Encoding.ASCII.GetBytes(message.Content);
                MemoryStream stream = new MemoryStream(byteArray);
            var file = File(byteArray, "image/jpg");
            return file;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

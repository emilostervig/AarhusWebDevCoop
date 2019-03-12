using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: Default
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        // POST: Sending form
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            MailMessage message = new MailMessage();
            message.To.Add("eaaeao@students.eaaa.dk");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;


            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("emil.ostervig2@gmail.com", "qtgaaxspbnbihnnj");
             
               
                TempData["success"] = true;
                // send mail
                smtp.Send(message);

                IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

                comment.SetValue("messageName", model.Name);
                comment.SetValue("email", model.Email);
                comment.SetValue("subject", model.Subject);
                comment.SetValue("messageContent", model.Message);

                // save without publishing
                Services.ContentService.Save(comment);

                // save and publish
                //Services.ContentService.SaveAndPublishWithStatus(comment);

                return RedirectToCurrentUmbracoPage();

                
            
            }


            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PWChange2.Models;
using PWChange2.Classes;


namespace PWChange2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {

            if (TempData["Pass"] == null)
                TempData["Pass"] = "";
            if (TempData["Exception"] == null)
                TempData["Exception"] = "";
            ViewData["ExceptionMessage"] = TempData["Exception"];
            ViewData["Pass"] = TempData["Pass"];
            return View();
        }

        public IActionResult Complete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePW([Bind("UserName,CurrentPassword,NewPassword,ConfirmPassword")] User userObject)
        {
            //Validating ReCaptcha
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

            String displayName = "";

            if (IsCaptchaValid)
            {
                if (userObject.confirmNewPwEqualsConfirmPw())
                {
                    using (var context = new PrincipalContext(ContextType.Domain))
                    {
                        try
                        {
                            var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userObject.UserName);

                            if (user == null)
                                throw new Exception("Username does not Exist, Please enter a valid Username");

                            user.ChangePassword(userObject.CurrentPassword, userObject.NewPassword);
                            displayName = user.Name;
                            user.Save();
                        }
                        catch (PasswordException e)
                        {
                            TempData["Exception"] = e.Message.Remove(e.Message.Length - 36);
                            return RedirectToAction("Index");
                        }
                        catch (PrincipalOperationException e)
                        {
                            TempData["Exception"] = e.Message.Remove(e.Message.Length - 36);
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            TempData["Exception"] = e.Message;
                            return RedirectToAction("Index");
                        }
                    }
                }
            }else
            {
                TempData["Exception"] = "Invalid ReCaptcha Response - Please ensure the I'm not a robot check box is ticked";
                return RedirectToAction("Index");
            }

            TempData["Pass"] = "Password Changed for " + displayName;
            return RedirectToAction("Index");
        }



    public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }



}

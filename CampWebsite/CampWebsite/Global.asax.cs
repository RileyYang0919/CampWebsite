
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace CampWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(Application_AuthenticateRequest);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
            {
                return;
            }
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authCookie.Expires = DateTime.Now.AddDays(7); //登入後自動延長七天
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }
            string[] roles = authTicket.UserData.Split(new char[] { ',' });
            //Session["myRoles"] = roles[0];
            //(new HomeController()).getUserName(roles[0]); 
            if (Context.User != null)
            {
                Context.User = new System.Security.Principal.GenericPrincipal(Context.User.Identity, roles);
            }
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                Session["currentUserName"] = ticket.UserData.Split(new char[] { ',' })[1];
            }

        }
    }
}

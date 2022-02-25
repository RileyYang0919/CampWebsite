using System;
using System.Collections.Generic;
using System.Web.Optimization;
using System.Linq;
using System.Web;

namespace CampWebsite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundleScripts").Include(
                        "~/Scripts/jQuery/jquery-3.6.0.min.js"));
            bundles.Add(new ScriptBundle("~/bundleScripts").Include(
                        "~/Scripts/Bootstrap/bootstrap.bundle.min.js"));
            
            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/themes/base/jquery-ui.css",
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundleCss").Include(
                      "~/Content/Bootstrap/bootstrap.min.css"));

            BundleTable.EnableOptimizations = true;

            //var jqueryCdnPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js";
            //bundles.Add(new ScriptBundle("~/cdn/jquery", jqueryCdnPath).Include(
            //            "~/Scripts/jquery-{version}.js"));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Controls.Navigation;

public class UrlNavigationHelper
{
    public bool isPop = false;
    public bool isMultiPop = false;
    public bool isMultiPopAndPush = false;
    public bool isPush = false;
    public bool isPushAsRoot = false;
    public bool isMultiPushAsRoot = false;

    public static UrlNavigationHelper Parse(string url)
    {
        var obj = new UrlNavigationHelper();
        if (url == "../")
        {
            obj.isPop = true;
        }
        else if (url.Contains("../"))
        {
            obj.isMultiPop = url.Replace("../", "") == string.Empty;
            obj.isMultiPopAndPush = !obj.isMultiPop;
        }
        else if (url.Contains("/"))
        {
            var pages = url.Split("/").Where(s => !string.IsNullOrEmpty(s)).ToList();

            if(pages.Count>1)
            {
                obj.isMultiPushAsRoot = true;
            }
            else
            {
                obj.isPushAsRoot = true;
            }
        }
        else
        {
            obj.isPush = true;
        }

        return obj;
    }
}

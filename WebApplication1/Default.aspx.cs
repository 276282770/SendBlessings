using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        string rsvUrl = "http%3a%2f%2fzf.cracre.vip%2fRsvWXSnsapiBase.ashx";
        const string url_wxbase = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx34ee0c35e400d9d6&redirect_uri=http%3a%2f%2fzf.cracre.vip%2fRsvWXSnsapiBase.ashx&response_type=code&scope=snsapi_userinfo&state=yh#wechat_redirect";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect(url_wxbase);
        }
    }
}
<%@ WebHandler Language="C#" Class="SessionHandler" %>

using System;
using System.Web;
using System.Web.SessionState;

public class SessionHandler : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetNoStore();
        context.Response.ContentType = "application/x-javascript";
        context.Response.Write("//");
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Codaxy.Dextop.Remoting
{
    class PollingResult
    {
        public string type { get; set; }
        public string name { get; set; }
        public bool success { get; set; }
        public object data { get; set; }
    }

	/// <summary>
	/// A HTTP handler responsible for Ext.direct polling requests.
	/// </summary>
    public class DextopPollingHandler : IHttpHandler
    {
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            PollingResult result;
            try
            {
                var session = GetSession(context);
                result = session.HandlePollingRequest(context);
            }
            catch (Exception ex)
            {
                //Application should handle the request and give a valid result.
                //Any exception is a sign that session is not valid anymore.
                result = new PollingResult
                {
                    type = "rpc",
                    name = "message",
                    success = false,
                    data = new DextopRemoteMethodCallException
                    {
                        exception = ex.Message,
                        type = "session"
                    }
                };
            }

            context.Response.ContentType = "application/json";
            DextopUtil.Encode(result, context.Response.Output);
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable { get { return true; } }

        DextopSession GetSession(HttpContext context)
        {
            var appKey = context.Request.QueryString["app"];
            var app = DextopApplication.GetApplication(appKey);
            var sessionId = context.Request.QueryString["sid"];
            var session = app.GetSession(sessionId);
            return session;
        }
    }
}

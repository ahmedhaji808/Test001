using System;
using System.Web;

namespace AccountingSystem
{
	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			// Application start-up logic can be placed here
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception error = Server.GetLastError();
			// Consider logging error
		}
	}
}
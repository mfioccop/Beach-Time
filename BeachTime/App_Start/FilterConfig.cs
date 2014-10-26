namespace BeachTime
{
	using System.Web;
	using System.Web.Mvc;

	/// <summary>
	/// Registered global filters that will be applied to every Action and Controller
	/// </summary>
	public class FilterConfig
	{
		/// <summary>
		/// Adds global filters to the application
		/// </summary>
		/// <param name="filters">The GlobalFilterCollection that filters will be added to</param>
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new System.Web.Mvc.AuthorizeAttribute());
			filters.Add(new RequireHttpsAttribute());
		}
	}
}

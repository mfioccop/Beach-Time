using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using BeachTime.Data;

namespace BeachTime.Models
{
	/// <summary>
	/// ViewModel for the Account Executive index page
	/// </summary>
    public class ExecutiveIndexViewModel : NavbarViewModelBase
    {
		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
        [DisplayName("First Name")]
        public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
        [DisplayName("Last Name")]
        public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the occupied consultants count.
		/// </summary>
		/// <value>
		/// The occupied consultants count.
		/// </value>
        [DisplayName("Occupied Consultants")]
        public int OccupiedConsultantsCount { get; set; }

		/// <summary>
		/// Gets or sets the beach consultants count.
		/// </summary>
		/// <value>
		/// The beach consultants count.
		/// </value>
        [DisplayName("Beach Consultants")]
        public int BeachConsultantsCount { get; set; }

		/// <summary>
		/// Gets or sets the skill list.
		/// </summary>
		/// <value>
		/// The skill list.
		/// </value>
        [DisplayName("Skills")]
        public List<string> SkillList { get; set; }

    }

	/// <summary>
	/// ViewModel for a list of consultants.
	/// </summary>
    public class ExecutiveUserListViewModel : NavbarViewModelBase
    {
		/// <summary>
		/// Gets or sets the beach consultant view models.
		/// </summary>
		/// <value>
		/// The beach consultant view models.
		/// </value>
        [DisplayName("List of Consultants")]
        public List<ConsultantIndexViewModel> BeachConsultantViewModels { get; set; }
   }

}
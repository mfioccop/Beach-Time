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
    public class ExecutiveIndexViewModel : NavbarViewModelBase
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Occupied Consultants")]
        public int OccupiedConsultantsCount { get; set; }

        [DisplayName("Beach Consultants")]
        public int BeachConsultantsCount { get; set; }

        [DisplayName("Skills")]
        public List<string> SkillList { get; set; }

    }

    public class ExecutiveUserListViewModel : NavbarViewModelBase
    {
        [DisplayName("List of Consultants")]
        public List<ConsultantIndexViewModel> BeachConsultantViewModels { get; set; }
   }

}
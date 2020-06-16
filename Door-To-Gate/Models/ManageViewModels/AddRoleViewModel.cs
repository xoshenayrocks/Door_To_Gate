using System;
using System.ComponentModel.DataAnnotations;

namespace Door_To_Gate.Models.ManageViewModels
{
    public class AddRoleViewModel
    {
            [Display(Name = "Role Name")]
            [Required]
            public string RoleName { get; set; }
       
    }
}

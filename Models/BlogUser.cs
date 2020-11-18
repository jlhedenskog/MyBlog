using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class BlogUser : IdentityUser 
    {
        #region Keys
        #endregion

        #region BlogUser Properties
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        
        public byte[] ProfilePicture { get; set; }
        #endregion

        #region Navigation
        public virtual ICollection<Comment> Comments { get; set; }

        public BlogUser()
        {
            Comments = new HashSet<Comment>();
            DisplayName = "New User";
        }

        #endregion
    }
}

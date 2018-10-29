using System;
using System.ComponentModel.DataAnnotations;

namespace SerkoTest.Models
{
    public class Message
    {
        [Required(ErrorMessage = "The text is required")]
        public string Text { get; set; }
    }
}

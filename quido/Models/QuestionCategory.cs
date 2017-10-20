using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace quido.Models
{
    public class QuestionCategory
    {
        private static int counter = 1;

        public QuestionCategory()
        {
            Id = counter;
            counter++;
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
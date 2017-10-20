using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace quido.Models
{
    public class Question
    {
        public Question()
        {
            Id = Guid.NewGuid();
            Answers = new HashSet<Answer>();
            IsDeleted = false;
        }
        
        [Key]
        public Guid Id { get; set; }
        
        public ApplicationUser Author{ get; set; }
        public string QuestionText { get; set; }
        public QuestionCategory QuestionCat { get; set; }
        public string Description { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public DateTime QuestionCreated { get; set; }
        public string QuestionType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace quido.Models
{
    public class Answer
    {
        public Answer()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
        }
        
        public Guid Id { get; set; }
        public Question ConcreteQuestion{ get; set; }
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public bool IsDeleted { get; set; }
    }
}
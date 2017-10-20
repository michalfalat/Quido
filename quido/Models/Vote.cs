using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace quido.Models
{
    public class Vote
    {
        public Vote()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
        }

        public Guid Id { get; set; }
        public Answer votedAnswer { get; set; }
        public string MacAddress { get; set; }
        public string Gender { get; set; }
        public int YearOfBirth { get; set; }
        public string CountryCode { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime VoteCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
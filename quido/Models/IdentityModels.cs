using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace quido.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {       
        public ApplicationUser()
        {
            UserQuestions = new HashSet<Question>();
        }
      
           
        public string Nationality { get; set; }
        public int YearOfBirth { get; set; }
        public string Sex { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Question> UserQuestions { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=quidoDbContext", throwIfV1Schema: false)
        {
           // Database.SetInitializer(new quidoInitializer());
        }


        public DbSet<Question> questions { get; set; }
        public DbSet<QuestionCategory> questionCategories { get; set; }
        public DbSet<Answer> answers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }


    public class quidoInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            IList<QuestionCategory> defaultCategories = new List<QuestionCategory>();

            defaultCategories.Add(new QuestionCategory() { Name = "noCategory" });
            defaultCategories.Add(new QuestionCategory() { Name = "sport" });
            defaultCategories.Add(new QuestionCategory() { Name = "traveling" });
            defaultCategories.Add(new QuestionCategory() { Name = "movies" });
            defaultCategories.Add(new QuestionCategory() { Name = "music" });
            defaultCategories.Add(new QuestionCategory() { Name = "social" });
            defaultCategories.Add(new QuestionCategory() { Name = "literature" });
            defaultCategories.Add(new QuestionCategory() { Name = "computers" });
            defaultCategories.Add(new QuestionCategory() { Name = "games" });
            defaultCategories.Add(new QuestionCategory() { Name = "fasion" });
            defaultCategories.Add(new QuestionCategory() { Name = "health" });
            defaultCategories.Add(new QuestionCategory() { Name = "other" });
            defaultCategories.Add(new QuestionCategory() { Name = "school" });


            foreach (QuestionCategory qc in defaultCategories)
                context.questionCategories.Add(qc);

            base.Seed(context);
        }
    }
}
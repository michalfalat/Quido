using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quido.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserQuestions = new HashSet<Question>();
            isDeleted = false;
            questionsAnswered = 0;
        }


        public string Nationality { get; set; }
        public int YearOfBirth { get; set; }
        public string Sex { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int questionsAnswered { get; set; }

        public ICollection<Question> UserQuestions { get; set; }
        public bool isDeleted { get; set; }

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
            Database.SetInitializer(new quidoInitializer());
        }


        public DbSet<Question> questions { get; set; }
        public DbSet<QuestionCategory> questionCategories { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<Vote> votes { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Report> reports { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }


    public class quidoInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            IList<QuestionCategory> defaultCategories = new List<QuestionCategory>();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string idAdmin = "f030c299-1df4-4cf9-937b-934ea8abe0bc";
            const string email = "info@quido.org";
            const string userName = "admin";
            const string password = "Pa55w0rd!*";
            const string roleName = "AdminRole";


            try
            {
                if (!RoleManager.RoleExists(roleName))
                {
                    var roleresult = RoleManager.Create(new IdentityRole(roleName));
                }

                var user = new ApplicationUser();
                user.UserName = userName;
                user.Email = email;
                user.Id = idAdmin;
                user.Nationality = "CZ";
                user.Sex = "male";
                user.YearOfBirth = 1990;
                user.RegistrationDate = DateTime.Now;
                var adminresult = UserManager.Create(user, password);

                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, roleName);
                }
            }
            catch (Exception ex)
            {
                // Error catched!
            }

            defaultCategories.Add(new QuestionCategory() { Name = "Bez kategorie" });
            defaultCategories.Add(new QuestionCategory() { Name = "Sport" });
            defaultCategories.Add(new QuestionCategory() { Name = "Cestování" });
            defaultCategories.Add(new QuestionCategory() { Name = "Filmy" });
            defaultCategories.Add(new QuestionCategory() { Name = "Hudba" });
            defaultCategories.Add(new QuestionCategory() { Name = "Jídlo" });
            defaultCategories.Add(new QuestionCategory() { Name = "Lidé" });
            defaultCategories.Add(new QuestionCategory() { Name = "Příroda" });
            defaultCategories.Add(new QuestionCategory() { Name = "Politika" });
            defaultCategories.Add(new QuestionCategory() { Name = "PC" });
            defaultCategories.Add(new QuestionCategory() { Name = "Hry" });
            defaultCategories.Add(new QuestionCategory() { Name = "Móda" });
            defaultCategories.Add(new QuestionCategory() { Name = "Zdraví" });            
            defaultCategories.Add(new QuestionCategory() { Name = "Škola" });
            defaultCategories.Add(new QuestionCategory() { Name = "Ostatní" });

            
            context.countries.Add(new Country() { Name = "Česko", ISOCode = "CZ" });            
            context.countries.Add(new Country() { Name = "Slovensko", ISOCode = "SK" });           
            context.countries.Add(new Country() { Name = "Nedefinovano", ISOCode = "undefined" });


            foreach (QuestionCategory qc in defaultCategories)
                context.questionCategories.Add(qc);

            base.Seed(context);
        }
    }
}
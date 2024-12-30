using Microsoft.Extensions.Logging;
using StoreSolution.Core.Enums;
using StoreSolution.Core.Infraestructure.DatabaseSeeder.Interface;
using StoreSolution.Core.Models.Account;
using StoreSolution.Core.Models.Store;
using StoreSolution.Core.Services.Account.Exceptions;
using StoreSolution.Core.Services.Account.Interfaces;
using StoreSolution.Core.Services.Account;
using StoreSolution.Core.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace StoreSolution.Core.Infraestructure.DatabaseSeeder
{
    public class DatabaseSeeder(StoreSolutionDbContext dbContext, ILogger<DatabaseSeeder> logger,
            IUserAccountService userAccountService, IUserRoleService userRoleService) : IDatabaseSeeder
    {
        public async Task SeedAsync()
        {
            await dbContext.Database.MigrateAsync();
            await SeedDefaultUsersAsync();
            await SeedDemoDataAsync();
        }

        private async Task SeedDefaultUsersAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                logger.LogInformation("INICIO - Geração de contas");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator",
                    ApplicationPermissions.GetAllPermissionValues());

                await EnsureRoleAsync(userRoleName, "Default user", []);

                await CreateUserAsync("admin",
                                      "RafaelADM2@25",
                                      "Admin Rafael",
                                      "rafaa.cfc@gmail.com",
                                      "+55 (41) 99992-8000",
                                      [adminRoleName]);

                await CreateUserAsync("user",
                                      "RafaelUSER2@25",
                                      "User Rafael",
                                      "rafaa.cfc1@gmail.com",
                                      "+55 (41) 99992-8001",
                                      [userRoleName]);

                logger.LogInformation("FIM - Geração de contas");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (await userRoleService.GetRoleByNameAsync(roleName) == null)
            {
                logger.LogInformation("Generating default role: {roleName}", roleName);

                var applicationRole = new ApplicationRole(roleName, description);

                var result = await userRoleService.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                {
                    throw new UserRoleException($"Seeding \"{description}\" role failed. Errors: " +
                        $"{string.Join(Environment.NewLine, result.Errors)}");
                }
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(
            string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            logger.LogInformation("Generating default user: {userName}", userName);

            var applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await userAccountService.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
            {
                throw new UserAccountException($"Seeding \"{userName}\" user failed. Errors: " +
                    $"{string.Join(Environment.NewLine, result.Errors)}");
            }

            return applicationUser;
        }

        //DATA INITIAL CREATE
        private async Task SeedDemoDataAsync()
        {
            if (!await dbContext.Customers.AnyAsync() && !await dbContext.Categories.AnyAsync())
            {
                logger.LogInformation("Seeding data");

                var customer_1 = new Customer
                {
                    Name = "Rafael Rodrigues da Silva",
                    Email = "rafael@teste.com",
                    PhoneNumber = "+55 41 99992-8003",
                    Address = "Rua da Glória, 251, ap 607",
                    City = "Curitiba",
                    Gender = Gender.Male
                };

                var customer_2 = new Customer
                {
                    Name = "Carolina Osieki",
                    Email = "carolina@teste.com",
                    PhoneNumber = "+55 41 99992-8004",
                    Address = "Rua da Glória, 607, ap 251",
                    City = "Curitiba",
                    Gender = Gender.Female
                };

                var customer_3 = new Customer
                {
                    Name = "Luciano",
                    Email = "luciano@teste.com",
                    PhoneNumber = "+55 41 99248-8773",
                    Address = "Rua da Abc, 123",
                    City = "Curitiba",
                    Gender = Gender.Male
                };

                var customer_4 = new Customer
                {
                    Name = "Fabio",
                    Email = "fabio@teste.com",
                    PhoneNumber = "+55 11 91914-2930",
                    Address = "Av. Abcd Efgh, 2025",
                    City = "São Paulo",
                    Gender = Gender.Male
                };

                var category_1 = new Category { Name = "Ação" };

                var category_2 = new Category { Name = "Comédia" };

                var category_3 = new Category { Name = "Drama" };

                var category_4 = new Category { Name = "Ficção" };

                var category_5 = new Category { Name = "Terror" };

                var category_6 = new Category { Name = "Romance" };

                var category_7 = new Category { Name = "Documentário" };

                var category_8 = new Category { Name = "Suspense" };

                var category_9 = new Category { Name = "Aventura" };

                var category_10 = new Category { Name = "Fantasia" };

                var category_11 = new Category { Name = "Animação" };

                var movie_1 = new Movie
                {
                    Title = "Interstelar",
                    Description = "Classificação indicativa 10 Anos. Contém violência.",
                    Sinopse = "Dirigido por Christopher Nolan, é um épico filme científico de 2014 que explora as fronteiras do espaço-tempo. Em um futuro distópico, a Terra enfrenta uma crise ambiental e alimentar. Uma equipe de astronautas, liderada pelo comandante Cooper (Matthew McConaughey), é enviada em uma missão desesperada para encontrar um novo lar para a humanidade.",
                    PricePerDay = 10,
                    UnitsInStock = 12,
                    IsActive = true,
                    MovieCategory = category_4
                };

                var movie_2 = new Movie
                {
                    Title = "Kraven",
                    Description = " Classificação indicativa 16 Anos. Contém drogas lícitas, violência extrema.",
                    Sinopse = "Em um futuro distópico, a Terra enfrenta uma crise ambiental e alimentar. Uma equipe de astronautas, liderada pelo comandante Cooper (Matthew McConaughey), é enviada em uma missão desesperada para encontrar um novo lar para a humanidade. Classificação indicativa 10 Anos. Contém violência.",
                    PricePerDay = 11,
                    UnitsInStock = 5,
                    IsActive = true,
                    MovieCategory = category_1
                };

                var movie_3 = new Movie
                {
                    Title = "Sonic",
                    Description = "Classificação indicativa 12 Anos. Contém violência.",
                    Sinopse = "Sonic, Knuckles e Tails se reúnem contra um novo e poderoso adversário, Shadow, um vilão misterioso com poderes diferentes de tudo o que já enfrentaram antes. Com suas habilidades excepcionais, a Equipe Sonic vai buscar uma aliança improvável na esperança de deter Shadow e proteger o planeta.",
                    PricePerDay = 6,
                    UnitsInStock = 2,
                    IsActive = true,
                    MovieCategory = category_1
                };

                var movie_4 = new Movie
                {
                    Title = "Moana 2",
                    Description = "Classificação indicativa Livre. Contém violência.",
                    Sinopse = "Em Moana 2, Moana e Maui se reencontram após três anos para uma nova e incrível jornada com um grupo improvável de marujos. Após receber um chamado de seus ancestrais, Moana parte em uma jornada nos mares distantes da Oceania, desbravando águas perigosas, rumo a uma aventura diferente de todas as que já viveu.",
                    PricePerDay = 6,
                    UnitsInStock = 2,
                    IsActive = true,
                    MovieCategory = category_1
                };

                dbContext.Customers.Add(customer_1);
                dbContext.Customers.Add(customer_2);
                dbContext.Customers.Add(customer_3);
                dbContext.Customers.Add(customer_4);

                dbContext.Movies.Add(movie_1);
                dbContext.Movies.Add(movie_2);
                dbContext.Movies.Add(movie_3);
                dbContext.Movies.Add(movie_4);

                dbContext.Categories.Add(category_1);
                dbContext.Categories.Add(category_2);
                dbContext.Categories.Add(category_3);
                dbContext.Categories.Add(category_4);
                dbContext.Categories.Add(category_5);
                dbContext.Categories.Add(category_6);
                dbContext.Categories.Add(category_7);
                dbContext.Categories.Add(category_8);
                dbContext.Categories.Add(category_9);
                dbContext.Categories.Add(category_10);
                dbContext.Categories.Add(category_11);

                await dbContext.SaveChangesAsync();

                logger.LogInformation("Seeding initial create completed");
            }
        }
    }
}

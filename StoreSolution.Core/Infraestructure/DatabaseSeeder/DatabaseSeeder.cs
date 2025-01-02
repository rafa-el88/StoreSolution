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
            string idGuidUser = await SeedDefaultUsersAsync();
            await SeedDataStoreSolutionAsync(idGuidUser);
        }

        //DATA USERS INITIAL CREATE
        private async Task<string> SeedDefaultUsersAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                logger.LogInformation("START - Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", []);

                await CreateUserAsync("admin", "RafaelADM2@25", "Admin Rafael", "rafaa.cfc@gmail.com", "+55 (41) 99992-8000", [adminRoleName]);

                var user = await CreateUserAsync("user", "RafaelUSER2@25", "User Rafael", "rafaa.cfc1@gmail.com", "+55 (41) 99992-8001", [userRoleName]);

                logger.LogInformation("END - Generating inbuilt accounts");

                if (user != null)
                    return user.Id;

                return "error";
            }
            return "error";
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            logger.LogInformation("START - Generating default user: {userName}", userName);

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
                throw new UserAccountException($"Seeding \"{userName}\" user failed. Errors: " + $"{string.Join(Environment.NewLine, result.Errors)}");

            logger.LogInformation("END - Generating default user: {userName}", userName);

            return applicationUser;
        }

        //DATA ROLE INITIAL CREATE
        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (await userRoleService.GetRoleByNameAsync(roleName) == null)
            {
                logger.LogInformation("START - Generating default role: {roleName}", roleName);

                var applicationRole = new ApplicationRole(roleName, description);

                var result = await userRoleService.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                    throw new UserRoleException($"Seeding \"{description}\" role failed. Errors: " + $"{string.Join(Environment.NewLine, result.Errors)}");

                logger.LogInformation("END - Generating default role: {roleName}", roleName);
            }
        }

        private async Task SeedDataStoreSolutionAsync(string userId)
        {
            if (!await dbContext.Customers.AnyAsync() && !await dbContext.Categories.AnyAsync())
            {
                logger.LogInformation("START - Seeding initial customers");
                await SeedDataCustomerAsync("Rafael Rodrigues da Silva", "rafael@teste.com", "+55 41 99992-8003", "Rua da Glória, 251, ap 607", "Curitiba", Gender.Male, userId);
                await SeedDataCustomerAsync("Carolina Osieki", "carolina@teste.com", "+55 41 99992-8004", "Rua da Glória, 607, ap 251", "Curitiba", Gender.Female, userId);
                await SeedDataCustomerAsync("Luciano", "luciano@teste.com", "+55 41 99248-8773", "Rua da Abc, 123", "Curitiba", Gender.Male, userId);
                await SeedDataCustomerAsync("Fabio", "fabio@teste.com", "+55 11 91914-2930", "Av. Abcd Efgh, 2025", "São Paulo", Gender.Male, userId);
                logger.LogInformation("END - Seeding initial customers");

                logger.LogInformation("START - Seeding initial categories");
                await SeedDataCategoryAsync("Ação", userId);
                await SeedDataCategoryAsync("Comédia", userId);
                await SeedDataCategoryAsync("Drama", userId);
                await SeedDataCategoryAsync("Ficção", userId);
                await SeedDataCategoryAsync("Terror", userId);
                await SeedDataCategoryAsync("Romance", userId);
                await SeedDataCategoryAsync("Documentário", userId);
                await SeedDataCategoryAsync("Suspense", userId);
                await SeedDataCategoryAsync("Aventura", userId);
                await SeedDataCategoryAsync("Fantasia", userId);
                await SeedDataCategoryAsync("Animação", userId);
                logger.LogInformation("END - Seeding initial categories");

                logger.LogInformation("START - Seeding initial movies");
                await SeedDataMovieAsync("Interstelar", "Classificação indicativa 10 Anos. Contém violência.",
                                         "Dirigido por Christopher Nolan, é um épico filme científico de 2014 que explora as fronteiras do espaço-tempo. Em um futuro distópico, a Terra enfrenta uma crise ambiental e alimentar. Uma equipe de astronautas, liderada pelo comandante Cooper (Matthew McConaughey), é enviada em uma missão desesperada para encontrar um novo lar para a humanidade.",
                                         10, 12, 12, 4, userId);
                await SeedDataMovieAsync("Kraven", "Classificação indicativa 16 Anos. Contém drogas lícitas, violência extrema.",
                                         "Em um futuro distópico, a Terra enfrenta uma crise ambiental e alimentar. Uma equipe de astronautas, liderada pelo comandante Cooper (Matthew McConaughey), é enviada em uma missão desesperada para encontrar um novo lar para a humanidade. Classificação indicativa 10 Anos. Contém violência.",
                                         11, 5, 5, 1, userId);
                await SeedDataMovieAsync("Sonic", "Classificação indicativa 12 Anos. Contém violência.",
                                         "Sonic, Knuckles e Tails se reúnem contra um novo e poderoso adversário, Shadow, um vilão misterioso com poderes diferentes de tudo o que já enfrentaram antes. Com suas habilidades excepcionais, a Equipe Sonic vai buscar uma aliança improvável na esperança de deter Shadow e proteger o planeta.",
                                         6, 2, 2, 1, userId);
                await SeedDataMovieAsync("Moana 2", "Classificação indicativa Livre. Contém violência.",
                                         "Em Moana 2, Moana e Maui se reencontram após três anos para uma nova e incrível jornada com um grupo improvável de marujos. Após receber um chamado de seus ancestrais, Moana parte em uma jornada nos mares distantes da Oceania, desbravando águas perigosas, rumo a uma aventura diferente de todas as que já viveu.",
                                         6, 3, 3, 11, userId);
                logger.LogInformation("END - Seeding initial movies");

                logger.LogInformation("START - Seeding initial orders");
                await SeedDataOrderAsync(2, 3, userId);
                await SeedDataOrderAsync(3, 8, userId);
                await SeedDataOrderAsync(2, 5, userId);


                await SeedDataOrderDetailAsync(1, 3, userId);
                await SeedDataOrderDetailAsync(1, 1, userId);
                await SeedDataOrderDetailAsync(2, 3, userId);
                await SeedDataOrderDetailAsync(3, 1, userId);
                await SeedDataOrderDetailAsync(3, 2, userId);
                logger.LogInformation("END - Seeding initial orders");

                logger.LogInformation("END - Seeding intial data StoreSolution");
            }
        }

        private async Task SeedDataOrderAsync(int customerId, int days, string useId)
        {
            logger.LogInformation("START - Generating default order to customerId: {customerId}", useId);

            var order = new Order
            {
                DateStartRental = DateTime.Now,
                DateEndRental = DateTime.Now.AddDays(days),
                ReturnedMovie = false,
                CashierId = useId,
                CustomerId = customerId,
                CreatedBy = useId,
                UpdatedBy = useId,
            };

            dbContext.Orders.Add(order);

            await dbContext.SaveChangesAsync();

            logger.LogInformation("END - Generating default order to customerId: {customerId}", useId);
        }

        //DATA INITIAL CREATE ORDER DETAIL DETAIL
        private async Task SeedDataOrderDetailAsync(int orderId, int movieId, string useId)
        {
            logger.LogInformation("START - Generating default order detail for orderId: {orderId}.", orderId);

            var orderDetail = new OrderDetail
            {
                MovieId = movieId,
                OrderId = orderId,
                CreatedBy = useId,
                UpdatedBy = useId
            };

            dbContext.OrderDetails.Add(orderDetail);
            var orderDetailId = await dbContext.SaveChangesAsync();

            //Update movie stock
            var movie = await dbContext.Movies.FindAsync(movieId);
            if (movie != null)
            {
                movie.UnitsInStock -= 1;
                dbContext.Movies.Update(movie);
                dbContext.SaveChanges();
            }

            logger.LogInformation("END - Generating default order detail: {idOrderDetail},  for orderId: {orderId}.", orderDetailId, orderId);
        }

        //DATA INITIAL CREATE CUSTOMER
        private async Task SeedDataCustomerAsync(string name, string email, string phoneNumber, string adress, string city, Gender gender, string userId)
        {
            logger.LogInformation("START - Generating default customer: {name}: ", name);

            var customer = new Customer
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = adress,
                City = city,
                Gender = gender,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("END - Generating default customer: {name}: ", name);
        }

        //DATA INITIAL CREATE CATEGORY
        private async Task SeedDataCategoryAsync(string name, string userId)
        {
            logger.LogInformation("START - Generating default category: {name}: ", name);

            var category = new Category { Name = name, IsActive = true, CreatedBy = userId, UpdatedBy = userId };

            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("END - Generating default customer: {name}: ", name);
        }

        //DATA INITIAL CREATE Movie
        private async Task SeedDataMovieAsync(string title, string description, string sinopse, decimal pricePerDay, int quantityCopies, int unitsInStock, int categoryId, string userId)
        {
            logger.LogInformation("START - Generating default movie: {title}: ", title);

            var movie = new Movie
            {
                Title = title,
                Description = description,
                Sinopse = sinopse,
                PricePerDay = pricePerDay,
                QuantityCopies = quantityCopies,
                UnitsInStock = 12,
                IsActive = true,
                CategoryId = categoryId,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("END - Generating default  movie: {title}: ", title);
        }
    }
}

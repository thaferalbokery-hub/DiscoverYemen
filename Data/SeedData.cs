using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Models;

namespace DiscoverYemen.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed Admin User
            if (await userManager.FindByEmailAsync("admin@discoveryemen.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@discoveryemen.com",
                    Email = "admin@discoveryemen.com",
                    FullName = "مدير النظام",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Seed Default User
            if (await userManager.FindByEmailAsync("user@discoveryemen.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@discoveryemen.com",
                    Email = "user@discoveryemen.com",
                    FullName = "مستخدم عادي",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }

            // Seed Governorates
            if (!context.Governorates.Any())
            {
                var governorates = new List<Governorate>
                {
                    new Governorate { Name = "صنعاء", Description = "العاصمة اليمنية، مدينة تاريخية عريقة تضم العديد من المعالم الأثرية" },
                    new Governorate { Name = "عدن", Description = "مدينة ساحلية جميلة تقع على خليج عدن" },
                    new Governorate { Name = "تعز", Description = "مدينة الثقافة والعلم، تقع على سفح جبل صبر" },
                    new Governorate { Name = "حضرموت", Description = "أكبر محافظات اليمن مساحة، تشتهر بوادي حضرموت" },
                    new Governorate { Name = "إب", Description = "اللواء الأخضر، تتميز بطبيعتها الخلابة" },
                    new Governorate { Name = "المهرة", Description = "محافظة ساحلية تقع في أقصى شرق اليمن" }
                };
                context.Governorates.AddRange(governorates);
                await context.SaveChangesAsync();
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "تاريخي", Description = "معالم ومواقع تاريخية" },
                    new Category { Name = "طبيعي", Description = "مناظر طبيعية وحدائق" },
                    new Category { Name = "ديني", Description = "مساجد ومعالم دينية" },
                    new Category { Name = "ثقافي", Description = "متاحف ومراكز ثقافية" },
                    new Category { Name = "ترفيهي", Description = "أماكن ترفيهية وسياحية" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Attractions
            if (!context.Attractions.Any())
            {
                var sanaa = context.Governorates.First(g => g.Name == "صنعاء");
                var aden = context.Governorates.First(g => g.Name == "عدن");
                var taiz = context.Governorates.First(g => g.Name == "تعز");
                var hadramout = context.Governorates.First(g => g.Name == "حضرموت");

                var attractions = new List<Attraction>
                {
                    new Attraction { Name = "باب اليمن", Description = "البوابة الرئيسية لمدينة صنعاء القديمة، رمز تاريخي للمدينة", Location = "صنعاء القديمة", GovernorateId = sanaa.Id },
                    new Attraction { Name = "دار الحجر", Description = "قصر صخري تاريخي يقع في وادي ظهر بالقرب من صنعاء", Location = "وادي ظهر", GovernorateId = sanaa.Id },
                    new Attraction { Name = "صهاريج عدن", Description = "خزانات مياه تاريخية تعود للعصور القديمة", Location = "كريتر، عدن", GovernorateId = aden.Id },
                    new Attraction { Name = "قلعة صيرة", Description = "قلعة تاريخية تطل على ميناء عدن", Location = "صيرة، عدن", GovernorateId = aden.Id },
                    new Attraction { Name = "قلعة القاهرة", Description = "قلعة تاريخية تطل على مدينة تعز", Location = "تعز", GovernorateId = taiz.Id },
                    new Attraction { Name = "مدينة شبام", Description = "مدينة ناطحات السحاب الطينية، موقع تراث عالمي", Location = "وادي حضرموت", GovernorateId = hadramout.Id }
                };
                context.Attractions.AddRange(attractions);
                await context.SaveChangesAsync();

                // Seed AttractionCategories
                var historicalCat = context.Categories.First(c => c.Name == "تاريخي");
                var naturalCat = context.Categories.First(c => c.Name == "طبيعي");
                var culturalCat = context.Categories.First(c => c.Name == "ثقافي");

                var attractionCategories = new List<AttractionCategory>
                {
                    new AttractionCategory { AttractionId = attractions[0].Id, CategoryId = historicalCat.Id },
                    new AttractionCategory { AttractionId = attractions[0].Id, CategoryId = culturalCat.Id },
                    new AttractionCategory { AttractionId = attractions[1].Id, CategoryId = historicalCat.Id },
                    new AttractionCategory { AttractionId = attractions[1].Id, CategoryId = naturalCat.Id },
                    new AttractionCategory { AttractionId = attractions[2].Id, CategoryId = historicalCat.Id },
                    new AttractionCategory { AttractionId = attractions[3].Id, CategoryId = historicalCat.Id },
                    new AttractionCategory { AttractionId = attractions[4].Id, CategoryId = historicalCat.Id },
                    new AttractionCategory { AttractionId = attractions[5].Id, CategoryId = historicalCat.Id },
                    new AttractionCategory { AttractionId = attractions[5].Id, CategoryId = culturalCat.Id }
                };
                context.AttractionCategories.AddRange(attractionCategories);
                await context.SaveChangesAsync();
            }

            // Seed Hotels
            if (!context.Hotels.Any())
            {
                var sanaa = context.Governorates.First(g => g.Name == "صنعاء");
                var aden = context.Governorates.First(g => g.Name == "عدن");
                var taiz = context.Governorates.First(g => g.Name == "تعز");

                var hotels = new List<Hotel>
                {
                    new Hotel { Name = "فندق موفنبيك صنعاء", Description = "فندق فاخر في قلب العاصمة", Address = "شارع الزبيري، صنعاء", Phone = "01-200100", Stars = 5, GovernorateId = sanaa.Id },
                    new Hotel { Name = "فندق شيراتون عدن", Description = "فندق على شاطئ البحر", Address = "جولد مور، عدن", Phone = "02-300200", Stars = 4, GovernorateId = aden.Id },
                    new Hotel { Name = "فندق تعز بلازا", Description = "فندق في وسط المدينة", Address = "شارع جمال، تعز", Phone = "04-100300", Stars = 3, GovernorateId = taiz.Id }
                };
                context.Hotels.AddRange(hotels);
                await context.SaveChangesAsync();
            }

            // Seed Restaurants
            if (!context.Restaurants.Any())
            {
                var sanaa = context.Governorates.First(g => g.Name == "صنعاء");
                var aden = context.Governorates.First(g => g.Name == "عدن");

                var restaurants = new List<Restaurant>
                {
                    new Restaurant { Name = "مطعم السلطان", Description = "مطعم يقدم أشهى المأكولات اليمنية التقليدية", Address = "شارع حدة، صنعاء", Phone = "01-400100", CuisineType = "يمني تقليدي", GovernorateId = sanaa.Id },
                    new Restaurant { Name = "مطعم بحر عدن", Description = "مطعم متخصص في المأكولات البحرية", Address = "التواهي، عدن", Phone = "02-500200", CuisineType = "مأكولات بحرية", GovernorateId = aden.Id },
                    new Restaurant { Name = "مطعم الشيباني", Description = "مطعم شعبي يقدم المندي والمظبي", Address = "شارع الستين، صنعاء", Phone = "01-600300", CuisineType = "يمني شعبي", GovernorateId = sanaa.Id }
                };
                context.Restaurants.AddRange(restaurants);
                await context.SaveChangesAsync();
            }

            // Seed Events
            if (!context.Events.Any())
            {
                var sanaa = context.Governorates.First(g => g.Name == "صنعاء");
                var aden = context.Governorates.First(g => g.Name == "عدن");

                var events = new List<Event>
                {
                    new Event { Name = "مهرجان صنعاء للثقافة", Description = "مهرجان سنوي يحتفي بالتراث اليمني", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 3, 7), Location = "صنعاء القديمة", Status = "قادم", GovernorateId = sanaa.Id },
                    new Event { Name = "مهرجان عدن السياحي", Description = "فعالية سياحية على شواطئ عدن", StartDate = new DateTime(2024, 6, 15), EndDate = new DateTime(2024, 6, 20), Location = "شاطئ جولد مور", Status = "قادم", GovernorateId = aden.Id },
                    new Event { Name = "معرض الحرف اليدوية", Description = "معرض للحرف والصناعات التقليدية اليمنية", StartDate = new DateTime(2024, 1, 10), EndDate = new DateTime(2024, 1, 15), Location = "المتحف الوطني، صنعاء", Status = "منتهي", GovernorateId = sanaa.Id }
                };
                context.Events.AddRange(events);
                await context.SaveChangesAsync();
            }
        }
    }
}
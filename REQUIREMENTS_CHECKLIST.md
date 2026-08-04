# قائمة متطلبات المشروع - اكتشف اليمن

| # | المتطلب | الحالة | الملف/الدليل |
|---|---------|--------|--------------|
| 1 | ASP.NET Core MVC (.NET 9) | PASS | DiscoverYemen.csproj (net9.0), Program.cs |
| 2 | 10+ نماذج قاعدة بيانات | PASS | 12 نموذج: ApplicationUser, UserProfile, Governorate, Attraction, Category, AttractionCategory, Hotel, Restaurant, Event, Favorite, Review, Booking, BookingItem |
| 3a | علاقة 1:1 | PASS | ApplicationUser → UserProfile (Data/ApplicationDbContext.cs) |
| 3b | علاقات 1:N | PASS | Governorate→Attractions/Hotels/Restaurants/Events, User→Reviews/Bookings/Favorites, Booking→BookingItems |
| 3c | علاقة M:N | PASS | Attraction ↔ Category via AttractionCategory (composite key) |
| 4 | بذر البيانات (Seed Data) | PASS | Data/SeedData.cs - Roles, Users, Governorates, Categories, Attractions, Hotels, Restaurants, Events |
| 5 | المصادقة (Identity) | PASS | Controllers/AccountController.cs - Register, Login, Logout |
| 6 | التفويض (Authorization) | PASS | [Authorize(Roles="Admin")] على Areas/Admin/Controllers/ |
| 7 | CRUD كامل | PASS | Areas/Admin/Controllers/AttractionsController.cs (Create, Edit, Delete) + Index/Details عام |
| 8 | رفع الصور وإدارتها | PASS | Services/AttractionService.cs - SaveImageAsync, DeleteImage, تحديث عند Edit |
| 9 | Data Annotations | PASS | Models/ و ViewModels/ - [Required], [StringLength], [Range], [EmailAddress], [Display], [DataType] |
| 10 | Server-side validation | PASS | ModelState.IsValid في جميع POST actions |
| 11 | Client-side validation | PASS | _ValidationScriptsPartial.cshtml + asp-validation-for في Login, Register, Admin Create/Edit |
| 12 | Tag Helpers | PASS | asp-for, asp-action, asp-controller, asp-route-id, asp-validation-for, asp-validation-summary, asp-items |
| 13 | Where() - Row filtering | PASS | Services/AttractionService.cs, Controllers/HotelsController.cs, EventsController.cs, GovernoratesController.cs |
| 14 | Select() - Column projection | PASS | Services/AttractionService.cs → AttractionListViewModel, Services/ReportService.cs |
| 15 | Include() / ThenInclude() | PASS | Services/AttractionService.cs, Controllers/GovernoratesController.cs, FavoritesController.cs |
| 16 | ViewBag / ViewData | PASS | HomeController (إحصائيات), AttractionsController (فلاتر), DashboardController |
| 17 | Partial Views | PASS | _SearchForm, _SummaryCard, _ValidationScriptsPartial, _AdminLayout |
| 18 | التقارير من قاعدة البيانات | PASS | Services/ReportService.cs - CountAsync, Select, Where |
| 19 | واجهة مستخدم متجاوبة | PASS | Bootstrap 5 RTL, responsive grid في جميع الصفحات |
| 20 | منطقة المسؤول محمية | PASS | Areas/Admin/ مع [Authorize(Roles="Admin")] |
| 21 | التوثيق | PASS | README.md - نظرة عامة، مخطط فئات، دليل مستخدم |
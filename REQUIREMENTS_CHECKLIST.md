# قائمة متطلبات المشروع - اكتشف اليمن

| # | المتطلب | الحالة | الملف/الدليل |
|---|---------|--------|--------------|
| 1 | ASP.NET Core MVC (.NET 9) | PASS | DiscoverYemen.csproj, Program.cs |
| 2 | 10+ نماذج قاعدة بيانات | PASS | Models/ — 13 نموذج (ApplicationUser, UserProfile, Governorate, Attraction, Category, AttractionCategory, Hotel, Restaurant, Event, Favorite, Review, Booking, BookingItem) |
| 3a | علاقة 1:1 | PASS | ApplicationUser → UserProfile — Data/ApplicationDbContext.cs |
| 3b | علاقات 1:N | PASS | Governorate→Attractions/Hotels/Restaurants/Events, User→Reviews/Bookings/Favorites, Booking→BookingItems |
| 3c | علاقة M:N | PASS | Attraction ↔ Category via AttractionCategory (composite key) |
| 4 | بذر البيانات (Seed Data) | PASS | Data/SeedData.cs — Roles, Users, Governorates, Categories, Attractions, Hotels, Restaurants, Events |
| 5 | المصادقة (Identity) | PASS | Controllers/AccountController.cs — Register, Login, Logout |
| 6 | التفويض (Authorization) | PASS | [Authorize(Roles="Admin")] على Areas/Admin/Controllers/ |
| 7 | CRUD كامل | PASS | Areas/Admin/Controllers/AttractionsController.cs — Create, Edit, Delete + Index/Details عام |
| 8 | رفع الصور مع التحقق | PASS | Services/AttractionService.cs — ValidateImageFile (extension, MIME, size, empty, safe name, path traversal), SaveImageAsync, DeleteImage |
| 9 | تحديث الصور | PASS | Services/AttractionService.cs — UpdateAttractionAsync: يتحقق من الصورة الجديدة أولاً، يحفظها، ثم يحذف القديمة |
| 10 | حذف الصور | PASS | Services/AttractionService.cs — DeleteAttractionAsync: يحذف الصورة من الخادم بأمان (لا يرمي استثناء إذا الملف مفقود) |
| 11 | Data Annotations | PASS | Models/ و ViewModels/ — [Required], [StringLength], [Range], [EmailAddress], [Display], [DataType] |
| 12 | Server-side validation | PASS | ModelState.IsValid في جميع POST actions |
| 13 | Client-side validation | PASS | _ValidationScriptsPartial.cshtml + asp-validation-for في Login, Register, Admin Create/Edit |
| 14 | Tag Helpers | PASS | asp-for, asp-action, asp-controller, asp-route-id, asp-validation-for, asp-validation-summary, asp-items |
| 15 | Where() — تصفية الصفوف | PASS | Services/AttractionService.cs, Controllers/HotelsController.cs, EventsController.cs |
| 16 | Select() — إسقاط الأعمدة | PASS | Services/AttractionService.cs → AttractionListViewModel, Services/ReportService.cs |
| 17 | Include() / ThenInclude() | PASS | Services/AttractionService.cs, Controllers/GovernoratesController.cs, FavoritesController.cs |
| 18 | ViewBag / ViewData | PASS | HomeController (إحصائيات), AttractionsController (فلاتر), DashboardController |
| 19 | Partial Views | PASS | _ValidationScriptsPartial, _AdminLayout |
| 20 | التقارير من قاعدة البيانات | PASS | Services/ReportService.cs — CountAsync, Select, Where |
| 21 | واجهة مستخدم متجاوبة | PASS | Bootstrap 5 RTL, responsive grid |
| 22 | منطقة المسؤول محمية | PASS | Areas/Admin/ مع [Authorize(Roles="Admin")] |
| 23 | التوثيق | PASS | README.md — نظرة عامة، بنية، دليل مستخدم |
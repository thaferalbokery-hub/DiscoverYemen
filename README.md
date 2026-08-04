# اكتشف اليمن - Discover Yemen

## نظرة عامة

مشروع "اكتشف اليمن" هو نظام إدارة سياحية مبني باستخدام ASP.NET Core MVC.
يهدف المشروع إلى تقديم منصة شاملة لاكتشاف المعالم السياحية والفنادق والمطاعم والفعاليات في اليمن.

## الأهداف

- تطوير منصة سياحية شاملة لليمن
- تنفيذ نظام مصادقة وتفويض متكامل
- تطبيق عمليات CRUD كاملة مع رفع الصور
- استخدام Entity Framework Core مع العلاقات المختلفة

## مجال المشكلة

اليمن بلد غني بالتراث والمعالم السياحية لكن يفتقر لمنصات رقمية شاملة تعرض هذه المعالم.
هذا النظام يوفر حلاً لعرض وإدارة المعلومات السياحية بشكل منظم.

## التقنيات المستخدمة

- .NET 9 / ASP.NET Core MVC
- Entity Framework Core 9
- ASP.NET Core Identity
- SQLite
- Bootstrap 5 RTL
- Font Awesome
- jQuery Validation

## البنية المعمارية

```
DiscoverYemen/
├── Areas/Admin/         → لوحة تحكم المسؤول (Controllers, Views)
├── Controllers/         → وحدات التحكم العامة
├── Data/                → DbContext وبذر البيانات
├── Migrations/          → ترحيلات EF Core
├── Models/              → نماذج البيانات (12 نموذج)
├── Services/            → طبقة الخدمات (AttractionService, BookingService, ReportService)
├── ViewModels/          → نماذج العرض
├── Views/               → واجهات Razor
├── wwwroot/             → ملفات ثابتة (CSS, JS, مكتبات)
├── uploads/             → صور المعالم المرفوعة
├── Program.cs           → نقطة الدخول
└── appsettings.json     → إعدادات التطبيق
```

## قاعدة البيانات

- **12 نموذج**: ApplicationUser, UserProfile, Governorate, Attraction, Category, AttractionCategory, Hotel, Restaurant, Event, Favorite, Review, Booking, BookingItem
- **العلاقات**:
  - 1:1 — ApplicationUser → UserProfile
  - 1:N — Governorate → Attractions, Hotels, Restaurants, Events
  - 1:N — User → Reviews, Bookings, Favorites
  - 1:N — Booking → BookingItems
  - M:N — Attraction ↔ Category (via AttractionCategory)

## المصادقة والتفويض

- ASP.NET Core Identity مع أدوار (Admin, User)
- تسجيل حساب جديد، تسجيل دخول، تسجيل خروج
- منطقة المسؤول محمية بـ `[Authorize(Roles = "Admin")]`
- صفحة رفض الوصول للمستخدمين غير المصرح لهم

## عمليات CRUD

- إضافة، عرض، تعديل، حذف للمعالم والفنادق والمطاعم والفعاليات والمحافظات والتصنيفات
- إدارة الحجوزات والتقييمات والمفضلة

## إدارة الصور

- رفع صور المعالم مع التحقق من:
  - امتداد الملف (jpg, jpeg, png, webp فقط)
  - نوع المحتوى (MIME type)
  - حجم الملف (5 ميجابايت كحد أقصى)
  - الملفات الفارغة
  - أسماء الملفات الآمنة (Guid)
  - حماية من Path Traversal
- تحديث الصورة: التحقق من الصورة الجديدة أولاً، ثم حفظها، ثم حذف القديمة
- حذف الصورة عند حذف المعلم بأمان

## الاستعلامات

- `Where()` — تصفية المعالم حسب المحافظة والتصنيف والبحث النصي
- `Select()` — إسقاط البيانات إلى ViewModels
- `Include()` / `ThenInclude()` — تحميل العلاقات

## التحقق من البيانات

- Data Annotations على Models و ViewModels
- Server-side: `ModelState.IsValid` في جميع POST actions
- Client-side: jQuery unobtrusive validation

## التقارير

- إحصائيات شاملة من قاعدة البيانات (عدد المستخدمين، المعالم، الفنادق، المطاعم، الفعاليات، التقييمات، الحجوزات)
- تقرير المعالم حسب المحافظة

## كيفية التشغيل

```bash
cd DiscoverYemen
dotnet restore
dotnet build
dotnet run
```

## دليل المستخدم

### بيانات الدخول الافتراضية

| الدور | البريد الإلكتروني | كلمة المرور |
|-------|-------------------|-------------|
| مسؤول | admin@discoveryemen.com | Admin@123 |
| مستخدم | user@discoveryemen.com | User@123 |

### الاستخدام الأساسي

1. **التصفح**: عرض المعالم والفنادق والمطاعم والفعاليات والمحافظات
2. **البحث**: استخدام فلاتر المحافظة والتصنيف والبحث النصي
3. **التسجيل**: إنشاء حساب جديد من القائمة
4. **المفضلة**: إضافة/إزالة المعالم من المفضلة (يتطلب تسجيل دخول)
5. **التقييم**: تقييم المعالم وكتابة تعليقات (يتطلب تسجيل دخول)
6. **الحجز**: حجز المعالم والفنادق والفعاليات (يتطلب تسجيل دخول)

### لوحة تحكم المسؤول

- تسجيل الدخول كمسؤول → لوحة التحكم
- إدارة المعالم مع رفع/تحديث/حذف الصور
- إدارة المحافظات والتصنيفات والفنادق والمطاعم والفعاليات
- عرض التقارير والإحصائيات
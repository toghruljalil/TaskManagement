# Task Management REST API

Bu layihə istifadəçilərin qeydiyyatdan keçərək layihələr yarada bildiyi, həmin layihələrə tasklar əlavə edib idarə etdiyi tam işlək REST API sistemidir. Sistem peşəkar sənaye standartlarına uyğun olaraq DTO arxitekturası və mürəkkəb verilənlər bazası əlaqələri üzərində qurulmuşdur.

## Texnologiyalar və Alətlər
- **Platforma:** .NET 9 / C#
- **Verilənlər Bazası:** PostgreSQL
- **ORM:** Entity Framework Core
- **Mapping:** AutoMapper
- **Təhlükəsizlik:** BCrypt

## Arxitektura və Təhlükəsizlik
- **Data Transfer Object (DTO):** Verilənlər bazası cədvəlləri kənardan gələn HTTP sorğularına birbaşa açılmır. Məlumat axını yalnız təyin olunmuş DTO-lar vasitəsilə süzgəcdən keçərək təhlükəsiz şəkildə təmin edilir.
- **Şifrələmə:** İstifadəçi şifrələri açıq mətndə saxlanılmır, `BCrypt` alqoritmi vasitəsilə hashlənərək verilənlər bazasına yazılır.
- **Performans və Optimallaşdırma:** `Tasks` və `Projects` siyahıları çəkilərkən məlumatlar yaddaşa doldurulmur. `IQueryable` istifadə edilərək axtarış, sıralama və səhifələmə birbaşa SQL səviyyəsində icra edilir.

## Əsas Funksionallıqlar
- **Autentifikasiya:** Register və Login sistemi.
- **Proyektlərin İdarəedilməsi:** Layihə yaratmaq, oxumaq, yeniləmək və silmək.
- **Taskların İdarəedilməsi:** Xüsusi layihələrə və istifadəçilərə təhkim olunmuş tapşırıqların idarə edilməsi.
- **Qabaqcıl Sorğular:** Həm `Projects`, həm də `Tasks` siyahılarında statusa/ada görə axtarış, dinamik sıralama və səhifələmə.

## API Endpoints

**Auth**
- `POST /api/auth/register` - Yeni istifadəçi qeydiyyatı
- `POST /api/auth/login` - Sistemə giriş

**Projects**
- `GET /api/projects` - Layihələri gətirir
- `GET /api/projects/{id}` - ID-yə əsasən tək layihəni gətirir
- `POST /api/projects` - Yeni layihə yaradır
- `PUT /api/projects/{id}` - Mövcud layihəni yeniləyir
- `DELETE /api/projects/{id}` - Layihəni silir

**Tasks**
- `GET /api/tasks` - Tapşırıqları gətirir
- `GET /api/tasks/{id}` - ID-yə əsasən tək tapşırığı gətirir
- `POST /api/tasks` - Yeni tapşırıq yaradır
- `PUT /api/tasks/{id}` - Mövcud tapşırığı yeniləyir
- `DELETE /api/tasks/{id}` - Tapşırığı silir
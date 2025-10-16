MyBlog, Clean Architecture prensipleriyle geliştirilmiş bir ASP.NET Core blog platformudur.
Uygulama, katmanlı ve sürdürülebilir bir mimari yapıya sahip olup; Admin ve Kullanıcı için ayrı paneller içerir.
Kullanıcılar kendi gönderilerini yönetebilirken, yöneticiler tüm sistemi merkezi olarak kontrol edebilir.
🚀 Özellikler
👤 Kullanıcı Paneli

Yeni blog gönderisi oluşturma ✍️

Mevcut gönderileri düzenleme ve silme

Kendi gönderilerini listeleme ve filtreleme

Kişisel profil bilgilerini görüntüleme

🔧 Admin Paneli

Tüm kullanıcıları listeleme 👥

Kullanıcı ekleme, düzenleme, silme işlemleri

Tüm blog gönderilerini görüntüleme

Gerektiğinde kullanıcı gönderilerine müdahale etme

Yönetici yetkilendirmesi ve kullanıcı rolleri kontrolü

🏗️ Teknik Mimari (Clean Architecture)

Presentation Layer (UI): ASP.NET Core MVC ile oluşturulmuş iki farklı arayüz (Admin ve Kullanıcı panelleri)

Application Layer: İş kuralları, DTO’lar ve servisler

Domain Layer: Entity modelleri ve çekirdek kurallar

Infrastructure Layer: EF Core üzerinden veri erişimi ve repository yapısı


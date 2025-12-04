# 🚀 HƯỚNG DẪN DEPLOYMENT - NH12 PC SHOP

## 📦 ĐÃ BUILD XONG!

Thư mục **`publish`** chứa tất cả files cần thiết để deploy:
- ✅ Application files (.dll, .exe)
- ✅ wwwroot (css, js, images, videos ~50MB)
- ✅ Database backup: `pc_shop2_backup.sql`
- ✅ Configuration files
- ✅ web.config

---

## 🎯 CHỌN PHƯƠNG THỨC DEPLOY

### 🥇 KHUYẾN NGHỊ: Render.com (MIỄN PHÍ)

**Tại sao chọn Render?**
- ✅ Hỗ trợ .NET Core 10.0
- ✅ SSL miễn phí (HTTPS)
- ✅ Database PostgreSQL miễn phí (hoặc MySQL external)
- ✅ Deploy tự động từ GitHub
- ✅ Miễn phí 100%
- ⚠️ Cold start 30s (sau 15 phút không hoạt động)
- ⚠️ PostgreSQL thay vì MySQL (cần chuyển đổi hoặc dùng external MySQL)

**📖 Xem hướng dẫn**: [`DEPLOY-RENDER.md`](DEPLOY-RENDER.md)

---

### 🥈 THAY THẾ: Azure App Service

**Ưu điểm**:
- ✅ Hỗ trợ .NET Core tốt nhất
- ✅ Performance cao
- ✅ Miễn phí 12 tháng (cần credit card)
- ✅ Tích hợp CI/CD

**Nhược điểm**:
- ⚠️ Cần credit card
- ⚠️ Sau 12 tháng: $13/month

**📖 Xem hướng dẫn**: [`DEPLOY-AZURE.md`](DEPLOY-AZURE.md)

---

### 🥉 InfinityFree (KHÔNG KHUYẾN NGHỊ)

**⚠️ LƯU Ý**: InfinityFree KHÔNG hỗ trợ ASP.NET Core!

Website này được build với .NET Core và **KHÔNG THỂ** chạy trên InfinityFree.

**📖 Chi tiết**: [`HUONG-DAN-DEPLOY.md`](HUONG-DAN-DEPLOY.md)

---

## 🚀 BƯỚC TIẾP THEO

### Bước 1: Chọn nền tảng

Khuyến nghị: **Render.com**

### Bước 2: Làm theo hướng dẫn

Mở file tương ứng:
- **Render**: `DEPLOY-RENDER.md` ⭐
- **Azure**: `DEPLOY-AZURE.md`

### Bước 3: Upload Database

File backup: `pc_shop2_backup.sql` (trong thư mục `publish`)

### Bước 4: Cấu hình

Cập nhật connection string trong `appsettings.Production.json`

### Bước 5: Deploy

Upload/Push code lên hosting

---

## 📊 SO SÁNH HOSTING

| Feature | Render (Free) | Azure | InfinityFree |
|---------|---------------|-------|--------------|
| **Hỗ trợ .NET Core** | ✅ | ✅ | ❌ |
| **SSL (HTTPS)** | ✅ Free | ✅ Free | ❌ |
| **Database** | ✅ MySQL Free | ✅ Paid | ✅ MySQL Free |
| **Bandwidth** | Unlimited | Limited | Unlimited |
| **Uptime** | 99%+ | 99.9%+ | 99%+ |
| **Cold Start** | ~30s | 0s | N/A |
| **Chi phí** | **FREE** | $13/month | FREE |
| **Khuyến nghị** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐ |

---

## 📂 CẤU TRÚC THỨ MỤC PUBLISH

```
publish/
├── wwwroot/                    # Static files
│   ├── css/                   # Stylesheets
│   ├── js/                    # JavaScript
│   ├── images/                # Hình ảnh sản phẩm
│   ├── videos/                # Video background (~50MB)
│   └── lib/                   # Libraries (Bootstrap, jQuery, etc.)
├── runtimes/                  # Runtime dependencies
├── ShopWeb.dll               # Main application
├── ShopWeb.exe               # Windows executable
├── ShopWeb.deps.json         # Dependencies
├── ShopWeb.runtimeconfig.json
├── web.config                # IIS configuration
├── appsettings.json          # Base settings
├── appsettings.Production.json # Production settings (CẬP NHẬT NÀY!)
├── pc_shop2_backup.sql       # Database backup
├── DEPLOY-RENDER.md          # Hướng dẫn deploy Render ⭐
├── DEPLOY-AZURE.md           # Hướng dẫn deploy Azure
└── HUONG-DAN-DEPLOY.md       # Thông tin chung
```

---

## ⚙️ CẤU HÌNH DATABASE

### Connection String mẫu:

```
Server=YOUR_SERVER;Database=pc_shop2;User=YOUR_USER;Password=YOUR_PASSWORD;Port=3306
```

### Thông tin Admin mặc định:

```
Email: admin@shopweb.com
Password: Admin@123
```

⚠️ **Đổi password ngay sau khi deploy!**

---

## 🔧 YÊU CẦU HỆ THỐNG

### Server Requirements:
- ✅ .NET Core Runtime 10.0 (hoặc SDK)
- ✅ MySQL 8.0+ (hoặc MariaDB 10.5+)
- ✅ 1GB RAM minimum
- ✅ 500MB disk space

### Database:
- Tables: 8 (Users, Products, Categories, Orders, Reviews, etc.)
- Size: ~10-50MB (tùy dữ liệu)

---

## 📋 CHECKLIST SAU KHI DEPLOY

- [ ] Website accessible (HTTP/HTTPS)
- [ ] Trang chủ hiển thị
- [ ] Video background chạy
- [ ] Danh sách sản phẩm
- [ ] Chi tiết sản phẩm
- [ ] Tìm kiếm hoạt động
- [ ] Giỏ hàng (thêm/xóa)
- [ ] Đăng ký tài khoản
- [ ] Đăng nhập
- [ ] Checkout
- [ ] Admin Panel: `/Admin`
- [ ] Quản lý sản phẩm (CRUD)
- [ ] Quản lý đơn hàng
- [ ] Đã đổi password admin

---

## 🆘 HỖ TRỢ

### Gặp vấn đề?

1. **Kiểm tra logs** trên hosting platform
2. **Test database connection**:
   ```powershell
   mysql -h SERVER -u USER -p DATABASE
   ```
3. **Xem lại connection string** trong `appsettings.Production.json`
4. **Liên hệ**: 0946703205

### Resources:

- **Render Docs**: https://render.com/docs
- **Azure Docs**: https://docs.microsoft.com/azure
- **ASP.NET Core**: https://docs.microsoft.com/aspnet/core

---

## 🎉 KẾT QUẢ CUỐI CÙNG

Sau khi deploy thành công:

**🌐 Website**: `https://your-app.onrender.com` (hoặc domain của bạn)  
**👨‍💼 Admin Panel**: `https://your-app.onrender.com/Admin`  
**📧 Admin Email**: admin@shopweb.com  
**🔑 Password**: Admin@123  

---

## 🚀 BẮT ĐẦU NGAY!

### Quick Start (Render - Khuyến nghị):

1. Push code lên GitHub
2. Đăng ký Render.com
3. Tạo MySQL Database
4. Import `pc_shop2_backup.sql`
5. Tạo Web Service
6. Cấu hình environment variables
7. Deploy!

**Chi tiết**: Xem [`DEPLOY-RENDER.md`](DEPLOY-RENDER.md) ⭐

---

**Chúc bạn deploy thành công! 🎊**

Website được build ngày: **December 3, 2025**  
Version: **1.0.0**

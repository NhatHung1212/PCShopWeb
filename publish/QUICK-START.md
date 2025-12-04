# 🚀 DEPLOYMENT - QUICK START

## 📦 FILES ĐÃ SẴN SÀNG

Thư mục `C:\Users\Nhat Hung\ShopWeb\publish` chứa:
- ✅ Application files
- ✅ wwwroot (css, js, images, videos)
- ✅ Database backup: `pc_shop2_backup.sql`
- ✅ Hướng dẫn deploy đầy đủ

---

## 🎯 3 PHƯƠNG THỨC DEPLOY

### 1️⃣ RENDER.COM - MIỄN PHÍ ⭐ (KHUYẾN NGHỊ)

**Thời gian**: 30-40 phút  
**Chi phí**: FREE (100%)  
**Độ khó**: ⭐⭐⭐☆☆  

**⚠️ LƯU Ý**: Render chỉ có PostgreSQL miễn phí, KHÔNG có MySQL!

**2 CÁCH XỬ LÝ**:

**Cách 1: Dùng PostgreSQL (Khuyến nghị)**
1. Push code lên GitHub
2. Đăng ký Render.com
3. Tạo PostgreSQL Database → Convert & Import SQL
4. Sửa code: MySQL → PostgreSQL
5. Deploy!

**Cách 2: Dùng External MySQL**
1. Đăng ký Aiven.io hoặc PlanetScale (free tier)
2. Tạo MySQL database
3. Import SQL
4. Deploy Render với external MySQL connection

📖 **Chi tiết**: `DEPLOY-RENDER.md`

**Kết quả**: `https://nhpc-shop.onrender.com`

---

### 2️⃣ AZURE APP SERVICE - PROFESSIONAL

**Thời gian**: 30-45 phút  
**Chi phí**: FREE 12 tháng (cần credit card), sau đó $12-25/month  
**Độ khó**: ⭐⭐⭐☆☆  

**6 bước**:
1. Đăng ký Azure Free Tier
2. Tạo MySQL Flexible Server
3. Import database
4. Tạo App Service
5. Deploy (Visual Studio hoặc CLI)
6. Cấu hình settings

📖 **Chi tiết**: `DEPLOY-AZURE.md`

**Kết quả**: `https://nhpc-shop.azurewebsites.net`

---

### 3️⃣ INFINITYFREE - KHÔNG HỖ TRỢ ❌

**⚠️ LƯU Ý**: InfinityFree KHÔNG hỗ trợ .NET Core!

Website này cần .NET Core runtime và KHÔNG THỂ chạy trên shared hosting PHP.

📖 **Chi tiết tại sao**: `HUONG-DAN-DEPLOY.md`

---

## 🏆 SO SÁNH NHANH

|  | Render | Azure | InfinityFree |
|--|--------|-------|--------------|
| **Hỗ trợ .NET** | ✅ | ✅ | ❌ |
| **Chi phí** | FREE | $0-25/month | FREE |
| **SSL** | ✅ | ✅ | ❌ |
| **Performance** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | N/A |
| **Dễ setup** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | N/A |
| **Khuyến nghị** | **Best** | Pro | No |

---

## 🚀 BẮT ĐẦU NGAY (RENDER)

### 1. Push code lên GitHub

```powershell
cd "C:\Users\Nhat Hung\ShopWeb"
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/YOUR_USERNAME/ShopWeb.git
git push -u origin main
```

### 2. Đăng ký Render

- Truy cập: https://render.com
- Sign up với GitHub
- Authorize Render

### 3. Tạo Database

- New + → MySQL
- Name: `shopweb-db`
- Region: Singapore
- Plan: Free

### 4. Import Database

```powershell
# Dùng MySQL Workbench hoặc:
mysql -h xxx.mysql.render.com -u shopweb_user -p pc_shop2 < "C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql"
```

### 5. Tạo Web Service

- New + → Web Service
- Connect repository: ShopWeb
- Environment: Docker
- Build: `dotnet publish -c Release -o out`
- Start: `cd out && dotnet ShopWeb.dll`
- Environment Variables:
  ```
  ASPNETCORE_ENVIRONMENT=Production
  PORT=10000
  ConnectionStrings__DefaultConnection=Server=...
  DatabaseProvider=MySql
  ```

### 6. Deploy!

Click "Create Web Service" → Chờ 5-10 phút

---

## ✅ SAU KHI DEPLOY

### Test Website:

- Trang chủ: `https://your-app.onrender.com`
- Admin: `https://your-app.onrender.com/Admin`

### Đăng nhập Admin:

```
Email: admin@shopweb.com
Password: Admin@123
```

⚠️ **ĐỔI PASSWORD NGAY!**

---

## 📂 FILES HƯỚNG DẪN

```
publish/
├── README.md                    # Tổng quan (bạn đang đọc)
├── QUICK-START.md              # Hướng dẫn nhanh này
├── DEPLOY-RENDER.md            # Chi tiết deploy Render ⭐
├── DEPLOY-AZURE.md             # Chi tiết deploy Azure
├── HUONG-DAN-DEPLOY.md         # Thông tin chung
├── pc_shop2_backup.sql         # Database backup
└── appsettings.Production.json # Cấu hình (CẬP NHẬT!)
```

---

## ❓ FAQ

**Q: Nên chọn Render hay Azure?**  
A: Render nếu muốn miễn phí. Azure nếu cần performance cao và sẵn sàng trả phí sau 12 tháng.

**Q: InfinityFree có dùng được không?**  
A: KHÔNG. InfinityFree không hỗ trợ .NET Core.

**Q: Tôi không có GitHub?**  
A: Đăng ký GitHub miễn phí tại https://github.com/join

**Q: Cần credit card không?**  
A: Render: KHÔNG. Azure: CÓ (nhưng không charge trong 12 tháng).

**Q: Website chậm?**  
A: Render Free tier có cold start (~30s). Nâng cấp lên Starter ($7/month) để bỏ cold start.

---

## 🆘 CẦN GIÚP?

1. Đọc file hướng dẫn chi tiết
2. Xem logs trên platform
3. Liên hệ: 0946703205

---

## 🎯 KHUYẾN NGHỊ CUỐI CÙNG

### Cho Demo/Test: **RENDER** ⭐
- Miễn phí 100%
- Setup nhanh (20 phút)
- SSL miễn phí

### Cho Production: **AZURE**
- Performance tốt nhất
- Stable & reliable
- Professional support

---

**Chọn platform phù hợp và bắt đầu deploy ngay! 🚀**

Chúc bạn thành công! 🎉

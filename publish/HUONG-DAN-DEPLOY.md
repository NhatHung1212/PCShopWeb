# 🚀 HƯỚNG DẪN DEPLOY SHOPWEB LÊN INFINITYFREE

## ✅ ĐÃ CHUẨN BỊ XONG

Thư mục `publish` đã chứa tất cả files cần thiết để deploy:
- ✅ Build files (.dll, .exe)
- ✅ wwwroot (css, js, images, videos)
- ✅ appsettings.json & appsettings.Production.json
- ✅ web.config
- ✅ Database backup: `pc_shop2_backup.sql`

---

## 📋 BƯỚC 1: ĐĂNG KÝ INFINITYFREE

1. Truy cập: **https://infinityfree.net**
2. Click **"Sign Up"** (góc phải trên)
3. Điền thông tin:
   - Email: (email của bạn)
   - Password: (đặt mật khẩu mạnh)
4. Click **"Sign Up"**
5. Kiểm tra email và xác nhận tài khoản
6. Đăng nhập

---

## 📋 BƯỚC 2: TẠO WEBSITE

1. Sau khi đăng nhập, click **"Create Account"**
2. Điền:
   - Website Name: `nhpc-shop` (hoặc tên bạn muốn)
3. Chọn subdomain miễn phí:
   - `nhpc-shop.free.nf` (hoặc)
   - `nhpc-shop.rf.gd`
4. Đặt password cho cPanel
5. Click **"Create Account"**
6. Chờ 1-2 phút

### 📝 Ghi lại thông tin:
```
Website URL: http://nhpc-shop.free.nf
cPanel: https://cpanel.infinityfree.net
FTP Host: ftpupload.net
```

---

## 📋 BƯỚC 3: TẠO MYSQL DATABASE

### A. Tạo Database:
1. Vào cPanel → **"MySQL Databases"**
2. Phần **"Create New Database"**:
   - Database Name: `nhpc_shop`
3. Click **"Create Database"**

### B. Tạo User:
1. Phần **"MySQL Users"**:
   - Username: `shopuser`
   - Password: **(đặt password mạnh - GHI LẠI!)**
2. Click **"Create User"**

### C. Gán quyền:
1. Phần **"Add User To Database"**:
   - Chọn user: `shopuser`
   - Chọn database: `nhpc_shop`
   - Chọn **"ALL PRIVILEGES"**
2. Click **"Add"**

### 📝 GHI LẠI THÔNG TIN NÀY (RẤT QUAN TRỌNG):
```
Database Host: sqlXXX.infinityfree.net (VD: sql305.infinityfree.net)
Database Name: if0_XXXXX_nhpc_shop (VD: if0_12345678_nhpc_shop)
Username: if0_XXXXX_shopuser (VD: if0_12345678_shopuser)
Password: (password bạn đặt)
Port: 3306
```

---

## 📋 BƯỚC 4: IMPORT DATABASE

### A. Mở phpMyAdmin:
1. Trong cPanel, click **"phpMyAdmin"**
2. Tự động đăng nhập

### B. Import:
1. Bên trái, click vào database: `if0_XXXXX_nhpc_shop`
2. Click tab **"Import"** ở trên
3. Click **"Choose File"**
4. Chọn file:
   ```
   C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql
   ```
5. **LƯU Ý**: Nếu file > 10MB, scroll xuống phần **"Import file lớn"** bên dưới
6. Click **"Go"**
7. Chờ import (1-3 phút)
8. Thành công khi thấy: ✅ **"Import has been successfully finished"**

### Import file lớn (nếu > 10MB):

**Cách 1: Chia nhỏ file**
1. Mở `pc_shop2_backup.sql` bằng Notepad++
2. Tìm các dòng `INSERT INTO`
3. Chia thành nhiều file nhỏ (mỗi file ~5MB):
   - `pc_shop2_part1.sql`
   - `pc_shop2_part2.sql`
   - ...
4. Import từng file theo thứ tự

**Cách 2: Dùng BigDump**
1. Tải BigDump: https://www.oik-plugins.com/oik-plugins/bigdump/
2. Upload lên `/htdocs/bigdump/`
3. Truy cập: `http://nhpc-shop.free.nf/bigdump/bigdump.php`
4. Làm theo hướng dẫn

---

## 📋 BƯỚC 5: CẬP NHẬT CONNECTION STRING

1. Mở file:
   ```
   C:\Users\Nhat Hung\ShopWeb\publish\appsettings.Production.json
   ```

2. Sửa với thông tin từ Bước 3:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sql305.infinityfree.net;Database=if0_12345678_nhpc_shop;User=if0_12345678_shopuser;Password=YOUR_PASSWORD;Port=3306"
  },
  "DatabaseProvider": "MySql",
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**THAY ĐỔI**:
- `sql305.infinityfree.net` → Server thật
- `if0_12345678_nhpc_shop` → Database name thật
- `if0_12345678_shopuser` → Username thật
- `YOUR_PASSWORD` → Password thật

3. **LƯU FILE** (Ctrl + S)

---

## 📋 BƯỚC 6: CÀI ĐẶT FILEZILLA

1. Tải: **https://filezilla-project.org/download.php?type=client**
2. Chọn **"Windows 64-bit"**
3. Cài đặt (Next → Next → Install)

---

## 📋 BƯỚC 7: UPLOAD FILES QUA FTP

### A. Lấy thông tin FTP:
1. Vào cPanel → **"FTP Details"**
2. Ghi lại:
   ```
   Host: ftpupload.net
   Username: if0_XXXXX (VD: if0_12345678)
   Password: (password cPanel)
   Port: 21
   ```

### B. Kết nối FileZilla:
1. Mở FileZilla
2. Điền thông tin FTP:
   - Host: `ftpupload.net`
   - Username: `if0_12345678` (thay thật)
   - Password: (password cPanel)
   - Port: `21`
3. Click **"Quickconnect"**

### C. Upload files:

**QUAN TRỌNG**: Upload vào `/htdocs/` (KHÔNG phải `/public_html/`)

1. Sau khi kết nối:
   - **Bên trái**: Máy tính (local)
   - **Bên phải**: Server (remote)

2. Bên phải: Vào `/htdocs/`
3. Bên trái: Vào `C:\Users\Nhat Hung\ShopWeb\publish`

4. **Chọn TẤT CẢ files và folders** trong `publish`:
   - ShopWeb.dll
   - ShopWeb.exe
   - wwwroot/
   - appsettings.json
   - appsettings.Production.json
   - web.config
   - ... (tất cả files khác)

5. **Kéo thả** sang `/htdocs/` bên phải
   - Hoặc click chuột phải → **"Upload"**

6. Chờ upload hoàn tất (10-20 phút):
   - Videos: ~50MB
   - Tổng: ~100MB

### D. Kiểm tra cấu trúc:
```
/htdocs/
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── videos/
│   ├── images/
│   └── lib/
├── ShopWeb.dll
├── ShopWeb.exe
├── web.config
├── appsettings.json
├── appsettings.Production.json
└── runtimes/
```

---

## 📋 BƯỚC 8: TẠO FILE .HTACCESS

InfinityFree dùng Apache, cần `.htaccess`:

1. Tạo file mới trên máy:
   ```
   C:\Users\Nhat Hung\ShopWeb\publish\.htaccess
   ```

2. Nội dung:

```apache
RewriteEngine On

# Bảo vệ các files cấu hình
<Files "appsettings.*.json">
    Order Allow,Deny
    Deny from all
</Files>

# Chuyển hướng tất cả request
RewriteCond %{REQUEST_FILENAME} !-f
RewriteCond %{REQUEST_FILENAME} !-d
RewriteRule ^(.*)$ index.php [QSA,L]

# Enable error display (chỉ để debug)
php_flag display_errors on
php_value error_reporting 32767
```

3. Upload file `.htaccess` lên `/htdocs/`

---

## 📋 BƯỚC 9: KIỂM TRA WEBSITE

1. Đợi 2-5 phút để server khởi động
2. Truy cập: `http://nhpc-shop.free.nf`
3. **Lần đầu có thể chậm 30s-1 phút**

### ❌ XỬ LÝ LỖI:

#### Lỗi 404 Not Found:
- ✅ Kiểm tra files đã upload vào `/htdocs/`
- ✅ Kiểm tra file `web.config` có trong `/htdocs/`
- ✅ Upload lại file `.htaccess`

#### Lỗi 500 Internal Server Error:
1. Xem logs: cPanel → **"Error Logs"**
2. Kiểm tra connection string trong `appsettings.Production.json`
3. Kiểm tra database đã import chưa
4. Test connection trong phpMyAdmin

#### Lỗi "This site can't be reached":
- Đợi thêm 5-10 phút (DNS propagation)
- Clear cache: Ctrl + Shift + Delete
- Thử trình duyệt ẩn danh (Incognito)

#### Lỗi database connection:
1. Vào phpMyAdmin kiểm tra:
   - Database có dữ liệu chưa?
   - Tables đã được tạo chưa?
2. Test connection string:
   ```
   mysql -h sql305.infinityfree.net -u if0_XXXXX_shopuser -p if0_XXXXX_nhpc_shop
   ```

---

## 📋 BƯỚC 10: ĐĂNG NHẬP ADMIN

1. Truy cập: `http://nhpc-shop.free.nf/Account/Login`
2. Đăng nhập:
   ```
   Email: admin@shopweb.com
   Password: Admin@123
   ```
3. Admin Panel: `http://nhpc-shop.free.nf/Admin`

### ⚠️ ĐỔI PASSWORD ADMIN NGAY!

Sau khi đăng nhập thành công, đổi password admin ngay lập tức!

---

## 📋 BƯỚC 11: KIỂM TRA CHỨC NĂNG

✅ **Checklist**:
- [ ] Trang chủ hiển thị
- [ ] Video background chạy (có thể chậm)
- [ ] Danh sách sản phẩm
- [ ] Chi tiết sản phẩm
- [ ] Tìm kiếm sản phẩm
- [ ] Giỏ hàng (thêm/xóa)
- [ ] Đăng ký tài khoản
- [ ] Đăng nhập
- [ ] Checkout đặt hàng
- [ ] Admin Panel
- [ ] Quản lý sản phẩm (Admin)
- [ ] Quản lý đơn hàng (Admin)

---

## 🎉 HOÀN THÀNH!

**🌐 Website**: http://nhpc-shop.free.nf  
**👨‍💼 Admin**: http://nhpc-shop.free.nf/Admin  
**📧 Email**: admin@shopweb.com  
**🔑 Password**: Admin@123

---

## 📝 LƯU Ý VỀ INFINITYFREE

### ✅ Ưu điểm:
- ✅ 5GB disk space (đủ cho videos)
- ✅ Unlimited bandwidth
- ✅ phpMyAdmin đầy đủ
- ✅ cPanel chuyên nghiệp
- ✅ Không quảng cáo
- ✅ 99.9% uptime

### ⚠️ Hạn chế:
- ⚠️ Chậm hơn hosting trả phí (shared server)
- ⚠️ Không hỗ trợ SSL miễn phí (chỉ HTTP)
- ⚠️ Giới hạn 50,000 hits/ngày (đủ cho demo)
- ⚠️ Không chạy được .NET Core trực tiếp

---

## ⚠️ LƯU Ý QUAN TRỌNG

**InfinityFree KHÔNG hỗ trợ ASP.NET Core!**

Website này được build với ASP.NET Core và **KHÔNG THỂ chạy trên InfinityFree**.

### 🔄 CÁC LỰA CHỌN THAY THẾ:

#### 1. **Render.com** (MIỄN PHÍ - KHUYẾN NGHỊ)
- ✅ Hỗ trợ .NET Core
- ✅ Deploy tự động từ GitHub
- ✅ SSL miễn phí
- ✅ Database PostgreSQL/MySQL miễn phí
- ⚠️ Spin down sau 15 phút không hoạt động

**Xem file**: `DEPLOY-RENDER.md`

#### 2. **Azure App Service** (Miễn phí 12 tháng)
- ✅ Hỗ trợ .NET Core tốt nhất
- ✅ SSL miễn phí
- ✅ Tích hợp CI/CD
- ⚠️ Cần credit card

**Xem file**: `DEPLOY-AZURE.md`

#### 3. **DigitalOcean** ($6/tháng)
- ✅ VPS Linux với .NET Runtime
- ✅ Full control
- ✅ Performance tốt

---

## 📞 HỖ TRỢ

**Logs**: cPanel → Error Logs  
**Database**: phpMyAdmin  
**FTP**: FileZilla  
**Hotline**: 0946703205

---

## 🆙 DEPLOY LÊN RENDER (KHUYẾN NGHỊ)

Để deploy website .NET Core này, hãy làm theo file:

```
DEPLOY-RENDER.md
```

Hoặc tôi có thể hướng dẫn deploy lên Render ngay bây giờ!

---

**Chúc bạn deploy thành công! 🚀**

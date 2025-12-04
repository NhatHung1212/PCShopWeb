# 🚀 DEPLOY SHOPWEB LÊN RENDER.COM (MIỄN PHÍ - KHUYẾN NGHỊ)

## ✅ TẠI SAO CHỌN RENDER?

- ✅ **Hỗ trợ .NET Core 10.0**
- ✅ **Miễn phí 100%** (có giới hạn)
- ✅ **SSL miễn phí** (HTTPS tự động)
- ✅ **Deploy tự động từ GitHub**
- ✅ **Database PostgreSQL miễn phí** (1GB, 30 ngày)
- ✅ **Dễ sử dụng**
- ⚠️ Spin down sau 15 phút không hoạt động (khởi động lại ~30s)

## ⚠️ LƯU Ý QUAN TRỌNG VỀ DATABASE

**Render chỉ cung cấp PostgreSQL miễn phí, KHÔNG có MySQL miễn phí!**

Website này đang dùng MySQL. Bạn có 2 lựa chọn:

### Lựa chọn 1: Dùng External MySQL (Khuyến nghị - Dễ nhất)
- ✅ Không cần sửa code
- ✅ Dùng **Aiven.io** (MySQL miễn phí, 25MB)
- ✅ Hoặc **Railway.app** ($5 credit/month)

**→ Xem file**: `DATABASE-SOLUTIONS.md` để biết chi tiết!

### Lựa chọn 2: Chuyển sang PostgreSQL
- ⚠️ Cần sửa code một chút
- ⚠️ Cần convert database
- ✅ Hoàn toàn miễn phí trên Render

**Hướng dẫn bên dưới sẽ dùng Aiven.io (External MySQL)**

---

## 📋 BƯỚC 1: PUSH CODE LÊN GITHUB

### A. Tạo GitHub Repository:

1. Truy cập: **https://github.com/new**
2. Điền:
   - Repository name: `ShopWeb`
   - Description: `NH12 PC Shop - ASP.NET Core`
   - Public (hoặc Private)
3. **KHÔNG** chọn "Add README"
4. Click **"Create repository"**

### B. Push code lên GitHub:

Mở PowerShell trong thư mục project:

```powershell
cd "C:\Users\Nhat Hung\ShopWeb"

# Khởi tạo git (nếu chưa có)
git init

# Thêm tất cả files
git add .

# Commit
git commit -m "Initial commit - ShopWeb"

# Thêm remote (thay YOUR_USERNAME)
git remote add origin https://github.com/YOUR_USERNAME/ShopWeb.git

# Push lên GitHub
git push -u origin main
```

**Lưu ý**: Nếu lỗi, thử:
```powershell
git branch -M main
git push -u origin main
```

---

## 📋 BƯỚC 2: ĐĂNG KÝ RENDER

1. Truy cập: **https://render.com**
2. Click **"Get Started for Free"**
3. Đăng ký bằng:
   - **GitHub** (khuyến nghị)
   - Hoặc Google/Email
4. Xác nhận email

---

## 📋 BƯỚC 3: TẠO MYSQL DATABASE (AIVEN.IO)

**Render không có MySQL miễn phí, dùng Aiven.io thay thế!**

### A. Đăng ký Aiven:

1. Truy cập: **https://console.aiven.io/signup**
2. Sign up với email hoặc GitHub
3. Verify email
4. Đăng nhập

### B. Tạo MySQL Service:

1. Click **"Create service"**
2. Chọn:
   - Service: **MySQL**
   - Cloud: **AWS**
   - Region: **Singapore** (aws-ap-southeast-1)
   - Plan: **Hobbyist** (FREE - 25MB)
3. Service name: `shopweb-mysql`
4. Click **"Create service"**
5. Chờ 2-5 phút (status → Running)

### C. Tạo Database:

1. Click vào service `shopweb-mysql`
2. Tab **"Databases"**
3. Click **"Create database"**
4. Database name: `pc_shop2`
5. Click **"Create"**

### D. Lấy Connection Info:

1. Tab **"Overview"**
2. Copy thông tin:
   ```
   Host: mysql-shopweb-mysql-xxx.aivencloud.com
   Port: 12345 (ví dụ)
   User: avnadmin
   Password: (click Show)
   Database: pc_shop2
   ```

**GHI LẠI** connection string:
```
Server=mysql-shopweb-mysql-xxx.aivencloud.com;Port=12345;Database=pc_shop2;User=avnadmin;Password=YOUR_PASSWORD;SslMode=Required
```

---

## 📋 BƯỚC 4: IMPORT DATABASE VÀO AIVEN

### Cách 1: Dùng MySQL Workbench (Khuyến nghị)

1. Mở **MySQL Workbench**
2. Tạo connection mới:
   - Connection Name: `Aiven ShopWeb`
   - Hostname: `mysql-shopweb-mysql-xxx.aivencloud.com` (từ Aiven)
   - Port: (từ Aiven, ví dụ: 12345)
   - Username: `avnadmin`
   - Password: (từ Aiven)
   - Default Schema: `pc_shop2`
   - SSL: **Require**
3. Click **"Test Connection"**
4. Nếu OK, click **"OK"**
5. Kết nối vào database
6. Click **"Server"** → **"Data Import"**
7. Chọn **"Import from Self-Contained File"**
8. Browse: `C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql`
9. Default Target Schema: `pc_shop2`
10. Click **"Start Import"**

### Cách 2: Dùng Command Line

```powershell
# Vào thư mục MySQL bin
cd "C:\xampp\mysql\bin"

# Import (thay thông tin thật từ Aiven)
.\mysql.exe -h mysql-shopweb-mysql-xxx.aivencloud.com `
            -P 12345 `
            -u avnadmin `
            -p `
            --ssl-mode=REQUIRED `
            pc_shop2 < "C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql"
```

Nhập password khi được hỏi.

---

## 📋 BƯỚC 5: TẠO WEB SERVICE

### A. Tạo Web Service:

1. Trong Dashboard Render, click **"New +"**
2. Chọn **"Web Service"**
3. Connect GitHub repository:
   - Click **"Connect GitHub"**
   - Authorize Render
   - Chọn repository: `ShopWeb`
4. Click **"Connect"**

### B. Cấu hình:

1. **General**:
   - Name: `nhpc-shop`
   - Region: **Singapore**
   - Branch: `main`
   - Root Directory: (để trống)

2. **Build & Deploy**:
   - Environment: **Docker** (hoặc **.NET**)
   - Build Command:
     ```
     dotnet publish -c Release -o out
     ```
   - Start Command:
     ```
     cd out && dotnet ShopWeb.dll
     ```

3. **Plans**:
   - Instance Type: **Free**

4. Click **"Advanced"**

### C. Environment Variables:

Click **"Add Environment Variable"** và thêm:

| Key | Value |
|-----|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `PORT` | `10000` |
| `ConnectionStrings__DefaultConnection` | `Server=mysql-shopweb-mysql-xxx.aivencloud.com;Port=12345;Database=pc_shop2;User=avnadmin;Password=YOUR_PASSWORD;SslMode=Required` |
| `DatabaseProvider` | `MySql` |

**Lưu ý**: Thay thông tin database thật từ Aiven (Bước 3)!  
**Quan trọng**: Phải có `SslMode=Required` cho Aiven!

### D. Deploy:

1. Click **"Create Web Service"**
2. Render sẽ tự động:
   - Clone code từ GitHub
   - Build project
   - Deploy
3. Chờ 5-10 phút

---

## 📋 BƯỚC 6: CẤU HÌNH RENDER.YAML (TÙY CHỌN)

Tạo file `render.yaml` trong root project để tự động hóa:

```yaml
services:
  - type: web
    name: nhpc-shop
    env: docker
    region: singapore
    plan: free
    buildCommand: dotnet publish -c Release -o out
    startCommand: cd out && dotnet ShopWeb.dll
    envVars:
      - key: ASPNETCORE_ENVIRONMENT
        value: Production
      - key: PORT
        value: 10000
      - key: DatabaseProvider
        value: MySql
      - key: ConnectionStrings__DefaultConnection
        sync: false

databases:
  - name: shopweb-db
    databaseName: pc_shop2
    user: shopweb_user
    region: singapore
    plan: free
```

Push lên GitHub:
```powershell
git add render.yaml
git commit -m "Add Render configuration"
git push
```

---

## 📋 BƯỚC 7: KIỂM TRA DEPLOYMENT

### A. Xem Logs:

1. Trong Render Dashboard
2. Click vào service `nhpc-shop`
3. Tab **"Logs"**
4. Xem quá trình build và deploy

### B. Kiểm tra Status:

- **Live**: ✅ Website đang chạy
- **Building**: ⏳ Đang build
- **Deploy failed**: ❌ Có lỗi

### C. Truy cập Website:

1. URL sẽ có dạng:
   ```
   https://nhpc-shop.onrender.com
   ```
2. Copy URL và truy cập
3. **Lần đầu**: Chờ 30s-1 phút (cold start)

---

## 📋 BƯỚC 8: TEST CHỨC NĂNG

✅ **Checklist**:
- [ ] Trang chủ hiển thị (HTTPS)
- [ ] Video background chạy
- [ ] Danh sách sản phẩm
- [ ] Chi tiết sản phẩm
- [ ] Tìm kiếm
- [ ] Giỏ hàng
- [ ] Đăng ký/Đăng nhập
- [ ] Checkout
- [ ] Admin Panel: `https://nhpc-shop.onrender.com/Admin`

### Đăng nhập Admin:
```
Email: admin@shopweb.com
Password: Admin@123
```

---

## ❌ XỬ LÝ LỖI

### Lỗi Build Failed:

**Xem logs**:
1. Tab "Logs"
2. Tìm dòng lỗi màu đỏ

**Lỗi thường gặp**:

#### 1. Thiếu .NET SDK:
Thêm vào `render.yaml`:
```yaml
buildCommand: |
  curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0
  export PATH="$HOME/.dotnet:$PATH"
  dotnet publish -c Release -o out
```

#### 2. Database connection failed:
- Kiểm tra connection string
- Test trong MySQL Workbench
- Kiểm tra database đã import chưa

### Lỗi Deploy Failed:

#### 1. Port conflict:
Đảm bảo `Program.cs` có:
```csharp
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Clear();
app.Urls.Add($"http://0.0.0.0:{port}");
```

#### 2. Static files không load:
Kiểm tra `wwwroot` đã được publish:
```powershell
dotnet publish -c Release -o out
```

### Website chậm (Cold Start):

**Nguyên nhân**: Free tier spin down sau 15 phút không hoạt động

**Giải pháp**:
1. Nâng cấp lên **Starter** ($7/month)
2. Hoặc dùng service ping:
   - **UptimeRobot**: https://uptimerobot.com
   - Ping mỗi 5 phút để giữ active

---

## 🔄 TỰ ĐỘNG DEPLOY KHI PUSH CODE

1. Sau khi setup xong, mỗi lần push code:
   ```powershell
   git add .
   git commit -m "Update features"
   git push
   ```

2. Render tự động:
   - Detect thay đổi
   - Build lại
   - Deploy
   - ~5-10 phút

---

## 🎉 HOÀN THÀNH!

**🌐 Website**: https://nhpc-shop.onrender.com  
**👨‍💼 Admin**: https://nhpc-shop.onrender.com/Admin  
**📧 Email**: admin@shopweb.com  
**🔑 Password**: Admin@123  

---

## 📝 SO SÁNH HOSTING

| Feature | Render + Aiven | InfinityFree | Azure |
|---------|----------------|--------------|-------|
| .NET Core | ✅ | ❌ | ✅ |
| SSL | ✅ | ❌ | ✅ |
| Database | ✅ MySQL (Aiven 25MB) | ✅ MySQL | ✅ |
| Bandwidth | Unlimited | Unlimited | Limited |
| Uptime | 99%+ | 99%+ | 99.9%+ |
| Cold Start | 30s | N/A | 0s |
| Price | FREE + FREE | FREE | $13/month |
| Setup | 30-40 phút | N/A | 45-60 phút |

---

## 🆙 NÂNG CẤP

### Render Starter ($7/month):
- No cold start
- Custom domain
- More resources

### Render Standard ($25/month):
- Higher performance
- More memory
- Priority support

---

## 📞 HỖ TRỢ

**Render Docs**: https://render.com/docs  
**Community**: https://community.render.com  
**Hotline**: 0946703205

---

**Chúc bạn deploy thành công! 🚀**

Website của bạn sẽ có SSL (HTTPS), chạy ổn định và hoàn toàn MIỄN PHÍ!

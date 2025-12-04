# 🔄 GIẢI PHÁP DATABASE CHO RENDER

## ⚠️ VẤN ĐỀ: RENDER KHÔNG CÓ MYSQL MIỄN PHÍ

**Render chỉ cung cấp miễn phí**:
- ✅ PostgreSQL (1GB, 30 ngày)
- ❌ KHÔNG có MySQL miễn phí

**Website của bạn đang dùng**: MySQL

---

## 🎯 2 GIẢI PHÁP

### 🥇 GIẢI PHÁP 1: DÙNG EXTERNAL MYSQL (MIỄN PHÍ)

Dùng MySQL từ nhà cung cấp khác (miễn phí):

#### A. Aiven.io - MySQL Free (KHUYẾN NGHỊ)

**Ưu điểm**:
- ✅ MySQL 8.0
- ✅ 25MB storage (đủ cho demo)
- ✅ Miễn phí vĩnh viễn
- ✅ Không cần credit card
- ✅ Singapore region

**Đăng ký**:
1. Truy cập: https://aiven.io/free-mysql-database
2. Sign up với email/GitHub
3. Chọn **Free Plan**:
   - Service: **MySQL**
   - Cloud: **AWS Singapore**
   - Plan: **Hobbyist (FREE)**
4. Database name: `pc_shop2`
5. Create service (2-3 phút)

**Lấy connection**:
```
Host: mysql-xxx.aivencloud.com
Port: 12345
Database: pc_shop2
User: avnadmin
Password: (tự động tạo)
```

**Import database**:
```powershell
# Dùng MySQL Workbench hoặc:
mysql -h mysql-xxx.aivencloud.com -P 12345 -u avnadmin -p pc_shop2 < "C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql"
```

#### B. PlanetScale - MySQL Serverless

**Ưu điểm**:
- ✅ MySQL compatible
- ✅ 5GB storage
- ✅ 1 billion row reads/month
- ✅ Branching (như Git)

**Nhược điểm**:
- ⚠️ Không hỗ trợ Foreign Keys

**Đăng ký**:
1. https://planetscale.com
2. Sign up với GitHub
3. Create database: `pc_shop2`
4. Get connection string

#### C. Railway.app - MySQL

**Ưu điểm**:
- ✅ MySQL 8.0
- ✅ $5 free credit/month (đủ cho demo)
- ✅ Dễ dùng

**Đăng ký**:
1. https://railway.app
2. Sign up với GitHub
3. New Project → Database → MySQL
4. Import SQL

---

### 🥈 GIẢI PHÁP 2: CHUYỂN SANG POSTGRESQL

**Ưu điểm**:
- ✅ Hoàn toàn miễn phí trên Render
- ✅ 1GB storage
- ✅ Không cần service ngoài
- ✅ Tích hợp tốt với Render

**Nhược điểm**:
- ⚠️ Cần sửa code một chút
- ⚠️ Convert database schema

---

## 🚀 HƯỚNG DẪN CHI TIẾT: DÙNG AIVEN MYSQL (KHUYẾN NGHỊ)

### BƯỚC 1: Đăng ký Aiven

1. Truy cập: **https://console.aiven.io/signup**
2. Sign up:
   - Email hoặc GitHub
3. Verify email
4. Đăng nhập

### BƯỚC 2: Tạo MySQL Service

1. Click **"Create service"**
2. Chọn:
   - Service: **MySQL**
   - Cloud: **AWS** 
   - Region: **Singapore** (aws-ap-southeast-1)
   - Plan: **Hobbyist** (FREE)
3. Service name: `shopweb-mysql`
4. Click **"Create service"**
5. Chờ 2-5 phút (status: Running)

### BƯỚC 3: Lấy Connection Info

1. Click vào service: `shopweb-mysql`
2. Tab **"Overview"**
3. Copy thông tin:
   ```
   Host: mysql-shopweb-mysql-xxx.aivencloud.com
   Port: 12345 (ví dụ)
   User: avnadmin
   Password: (click "Show" để xem)
   Database: defaultdb
   ```

**GHI LẠI** connection string:
```
Server=mysql-shopweb-mysql-xxx.aivencloud.com;Port=12345;Database=defaultdb;User=avnadmin;Password=YOUR_PASSWORD;SslMode=Required
```

### BƯỚC 4: Tạo Database

1. Tab **"Databases"**
2. Click **"Create database"**
3. Database name: `pc_shop2`
4. Click **"Create"**

### BƯỚC 5: Import Database

#### Cách 1: MySQL Workbench

1. Mở MySQL Workbench
2. New Connection:
   - Connection Name: `Aiven ShopWeb`
   - Hostname: (từ Aiven)
   - Port: (từ Aiven)
   - Username: `avnadmin`
   - Password: (từ Aiven)
   - Default Schema: `pc_shop2`
   - SSL: **Require**
3. Test Connection → OK
4. Connect
5. Server → Data Import
6. Import from Self-Contained File:
   ```
   C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql
   ```
7. Default Target Schema: `pc_shop2`
8. Start Import

#### Cách 2: Command Line

```powershell
cd "C:\xampp\mysql\bin"

.\mysql.exe -h mysql-shopweb-mysql-xxx.aivencloud.com `
            -P 12345 `
            -u avnadmin `
            -p `
            --ssl-mode=REQUIRED `
            pc_shop2 < "C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql"
```

### BƯỚC 6: Deploy lên Render

1. Push code lên GitHub
2. Đăng ký Render.com
3. New → Web Service
4. Connect repository: `ShopWeb`
5. Configure:
   - Build Command: `dotnet publish -c Release -o out`
   - Start Command: `cd out && dotnet ShopWeb.dll`

6. **Environment Variables**:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   PORT=10000
   DatabaseProvider=MySql
   ConnectionStrings__DefaultConnection=Server=mysql-shopweb-mysql-xxx.aivencloud.com;Port=12345;Database=pc_shop2;User=avnadmin;Password=YOUR_PASSWORD;SslMode=Required
   ```

7. Create Web Service
8. Chờ deploy (5-10 phút)

---

## 🔄 HƯỚNG DẪN: CHUYỂN SANG POSTGRESQL

Nếu muốn dùng PostgreSQL miễn phí của Render:

### BƯỚC 1: Cài Package PostgreSQL

```powershell
cd "C:\Users\Nhat Hung\ShopWeb"
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### BƯỚC 2: Sửa Program.cs

```csharp
// Thay thế phần MySQL configuration
if (databaseProvider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase))
{
    var pgConnection = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection is not configured.");
    
    options.UseNpgsql(pgConnection);
}
else if (databaseProvider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
{
    // Giữ code MySQL hiện tại
}
```

### BƯỚC 3: Convert Database

#### A. Dùng pgLoader (Tự động)

```bash
# Install pgLoader trên Linux/Mac
apt-get install pgloader

# Convert
pgloader mysql://root@localhost/pc_shop2 postgresql://user:pass@render-host/pc_shop2
```

#### B. Export/Import thủ công

1. Export data từ MySQL
2. Sửa SQL syntax cho PostgreSQL:
   - `AUTO_INCREMENT` → `SERIAL`
   - `DATETIME` → `TIMESTAMP`
   - Backticks `` ` `` → Double quotes `"`
3. Import vào PostgreSQL

### BƯỚC 4: Update appsettings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=xxx.render.com;Database=pc_shop2;Username=user;Password=pass"
  },
  "DatabaseProvider": "PostgreSql"
}
```

### BƯỚC 5: Test Local

```powershell
dotnet build
dotnet run
```

### BƯỚC 6: Deploy Render

Push code và deploy như bình thường.

---

## 📊 SO SÁNH GIẢI PHÁP

| Feature | Aiven MySQL | PlanetScale | Railway | PostgreSQL |
|---------|-------------|-------------|---------|------------|
| **Chi phí** | FREE | FREE | $5 credit | FREE |
| **Storage** | 25MB | 5GB | ~1GB | 1GB |
| **Thời hạn** | Vĩnh viễn | Vĩnh viễn | Monthly | 30 ngày |
| **Cần sửa code** | ❌ | ❌ | ❌ | ✅ |
| **Credit card** | ❌ | ❌ | ❌ | ❌ |
| **Khuyến nghị** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ |

---

## ✅ KHUYẾN NGHỊ CUỐI CÙNG

### Cho bạn (ShopWeb):

**→ DÙNG AIVEN.IO + RENDER**

**Lý do**:
1. ✅ Không cần sửa code
2. ✅ Hoàn toàn miễn phí
3. ✅ Dễ setup (30 phút)
4. ✅ MySQL giữ nguyên
5. ✅ 25MB đủ cho demo

**Quy trình**:
1. Đăng ký Aiven → Tạo MySQL
2. Import database
3. Deploy Render với Aiven connection string
4. Done! 🎉

---

## 🆘 XỬ LÝ LỖI

### Lỗi: Can't connect to MySQL server

**Nguyên nhân**: SSL/Firewall

**Giải pháp**:
1. Thêm `SslMode=Required` vào connection string
2. Check firewall trong Aiven console
3. Allow Render IPs

### Lỗi: Database size exceeded

**Nguyên nhân**: Aiven free = 25MB

**Giải pháp**:
1. Xóa dữ liệu test không cần
2. Nâng cấp Aiven ($10/month cho 100MB)
3. Chuyển sang Railway ($5 credit/month)

---

## 📞 HỖ TRỢ

**Aiven Docs**: https://docs.aiven.io/docs/products/mysql  
**PlanetScale Docs**: https://planetscale.com/docs  
**Railway Docs**: https://docs.railway.app  
**Hotline**: 0946703205

---

**Chúc bạn setup database thành công! 🚀**

Khuyến nghị: **Aiven.io** - Đơn giản, miễn phí, không cần sửa code!

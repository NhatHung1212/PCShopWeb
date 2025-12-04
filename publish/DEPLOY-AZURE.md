# 🚀 DEPLOY SHOPWEB LÊN AZURE APP SERVICE

## ✅ TẠI SAO CHỌN AZURE?

- ✅ **Hỗ trợ .NET Core tốt nhất** (Microsoft native)
- ✅ **SSL miễn phí** (HTTPS tự động)
- ✅ **Miễn phí 12 tháng** (cần credit card)
- ✅ **Performance cao**
- ✅ **Scale dễ dàng**
- ✅ **CI/CD tích hợp sẵn**
- ⚠️ Cần credit card để đăng ký
- ⚠️ Sau 12 tháng: $13/month

---

## 📋 BƯỚC 1: ĐĂNG KÝ AZURE

### A. Tạo tài khoản:

1. Truy cập: **https://azure.microsoft.com/free**
2. Click **"Start free"**
3. Đăng nhập bằng:
   - Microsoft Account
   - Hoặc tạo mới
4. Điền thông tin:
   - Country: **Vietnam**
   - Phone: Số điện thoại của bạn
   - Credit/Debit card: **BẮT BUỘC** (không bị charge trong 12 tháng free)
5. Xác minh SMS
6. Hoàn tất đăng ký

### B. Kích hoạt Free Tier:

Azure Free Tier bao gồm:
- ✅ 12 tháng miễn phí cho nhiều services
- ✅ $200 credit trong 30 ngày đầu
- ✅ 25+ services luôn miễn phí

**Lưu ý**: Cần credit card nhưng KHÔNG bị charge trừ khi bạn nâng cấp.

---

## 📋 BƯỚC 2: TẠO RESOURCE GROUP

1. Đăng nhập: **https://portal.azure.com**
2. Tìm **"Resource groups"** (hoặc search)
3. Click **"+ Create"**
4. Điền:
   - Subscription: **Free Trial**
   - Resource group name: `ShopWeb-RG`
   - Region: **Southeast Asia** (Singapore - gần VN)
5. Click **"Review + create"**
6. Click **"Create"**

---

## 📋 BƯỚC 3: TẠO AZURE DATABASE FOR MYSQL

### A. Tạo Database Server:

1. Trong Portal, search **"Azure Database for MySQL"**
2. Click **"+ Create"**
3. Chọn **"Flexible Server"**
4. Điền:

**Basics**:
- Subscription: **Free Trial**
- Resource group: `ShopWeb-RG`
- Server name: `shopweb-mysql-server` (unique globally)
- Region: **Southeast Asia**
- MySQL version: **8.0**
- Compute + storage: Click **"Configure server"**
  - Chọn **Burstable, B1ms** (Cheapest)
  - Storage: **20 GB**
  - Click **"Save"**

**Authentication**:
- Admin username: `shopwebadmin`
- Password: (đặt password mạnh - GHI LẠI!)
- Confirm password: (nhập lại)

**Networking**:
- Connectivity method: **Public access (allowed IP addresses)**
- Firewall rules:
  - ✅ Check **"Allow public access from any Azure service"**
  - Add current client IP: **Yes**

5. Click **"Review + create"**
6. Click **"Create"**
7. Chờ 5-10 phút

### B. Tạo Database:

Sau khi MySQL Server sẵn sàng:

1. Vào MySQL Server: `shopweb-mysql-server`
2. Bên trái, click **"Databases"**
3. Click **"+ Add"**
4. Database name: `pc_shop2`
5. Charset: **utf8mb4**
6. Collation: **utf8mb4_unicode_ci**
7. Click **"Save"**

### C. Lấy Connection String:

1. Vào MySQL Server
2. Bên trái, click **"Connection strings"**
3. Copy **"ADO.NET"** hoặc **"JDBC"**
4. Format lại:
   ```
   Server=shopweb-mysql-server.mysql.database.azure.com;
   Database=pc_shop2;
   User=shopwebadmin;
   Password=YOUR_PASSWORD;
   Port=3306;
   SslMode=Required
   ```

**GHI LẠI** connection string này!

---

## 📋 BƯỚC 4: IMPORT DATABASE

### Cách 1: MySQL Workbench (Khuyến nghị)

1. Mở **MySQL Workbench**
2. Tạo connection mới:
   - Connection Name: `Azure ShopWeb`
   - Hostname: `shopweb-mysql-server.mysql.database.azure.com`
   - Port: `3306`
   - Username: `shopwebadmin`
   - Password: (password bạn đặt)
   - Default Schema: `pc_shop2`
   - SSL: **Require**
3. Click **"Test Connection"**
4. Nếu OK, click **"OK"**
5. Kết nối vào
6. Click **"Server"** → **"Data Import"**
7. **"Import from Self-Contained File"**
8. Browse: `C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql`
9. Default Target Schema: `pc_shop2`
10. Click **"Start Import"**

### Cách 2: Azure Cloud Shell

1. Trong Azure Portal, click icon **Cloud Shell** (>_) góc trên
2. Chọn **Bash**
3. Upload file SQL:
   - Click icon upload
   - Chọn `pc_shop2_backup.sql`
4. Chạy:
   ```bash
   mysql -h shopweb-mysql-server.mysql.database.azure.com \
         -u shopwebadmin \
         -p \
         pc_shop2 < pc_shop2_backup.sql
   ```
5. Nhập password

### Cách 3: Command Line (Local)

```powershell
cd "C:\xampp\mysql\bin"

.\mysql.exe -h shopweb-mysql-server.mysql.database.azure.com `
            -u shopwebadmin `
            -p `
            --ssl-mode=REQUIRED `
            pc_shop2 < "C:\Users\Nhat Hung\ShopWeb\publish\pc_shop2_backup.sql"
```

---

## 📋 BƯỚC 5: TẠO APP SERVICE

### A. Tạo App Service:

1. Trong Portal, search **"App Services"**
2. Click **"+ Create"**
3. Điền:

**Basics**:
- Subscription: **Free Trial**
- Resource group: `ShopWeb-RG`
- Name: `nhpc-shop` (hoặc tên khác - phải unique)
- Publish: **Code**
- Runtime stack: **.NET 8** (hoặc 7 nếu có)
- Operating System: **Linux** (rẻ hơn Windows)
- Region: **Southeast Asia**

**Pricing**:
- Pricing plan: **Free F1** (1GB RAM, 60 min/day)
  - Hoặc **Basic B1** ($13/month nhưng tốt hơn)

4. Click **"Review + create"**
5. Click **"Create"**
6. Chờ 2-3 phút

### B. Cấu hình:

1. Vào App Service: `nhpc-shop`
2. Bên trái, click **"Configuration"**
3. Tab **"Application settings"**
4. Click **"+ New application setting"**

Thêm các settings:

| Name | Value |
|------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | `Server=shopweb-mysql-server.mysql.database.azure.com;Database=pc_shop2;User=shopwebadmin;Password=YOUR_PASSWORD;Port=3306;SslMode=Required` |
| `DatabaseProvider` | `MySql` |
| `WEBSITE_RUN_FROM_PACKAGE` | `1` |

5. Click **"Save"**
6. Click **"Continue"**

---

## 📋 BƯỚC 6: DEPLOY VIA VISUAL STUDIO (DỄ NHẤT)

### A. Mở Visual Studio:

1. Mở solution: `C:\Users\Nhat Hung\ShopWeb\ShopWeb.sln`
2. Right-click vào project **ShopWeb**
3. Chọn **"Publish..."**

### B. Chọn Target:

1. Target: **Azure**
2. Click **"Next"**
3. Specific target: **Azure App Service (Linux)**
4. Click **"Next"**

### C. Chọn App Service:

1. Đăng nhập Azure (nếu chưa)
2. Subscription: **Free Trial**
3. Resource group: `ShopWeb-RG`
4. Chọn: `nhpc-shop`
5. Click **"Next"**

### D. Publish:

1. API Management: **Skip this step**
2. Click **"Finish"**
3. Click **"Publish"**
4. Chờ deploy (3-5 phút)
5. Browser sẽ tự động mở website

---

## 📋 BƯỚC 7: DEPLOY VIA COMMAND LINE

Nếu không dùng Visual Studio:

### A. Cài Azure CLI:

```powershell
# Download và cài
winget install Microsoft.AzureCLI
```

### B. Login:

```powershell
az login
```

### C. Deploy:

```powershell
cd "C:\Users\Nhat Hung\ShopWeb"

# Publish
dotnet publish -c Release -o publish

# Zip files
Compress-Archive -Path publish\* -DestinationPath publish.zip -Force

# Deploy
az webapp deployment source config-zip `
    --resource-group ShopWeb-RG `
    --name nhpc-shop `
    --src publish.zip
```

---

## 📋 BƯỚC 8: DEPLOY VIA GITHUB ACTIONS (TỰ ĐỘNG)

### A. Push lên GitHub:

```powershell
cd "C:\Users\Nhat Hung\ShopWeb"

git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/YOUR_USERNAME/ShopWeb.git
git push -u origin main
```

### B. Tạo Deployment Center:

1. Vào App Service: `nhpc-shop`
2. Bên trái, click **"Deployment Center"**
3. Source: **GitHub**
4. Authorize Azure
5. Chọn:
   - Organization: (your GitHub username)
   - Repository: `ShopWeb`
   - Branch: `main`
6. Build provider: **GitHub Actions**
7. Runtime stack: **.NET 8**
8. Click **"Save"**

### C. Azure tự động:

- Tạo file `.github/workflows/azure-webapps-dotnet.yml`
- Commit vào repo
- Mỗi lần push code → Tự động deploy

---

## 📋 BƯỚC 9: CẤU HÌNH CUSTOM DOMAIN (TÙY CHỌN)

Nếu có domain riêng:

1. Vào App Service → **"Custom domains"**
2. Click **"+ Add custom domain"**
3. Domain: `www.yourshop.com`
4. Làm theo hướng dẫn thêm DNS records
5. Click **"Validate"**
6. Click **"Add"**

### SSL Certificate:

1. Vào **"TLS/SSL settings"**
2. Click **"+ Add TLS/SSL binding"**
3. Chọn domain
4. TLS/SSL type: **SNI SSL** (Free)
5. Click **"Add Binding"**

---

## 📋 BƯỚC 10: KIỂM TRA & TEST

### A. Truy cập Website:

URL: `https://nhpc-shop.azurewebsites.net`

### B. Test:

✅ **Checklist**:
- [ ] Trang chủ (HTTPS)
- [ ] Video background
- [ ] Danh sách sản phẩm
- [ ] Chi tiết sản phẩm
- [ ] Tìm kiếm
- [ ] Giỏ hàng
- [ ] Đăng ký/Đăng nhập
- [ ] Checkout
- [ ] Admin: `https://nhpc-shop.azurewebsites.net/Admin`

### C. Đăng nhập Admin:

```
Email: admin@shopweb.com
Password: Admin@123
```

⚠️ **Đổi password ngay!**

---

## 📊 MONITORING & LOGS

### A. Xem Logs:

1. Vào App Service
2. Bên trái, click **"Log stream"**
3. Xem real-time logs

### B. Application Insights:

1. Vào App Service
2. Bên trái, click **"Application Insights"**
3. Click **"Turn on Application Insights"**
4. Theo dõi:
   - Response time
   - Failed requests
   - Server exceptions

---

## ❌ XỬ LÝ LỖI

### Lỗi 503 Service Unavailable:

**Nguyên nhân**: App chưa start hoặc crash

**Giải pháp**:
1. Xem logs trong **Log stream**
2. Kiểm tra **Application settings**
3. Restart App Service:
   ```
   App Service → Overview → Restart
   ```

### Lỗi Database Connection:

1. Kiểm tra connection string
2. Test trong MySQL Workbench
3. Kiểm tra Firewall rules:
   ```
   MySQL Server → Networking → Firewall rules
   → Add "Allow Azure services"
   ```

### Lỗi Static Files 404:

Đảm bảo `wwwroot` được publish:
```powershell
dotnet publish -c Release -o publish
```

---

## 💰 CHI PHÍ

### Free Tier (12 tháng đầu):

| Service | Free | Sau 12 tháng |
|---------|------|--------------|
| App Service F1 | FREE | $0 (vẫn free) |
| MySQL Burstable B1ms | FREE | $12/month |
| Total | **$0** | **$12/month** |

### Basic Tier (Performance tốt hơn):

| Service | Giá |
|---------|-----|
| App Service B1 | $13/month |
| MySQL Flexible B1ms | $12/month |
| Total | **$25/month** |

---

## 🔄 SCALE & PERFORMANCE

### Scale Up (Tăng resources):

1. Vào App Service → **"Scale up (App Service plan)"**
2. Chọn tier cao hơn:
   - **Basic B1**: $13/month (1 core, 1.75GB RAM)
   - **Standard S1**: $75/month (1 core, 1.75GB RAM, auto-scale)
   - **Premium P1V2**: $100/month (1 core, 3.5GB RAM, better)

### Scale Out (Tăng instances):

1. Vào App Service → **"Scale out (App Service plan)"**
2. Chọn số instances: 1-10

---

## 🎉 HOÀN THÀNH!

**🌐 Website**: https://nhpc-shop.azurewebsites.net  
**👨‍💼 Admin**: https://nhpc-shop.azurewebsites.net/Admin  
**📧 Email**: admin@shopweb.com  
**🔑 Password**: Admin@123  

---

## 📝 TỔNG KẾT

### Ưu điểm Azure:

- ✅ Performance cao nhất
- ✅ Hỗ trợ .NET Core native
- ✅ Auto-scale
- ✅ SSL miễn phí
- ✅ CI/CD tích hợp
- ✅ Monitoring tốt
- ✅ 99.95% SLA

### Nhược điểm:

- ⚠️ Cần credit card
- ⚠️ Có chi phí sau 12 tháng
- ⚠️ Phức tạp hơn Render

---

## 📞 HỖ TRỢ

**Azure Docs**: https://docs.microsoft.com/azure  
**Support**: Azure Portal → Help + support  
**Community**: https://docs.microsoft.com/answers  
**Hotline**: 0946703205  

---

**Chúc bạn deploy thành công trên Azure! 🚀**

Website của bạn sẽ có performance cao nhất với Azure!

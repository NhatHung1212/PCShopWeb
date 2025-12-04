# NH12 PC Shop - Deployment Guide

## Production Build Completed ✅

Build location: `C:\Users\Nhat Hung\ShopWeb\publish`

## Deployment Options

### Option 1: IIS Deployment (Windows Server)

1. **Prerequisites:**
   - Install IIS on Windows Server
   - Install ASP.NET Core Runtime 10.0
   - Install MySQL Server

2. **Setup:**
   ```powershell
   # Enable IIS features
   Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
   Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServer
   Enable-WindowsOptionalFeature -Online -FeatureName IIS-ASPNET45
   ```

3. **Deploy:**
   - Copy `publish` folder to `C:\inetpub\wwwroot\shopweb`
   - Create IIS Application Pool (No Managed Code)
   - Create IIS Website pointing to the folder
   - Configure bindings (port 80/443)

4. **Update Configuration:**
   - Edit `appsettings.Production.json` with production database credentials

### Option 2: Kestrel (Self-hosted)

Run directly on server:
```powershell
cd C:\path\to\publish
set ASPNETCORE_ENVIRONMENT=Production
.\ShopWeb.exe
```

### Option 3: Docker Deployment

1. **Create Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY publish/ .
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ShopWeb.dll"]
```

2. **Build and Run:**
```bash
docker build -t shopweb .
docker run -d -p 8080:80 --name shopweb shopweb
```

### Option 4: Linux Server (Ubuntu/Debian)

1. **Install .NET Runtime:**
```bash
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0 --runtime aspnetcore
```

2. **Setup Nginx Reverse Proxy:**
```nginx
server {
    listen 80;
    server_name yourdomain.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

3. **Create systemd service:**
```bash
sudo nano /etc/systemd/system/shopweb.service
```

```ini
[Unit]
Description=NH12 PC Shop Web Application

[Service]
WorkingDirectory=/var/www/shopweb
ExecStart=/usr/bin/dotnet /var/www/shopweb/ShopWeb.dll
Restart=always
RestartSec=10
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

4. **Start Service:**
```bash
sudo systemctl enable shopweb
sudo systemctl start shopweb
```

## Database Setup

1. **Import database:**
```bash
mysql -u root -p pc_shop2 < database_backup.sql
```

2. **Update connection string** in `appsettings.Production.json`

3. **Test connection:**
```bash
mysql -u your_user -p -h your_host pc_shop2
```

## Post-Deployment Checklist

- ✅ Database connection verified
- ✅ Static files (wwwroot) accessible
- ✅ Videos folder permissions set
- ✅ HTTPS certificate installed (recommended)
- ✅ Firewall rules configured
- ✅ Environment variables set
- ✅ Logging configured
- ✅ Backup strategy in place

## Application URLs

- **Development:** http://localhost:5008
- **Production:** http://your-domain.com

## Support

For issues, check logs in `publish/logs/` directory.

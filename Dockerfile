# مرحله 1: استفاده از SDK برای ساخت پروژه
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# کپی کردن فایل‌های پروژه
COPY ["NikuAPI.csproj", "./"]
RUN dotnet restore

# کپی کردن سایر فایل‌ها و ساخت پروژه
COPY . .
RUN dotnet publish -c Release -o /app

# مرحله 2: استفاده از Runtime برای اجرای برنامه
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .

# تنظیم متغیر محیطی برای استفاده از تنظیمات Production
ENV ASPNETCORE_ENVIRONMENT=Production

# باز کردن پورت
EXPOSE 80

# اجرای برنامه
ENTRYPOINT ["dotnet", "NikuAPI.dll"]

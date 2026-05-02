# 🚀 Guía Rápida de Ejecución - SuEnvioSeguro MVP

## ⚡ Quick Start (5 minutos)

### 1️⃣ Requisitos Previos
- ✅ SQL Server corriendo (puerto 1433)
- ✅ .NET 10 SDK instalado
- ✅ Node.js 18+ instalado

### 2️⃣ Configurar Backend
```powershell
cd SuEnvioSeguro.API
dotnet restore
dotnet ef database update
```

### 3️⃣ Configurar Frontend
```powershell
cd suenvioseguro-web
npm install

# Crear archivo .env
copy .env.example .env
```

### 4️⃣ Ejecutar (2 Terminales)

**Terminal 1:**
```powershell
cd SuEnvioSeguro.API
dotnet run
```

**Terminal 2:**
```powershell
cd suenvioseguro-web
npm run dev
```

### 5️⃣ Acceder
- 🌐 Web: http://localhost:5174
- 📚 API Docs: http://localhost:5096/swagger/ui

### 🔑 Login
```
Usuario: admin.123456789
Contraseña: Password123!
```

---

## 📖 Documentación Completa

Ver **REQUIREMENTS.md** para:
- Instalación detallada
- Configuración avanzada
- Troubleshooting
- Requisitos de seguridad

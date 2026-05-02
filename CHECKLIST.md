# ✅ Verificación de Instalación - SuEnvioSeguro MVP

Use esta checklist antes de ejecutar la aplicación para asegurar que todo está correctamente configurado.

## 📋 Pre-Instalación

### Sistema Operativo & Herramientas
- [ ] Windows 10 o superior (o equivalente en Linux/Mac)
- [ ] PowerShell o terminal compatible
- [ ] Acceso administrador a la máquina

## 🔧 Dependencias del Sistema

### SQL Server
- [ ] SQL Server Express o Developer instalado
- [ ] SQL Server corriendo (verificar en Services)
- [ ] SQL Server Management Studio (SSMS) instalado (opcional pero recomendado)
  ```powershell
  # Verificar conexión
  sqlcmd -S localhost,1433 -U sa -P TuContraseña
  ```

### .NET 10 SDK
- [ ] .NET 10 SDK descargado e instalado
- [ ] Verificar versión:
  ```powershell
  dotnet --version
  # Debe mostrar: 10.0.0 o superior
  ```

### Node.js & npm
- [ ] Node.js 18.0 o superior instalado
- [ ] npm incluido (generalmente automático con Node.js)
- [ ] Verificar versiones:
  ```powershell
  node --version  # Debe ser v18.0.0 o superior
  npm --version   # Debe ser 9.0.0 o superior
  ```

## 📦 Backend - SuEnvioSeguro.API

### Estructura de Carpetas
```
SuEnvioSeguro.API/
├── Program.cs
├── SuEnvioSeguro.API.csproj
├── SuEnvioSeguro.API.http
├── appsettings.json
├── appsettings.Development.json
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── Persona.cs
│   ├── Usuario.cs
│   ├── Cliente.cs
│   ├── Factura.cs
│   ├── Envio.cs
│   └── Municipio.cs
├── Controllers/
└── Services/
```
- [ ] Todos los archivos presentes

### Dependencias NuGet
```
- [ ] BCrypt.Net-Next v4.1.0
- [ ] Microsoft.AspNetCore.Authentication.JwtBearer v10.0.7
- [ ] Microsoft.AspNetCore.OpenApi v10.0.2
- [ ] Microsoft.EntityFrameworkCore.Design v10.0.7
- [ ] Microsoft.EntityFrameworkCore.SqlServer v10.0.7
- [ ] Swashbuckle.AspNetCore v10.1.7
```

Restaurar dependencias:
```powershell
cd SuEnvioSeguro.API
dotnet restore
# [ ] Sin errores
```

### Configuración de Base de Datos
- [ ] `appsettings.Development.json` contiene:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost,1433;Database=SuEnvioSeguroDB;User Id=sa;Password=TuContraseña;TrustServerCertificate=true;"
    },
    "Jwt": {
      "SecretKey": "tu-clave-secreta-minimo-32-caracteres",
      "ExpirationMinutes": 240
    }
  }
  ```

### Crear Base de Datos
```powershell
cd SuEnvioSeguro.API
dotnet ef database update
# [ ] Todas las migraciones aplicadas correctamente
# [ ] BD SuEnvioSeguroDB creada en SQL Server
```

Verificar en SSMS:
```sql
-- [ ] Puedes conectar a localhost,1433
-- [ ] Existe la base de datos SuEnvioSeguroDB
-- [ ] Existen las tablas: Personas, Usuarios, Clientes, Facturas, Envios, Municipios
```

### Compilación
```powershell
cd SuEnvioSeguro.API
dotnet build
# [ ] Compilación exitosa sin errores
```

### Ejecución Inicial
```powershell
cd SuEnvioSeguro.API
dotnet run
# [ ] Mensaje: "Now listening on: http://localhost:5096"
# [ ] Swagger disponible en http://localhost:5096/swagger/ui
```

## 🎨 Frontend - suenvioseguro-web

### Estructura de Carpetas
```
suenvioseguro-web/
├── package.json
├── vite.config.js
├── eslint.config.js
├── tailwind.config.js
├── postcss.config.js
├── .env (crear)
├── .env.example
├── index.html
├── src/
│   ├── main.jsx
│   ├── App.jsx
│   ├── App.css
│   ├── index.css
│   └── assets/
├── public/
└── dist/ (generado en build)
```
- [ ] Todos los archivos presentes

### Dependencias npm
```
Producción:
- [ ] axios ^1.16.0
- [ ] react ^19.2.5
- [ ] react-dom ^19.2.5
- [ ] react-router-dom ^7.14.2

Desarrollo:
- [ ] @eslint/js ^10.0.1
- [ ] eslint ^10.2.1
- [ ] vite ^8.0.10
- [ ] tailwindcss ^3.4.19
- [ ] autoprefixer ^10.5.0
- [ ] postcss ^8.5.13
- [ ] @vitejs/plugin-react ^6.0.1
- [ ] @types/react ^19.2.14
- [ ] @types/react-dom ^19.2.3
- [ ] eslint-plugin-react-hooks ^7.1.1
- [ ] eslint-plugin-react-refresh ^0.5.2
- [ ] globals ^17.5.0
```

Instalar dependencias:
```powershell
cd suenvioseguro-web
npm install
# [ ] Sin errores críticos
```

### Variables de Entorno
- [ ] Archivo `.env` creado (copiar de `.env.example`)
- [ ] Contenido:
  ```env
  VITE_API_URL=http://localhost:5096/api
  ```

### Linting
```powershell
cd suenvioseguro-web
npm run lint
# [ ] Sin errores críticos (warnings de Tailwind son normales)
```

### Build de Producción
```powershell
cd suenvioseguro-web
npm run build
# [ ] Compilación exitosa
# [ ] Archivos generados en carpeta dist/
```

### Ejecución Desarrollo
```powershell
cd suenvioseguro-web
npm run dev
# [ ] Mensaje: "Local: http://localhost:5174"
# [ ] Accesible desde navegador
```

## 🔐 Datos Iniciales

### Usuario de Prueba (Creado Automáticamente)
```
Documento Identidad: 123456789
Nombre Usuario: admin.123456789
Contraseña: Password123!
Rol: ADMIN
```

- [ ] Intenta login con estas credenciales

## 🌐 Verificación de Conectividad

### Backend
```powershell
# Debe retornar respuesta (puede ser error 404, eso es normal)
curl http://localhost:5096/api/
# [ ] Respuesta exitosa o 404 esperado
```

### Frontend
```
Abre en navegador: http://localhost:5174
# [ ] Página carga correctamente
# [ ] Formulario de login visible
```

### Comunicación Backend-Frontend
```
1. En navegador ir a http://localhost:5174
2. Ingresa: admin.123456789 / Password123!
# [ ] Login exitoso
# [ ] Redirección al dashboard
# [ ] Puedes ver módulos de facturación, seguimiento, empleados
```

## 📊 Verificación Funcional

### Módulo de Facturación
- [ ] Ir a Facturacion tab
- [ ] Completar formulario con datos de prueba
- [ ] Click en "Generar factura"
- [ ] Factura aparece en panel derecho
- [ ] Campos mostrados: Código, Estado, Destino, Descripción
- [ ] Click en "🖨️ Imprimir Factura"
- [ ] Se abre ventana de impresión

### Módulo de Seguimiento
- [ ] Ir a Seguimiento tab
- [ ] Ver tabla con envíos creados
- [ ] Buscar por código
- [ ] Cambiar estado
- [ ] Actualizar estado

### Módulo de Empleados (Solo Admin)
- [ ] Ir a Empleados tab
- [ ] Crear nuevo empleado
- [ ] Editar empleado
- [ ] Desactivar/Activar empleado

## 🐛 Troubleshooting

Si algo falla, verificar:

### Error de Conexión Base de Datos
```
[ ] SQL Server está corriendo
[ ] Contraseña correcta en appsettings.Development.json
[ ] Ejecutar: dotnet ef database update
[ ] Verificar en SSMS: localhost,1433
```

### Error de Puerto en Uso
```powershell
# Encontrar proceso en puerto
netstat -ano | findstr :5096
# Matar proceso
taskkill /PID <PID> /F

# Para otro puerto
dotnet run --urls "http://localhost:5097"
```

### Error de Dependencias npm
```powershell
cd suenvioseguro-web
rm -r node_modules
npm cache clean --force
npm install
```

### Frontend no conecta con Backend
```
[ ] Verificar .env tiene: VITE_API_URL=http://localhost:5096/api
[ ] Backend corriendo en puerto 5096
[ ] CORS habilitado en backend
```

## ✨ Verificación Final

```
[ ] Backend compilado y corriendo (dotnet run)
[ ] Frontend corriendo (npm run dev)
[ ] Base de datos creada y poblada
[ ] Login funciona
[ ] Puedes crear, leer, actualizar envíos
[ ] Impresión de facturas funciona
[ ] Todos los campos se muestran correctamente
```

## 🎉 ¡Listo!

Si todas las casillas están marcadas, la instalación fue exitosa.

**Para iniciar nuevamente:**
```powershell
# Terminal 1
cd SuEnvioSeguro.API
dotnet run

# Terminal 2
cd suenvioseguro-web
npm run dev

# Abrir navegador
http://localhost:5174
```

---

**Documentación adicional:** Ver `REQUIREMENTS.md` para instalación completa y `QUICKSTART.md` para ejecución rápida.

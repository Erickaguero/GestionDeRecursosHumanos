# 🏢 Sistema de Gestión de Recursos Humanos

**Proyecto de Graduación** - Erick Agüero

Un sistema web completo para la gestión integral de recursos humanos, desarrollado en ASP.NET Core MVC que permite administrar colaboradores, nóminas, asistencias, permisos y más.

## 📋 Tabla de Contenidos

- [Características Principales](#-características-principales)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Funcionalidades](#-funcionalidades)
- [Base de Datos](#-base-de-datos)
- [Capturas de Pantalla](#-capturas-de-pantalla)
- [Contribuciones](#-contribuciones)
- [Licencia](#-licencia)

## ✨ Características Principales

### 🔐 Autenticación y Seguridad
- Sistema de login con JWT (JSON Web Tokens)
- Recuperación de contraseñas por correo electrónico
- Control de roles (Administrador/Empleado)
- Validación de estado de colaboradores

### 👥 Gestión de Personal
- **Mantenimiento de Colaboradores**: CRUD completo de empleados
- **Mantenimiento de Departamentos**: Administración de áreas
- **Mantenimiento de Puestos**: Gestión de posiciones laborales
- Información personal completa con validaciones

### ⏰ Control de Tiempo y Asistencias
- **Registro de Asistencias**: Entrada y salida de empleados
- **Gestión de Horas Extra**: Solicitud y aprobación
- **Sistema de Permisos**: Control de permisos laborales
- **Gestión de Incapacidades**: Ausencias por enfermedad
- **Administración de Vacaciones**: Días de descanso

### 💰 Gestión de Nómina
- **Generación de Planillas**: Cálculo automático de salarios
- **Sistema de Aguinaldos**: Bonificaciones anuales
- **Procesos de Liquidación**: Finiquitos de empleados
- Cálculo de deducciones (CCSS, Renta)

### 📊 Reportes y Análisis
- **Consultas y Reportes**: Análisis de datos del personal
- **Simulaciones**: Herramientas de proyección
- **Evaluación de Empleados**: Sistema de evaluación de desempeño
- **Dashboard Principal**: Resumen ejecutivo

## 🛠️ Tecnologías Utilizadas

### Backend
- **ASP.NET Core MVC** (.NET 8)
- **C#** como lenguaje principal
- **ADO.NET** para acceso a datos
- **SQL Server** como base de datos

### Frontend
- **Razor Views** para interfaces
- **Bootstrap** para diseño responsivo
- **JavaScript** para interactividad
- **CSS** personalizado

### Servicios Externos
- **JWT** para autenticación
- **SMTP** para envío de correos
- **Rotativa** para generación de PDFs

### Paquetes NuGet
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Rotativa.AspNetCore" Version="1.3.2" />
<PackageReference Include="System.Data.SqlClient" Version="4.8.6" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.5.2" />
```

## 🚀 Instalación y Configuración

### Prerrequisitos
- Visual Studio 2022 o superior
- .NET 8 SDK
- SQL Server (Local o Express)
- SQL Server Management Studio (opcional)

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/tu-usuario/PrototipoFuncionalRecursosHumanos.git
   cd PrototipoFuncionalRecursosHumanos
   ```

2. **Configurar la base de datos**
   - Crear la base de datos `PrototipoRecursosHumanos` en SQL Server
   - Ejecutar los scripts de creación de tablas y stored procedures
   - Verificar la cadena de conexión en `appsettings.json`

3. **Configurar el correo electrónico**
   ```json
   "Email": {
     "Username": "tu-email@gmail.com",
     "Password": "tu-app-password"
   }
   ```

4. **Ejecutar el proyecto**
   ```bash
   dotnet run
   ```
   O abrir en Visual Studio y presionar F5

5. **Acceder a la aplicación**
   - Navegar a `https://localhost:7000`
   - Usar las credenciales de administrador por defecto

## 📁 Estructura del Proyecto

```
PrototipoFuncionalRecursosHumanos/
├── Controllers/           # Controladores MVC
│   ├── HomeController.cs
│   ├── PlanillaController.cs
│   ├── MantenimientoColaboradoresController.cs
│   └── ...
├── Models/               # Entidades de datos
│   ├── Colaborador.cs
│   ├── Usuario.cs
│   ├── Planilla.cs
│   └── ...
├── Handlers/             # Lógica de acceso a datos
│   ├── ColaboradorHandler.cs
│   ├── UsuarioHandler.cs
│   ├── PlanillaHandler.cs
│   └── ...
├── Services/             # Servicios de negocio
│   ├── Autenticador.cs
│   ├── EnviadorCorreos.cs
│   └── ...
├── Views/                # Vistas Razor
│   ├── Home/
│   ├── Planilla/
│   ├── MantenimientoColaboradores/
│   └── ...
└── wwwroot/              # Archivos estáticos
    ├── css/
    ├── js/
    ├── images/
    └── lib/
```

## 🎯 Funcionalidades Detalladas

### Para Administradores
- ✅ Gestión completa de colaboradores
- ✅ Administración de departamentos y puestos
- ✅ Generación de planillas y aguinaldos
- ✅ Aprobación de solicitudes (horas extra, permisos, etc.)
- ✅ Generación de reportes y consultas
- ✅ Configuración del sistema

### Para Empleados
- ✅ Visualización de información personal
- ✅ Solicitud de horas extra
- ✅ Solicitud de permisos
- ✅ Solicitud de vacaciones
- ✅ Consulta de asistencias
- ✅ Dashboard personalizado

## 🗄️ Base de Datos

### Configuración
- **Motor**: SQL Server
- **Base de Datos**: `PrototipoRecursosHumanos`
- **Esquema**: `mydb`
- **Autenticación**: Windows Authentication

### Tablas Principales
- `persona` - Información personal
- `usuario` - Credenciales de acceso
- `colaborador` - Datos laborales
- `departamento` - Áreas de la empresa
- `puesto` - Posiciones laborales
- `asistencia` - Registro de tiempo
- `planilla` - Datos de nómina
- `horasextra` - Solicitudes de horas adicionales
- `permisos` - Permisos laborales
- `vacaciones` - Días de descanso

### Stored Procedures
El sistema utiliza múltiples stored procedures para operaciones complejas:
- `CrearColaborador`
- `GenerarPlanillaColaboradores`
- `CrearAguinaldoColaboradores`
- `AprobarHorasExtra`
- Y muchos más...

## 📸 Capturas de Pantalla

> *Las capturas de pantalla se pueden agregar aquí mostrando las principales funcionalidades del sistema*

## 🤝 Contribuciones

Este es un proyecto de graduación, pero las contribuciones son bienvenidas:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👨‍💻 Autor

**Erick Agüero**
- Proyecto de Graduación
- Universidad [Nombre de la Universidad]
- Año: 2024

## 📞 Contacto

Si tienes preguntas sobre este proyecto, puedes contactarme en:
- Email: [tu-email@ejemplo.com]
- LinkedIn: [tu-linkedin]
- GitHub: [tu-github]

---

⭐ Si este proyecto te fue útil, ¡dale una estrella!

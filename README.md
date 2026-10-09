# DVLD – Driving & Vehicle Licensing System

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-5.0%2B-blue.svg)](https://dotnet.microsoft.com/)

## 📖 Overview
The **Driving & Vehicle Licensing System (DVLD)** is a Windows‑desktop application that streamlines the issuance, renewal, and management of driving licences. Built with a 3‑tier architecture (Presentation → Business Logic → Data Access) using C# .NET WinForms, it provides a secure, performant, and user‑friendly interface for licensing officers, data entry clerks, and system administrators.

---
## 📚 Documentation
- **[User Requirements Specification (URS)](./URS.md)** – Operational goals and user expectations.
- **[Software Requirements Specification (SRS)](./SRS.md)** – Technical design, functional and non‑functional requirements.
- **Images & Mockups** – All UI screenshots, diagrams, and design assets are located in the `DVLD_Shared/images/` folder.

---
## 📋 Table of Contents
- [Overview](#-overview)
- [Features](#-features)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Documentation](#-documentation)
- [Contributing](#-contributing)
- [License](#-license)

---
## ✨ Features
- **Role‑Based Access Control** – Admin, Licensing Officer, Data Entry Clerk.
- **Secure Password Handling** – Single‑pass salted hashing, never stored in plain text.
- **Full License Lifecycle** – New issuance, international licenses, renewals, replacements, and detainment.
- **Real‑Time Dashboard** – Card‑based metrics showing application counts and status.
- **Robust Data Integrity** – Parameterized SQL, transaction handling, and comprehensive logging using `StringBuilder`.

---
## 🚀 Getting Started
### Prerequisites
1. Windows 10/11
2. .NET Framework 4.8 (or later) / .NET 6+ SDK
3. SQL Server (Express or full edition) with a database named `DVLD`
4. Visual Studio 2022 (or any IDE supporting C# WinForms)

### Installation
```powershell
# Clone the repository
git clone https://github.com/your‑org/DVLD.git
cd DVLD

# Open the solution in Visual Studio and restore NuGet packages
# Build the solution (Debug/Release)
# Update the connection string in DVLD\App.config if needed
# Run the application (DVLD.exe)
```

### Database Setup
- Execute the provided `Setup/DVLD_Schema.sql` script against your SQL Server instance to create tables, indexes, and stored procedures.
- Optionally run `Setup/DVLD_SeedData.sql` to insert sample data for testing.

---
## ⚙️ Configuration
The application reads its connection string and logging options from **App.config** using `ConfigurationManager`. Example snippet:
```xml
<connectionStrings>
  <add name="DVLDDB" connectionString="Data Source=.;Initial Catalog=DVLD;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```
Adjust the `Data Source` and authentication method to match your environment.

---
## 🤝 Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/YourFeature`).
3. Commit your changes with clear messages.
4. Open a Pull Request describing the changes.

Please ensure that new code adheres to the existing coding standards and that any added functionality is reflected in the SRS/URS documentation.

---
## 📄 License
This project is licensed under the **MIT License** – see the [LICENSE](LICENSE) file for details.

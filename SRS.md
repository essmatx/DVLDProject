# Software Requirements Specification (SRS)

## 1. System Architecture
| Layer | Technology | Responsibilities |
|-------|------------|------------------|
| **Presentation (UI)** | C# .NET WinForms | Render forms, cards, and dashboards; perform client‑side validation; invoke Business Logic services. |
| **Business Logic** | C# .NET Class Library | Enforce domain rules (e.g., password hashing policy, workflow state transitions); coordinate transactions between UI and Data Access. |
| **Data Access** | ADO.NET, SQL Server | Execute parameterized queries, map result sets to domain objects, manage connection strings via `ConfigurationManager`. |
| **Configuration Management** | `App.config` | Store `connectionString`, app settings, and logging options; loaded at runtime with `ConfigurationManager`. |

All layers communicate through well‑defined interfaces, enabling unit testing and future UI replacement (e.g., WPF or web).

## 2. Detailed Functional Requirements

| ID | Requirement |
|----|-------------|
| **FR‑1.0 – User Management & Password Security** | • Create, read, update, deactivate user accounts.<br>• Passwords hashed once using a strong algorithm (e.g., PBKDF2, SHA‑256 + salt).<br>• Hashing occurs only on password set/change; system detects existing hash to avoid double‑hashing.<br>• Account status toggles (Active/Inactive) enforce login eligibility. |
| **FR‑2.0 – Person Information Management & Search Filters** | • Store citizen data: Person ID, National ID, name, DOB, address, contact, photo (binary BLOB).<br>• Provide searchable UI with filters: Person ID, National ID, name partial match.<br>• Validation of mandatory fields; photo upload limited to JPEG/PNG ≤ 2 MB. |
| **FR‑3.0 – Application & Licensing Workflows** | • **Local License** – Capture applicant data, issue, and store license number.<br>• **International License** – Additional fields (passport number, country), export XML for foreign authority.<br>• **Renewal** – Verify existing license, generate renewal fee invoice, issue new certificate.<br>• **Lost/Damaged Replacement** – Flag original license as “lost”, generate replacement, track cost.<br>• **Detain/Release** – Ability to suspend a license pending legal review and later reactivate. |
| **FR‑4.0 – Metric Aggregation & Dashboard Reporting** | • Real‑time counts for: Total Applications, Pending, International, Cancelled/Rejected.<br>• Aggregations performed via SQL `COUNT` with appropriate indexes for O(1) response.<br>• Dashboard updates automatically on data change (event‑driven refresh). |

## 3. External Interface & Configuration Requirements
| Item | Specification |
|------|----------------|
| **App.config** | `<connectionStrings>` element containing a **SQL Server** provider‑specific string (`Data Source=.;Initial Catalog=DVLD;Integrated Security=True`). The system reads it at startup via `ConfigurationManager.ConnectionStrings["DVLDDB"].ConnectionString`. |
| **Database Drivers** | Microsoft .NET `System.Data.SqlClient` (or `Microsoft.Data.SqlClient` for .NET Core). |
| **WinForms Controls** | `DataGridView` for list views, `ComboBox` for look‑ups, `ErrorProvider` for validation, `PictureBox` for photos, `Button`/`ToolStrip` for actions, custom `UserControl` for KPI cards. |
| **Logging Interface** | `ILogger` implementation using `StringBuilder` to concatenate log entries before persisting to file/database, minimizing per‑write allocations. |
| **Configuration UI** | Admin screen to edit connection string and toggle logging level (Info/Debug/Error). Changes persisted back to `App.config` and applied on next launch. |

## 4. Non‑Functional Requirements (NFRs)
| Category | Requirement |
|----------|-------------|
| **Security** | • All SQL statements use **parameterized queries** (`SqlCommand.Parameters`) to prevent injection.<br>• Password handling follows **single‑pass hashing** with per‑user salt; no reversible encryption stored.<br>• Role‑based access enforced at UI and Business Logic layers. |
| **Performance & Memory** | • Data retrieval limited to required columns; indexes on `PersonID`, `NationalID`, and `ApplicationStatus`.<br>• Logging and dynamic query building leverage `StringBuilder` to avoid excessive string allocations.<br>• UI remains responsive (< 200 ms) for common operations (lookup, dashboard refresh). |
| **Reliability & Data Integrity** | • All multi‑step operations (e.g., application creation → license issuance) are wrapped in **SQL transactions**; rollback on failure.<br>• Centralized exception handling logs full stack traces and alerts the admin UI.<br>• Backup/restore scripts provided for the SQL database (full‑copy and point‑in‑time). |
| **Maintainability** | • 3‑Tier separation permits independent unit testing of each layer.<br>• Interfaces (`IUserService`, `IPersonRepository`, `ILicenseWorkflow`) enable mocking and future refactoring. |
| **Scalability** | • Designed for single‑server deployment; scaling out would involve moving the Data Access layer to a web service (e.g., WCF/ASP.NET Core) without UI changes. |
| **Usability** | • Forms designed for < 5 clicks to complete a typical workflow.<br>• Tooltips, inline validation, and keyboard shortcuts reduce training time. |
| **Portability** | • Target .NET Framework 4.8 (or .NET 6+ if re‑targeted) on Windows 10/11; no native dependencies beyond SQL Server. |

*Document generated on 2026‑10‑09*

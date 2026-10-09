# User Requirements Specification (URS)

## 1. System Purpose & Objectives
| Objective | Description |
|-----------|--------------|
| **Streamlined Licensing** | Enable front‑line staff to process driving‑license requests (new, renewal, international, replacement, detainment) quickly and accurately. |
| **Centralized Citizen Registry** | Maintain a single source of truth for citizen personal data, contact information, and identity photographs, indexed by National ID / Person ID. |
| **Secure Access Control** | Provide role‑based authentication and robust password handling to protect sensitive personal and licensing data. |
| **Operational Visibility** | Deliver an administrative dashboard that surfaces real‑time key performance indicators (KPIs) for monitoring workload and service levels. |
| **Compliance & Data Integrity** | Enforce SQL‑injection protection, transaction safety, and audit logging to satisfy regulatory and audit requirements. |

## 2. User Roles & Access Levels
| Role | Permissions | Typical Tasks |
|------|-------------|----------------|
| **System Administrator** | Full system configuration, user/role management, database connection settings. | Create/disable users, assign roles, modify `App.config`, monitor system health. |
| **Counter / Licensing Officer** | Create, review, approve, reject, or detain license applications; view dashboard metrics. | Process citizen applications, update application status, generate licenses, view KPI cards. |
| **Data Entry Clerk** | Insert and edit person‑registry records; limited read‑only access to licensing data. | Register new citizens, update contact details, upload identity photos, perform citizen look‑ups. |

## 3. Functional User Expectations
1. **Rapid Citizen Lookup & Registration** – Search by National ID or Person ID with instant results; add new citizens in a single, validated form.
2. **Fast Application Entry & Status Tracking** – One‑click creation of license applications, real‑time status bar, and searchable application history.
3. **Secure Credential Management** – Passwords are hashed exactly once on creation or change; no plaintext storage; administrators can safely edit user profiles without breaking authentication.
4. **Clear Operational Metrics on Launch** – Upon opening the application, a card‑based dashboard presents totals for *Total Applications*, *Pending*, *International*, and *Cancelled/Rejected* without additional navigation.

## 4. User Experience & Interface Requirements
| Requirement | Detail |
|-------------|--------|
| **Card‑Based Navigation Layout** | Home screen composed of clickable metric cards that open corresponding sub‑screens (e.g., “Pending Applications” → list view). |
| **Immediate Input Validation** | All data entry controls employ `ErrorProvider` components; invalid fields are highlighted before submission. |
| **Uncluttered Forms** | Each form displays only the fields relevant to the current workflow step; optional sections are collapsible. |
| **Consistent Look & Feel** | WinForms UI adheres to a unified colour palette and typography; icons and tooltips convey purpose without reliance on text. |
| **Accessibility** | Keyboard shortcuts for primary actions; screen‑reader friendly labels via the `AccessibleName` property. |

---
*Document generated on 2026‑10‑09*

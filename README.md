# 📘 ChandiTechnology Web API

A RESTful Web API built using **ASP.NET Core (.NET 8)** that provides secure **Agent authentication** and **Hotel search integration** using the Hotelbeds API.

---

## 🚀 Project Overview

The **ChandiTechnology Web API** allows travel agents to:

- Register and authenticate securely
- Obtain JWT tokens for authorized access
- Search hotels via Hotelbeds external API
- Maintain audit logs for system activities

---

## 🛠️ Technology Stack

- **Framework:** ASP.NET Core (.NET 8)
- **Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Token
- **External API:** Hotelbeds API
- **Architecture:** RESTful API

---

# ⚙️ Setup Instructions

## 1️⃣ Prerequisites

Install the following:

- .NET 8 SDK
- SQL Server (Express / Standard)
- Visual Studio 2022+ or VS Code
- Postman (recommended)

---


Add New Migration (After Model Changes)

Whenever you modify entities/models:

dotnet ef migrations add UpdateTables
dotnet ef database update
Remove Last Migration (If Needed)
dotnet ef migrations remove
Check Migration List
dotnet ef migrations list
▶️ Run the Application
dotnet run




## 2️⃣ Clone Repository

```bash
git clone https://github.com/SusavanMondal/ChandiTechnologyWebApi.git
cd ChandiTechnologyWebApi
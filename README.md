# 🧬 Family Tree Manager – Backend

This is the backend REST API for the Family Tree Manager — a service that powers the family tree frontend interface. It handles family member data, user management, and secure media storage, all backed by a MariaDB database.

---

## Features

- Manage family members, relationships, and user access
- Role-based permissions: View-only or Manager
- Support for uploading and retrieving profile photos
- RESTful endpoints to power the Next.js frontend

---

## Technologies Used

- .NET 9
- MariaDB
- ASP.NET Core Web API
- (Optional) Entity Framework Core for database migrations

---

## Getting Started

### Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [MariaDB](https://mariadb.org/) installed locally or accessible remotely

---

### Setup Instructions

1. **Clone the repository**

```
git clone https://github.com/your-username/your-backend-repo.git
cd your-backend-repo
```

2. **Configure the database connection**

Open `appsettings.json` and update the connection string:

```json
"ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3307;database=familytree;user=root;password=;"
  },
```

3. **Restore dependencies and build the project**

```
dotnet restore
dotnet build
```

4. **Run the application**

```
dotnet run
```

By default, the API will be available at:

- [https://localhost:5001](https://localhost:5001)
- [http://localhost:5000](http://localhost:5000)

---

### (Optional) Apply Migrations with Entity Framework Core

If you're using EF Core:

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Make sure you have the `Microsoft.EntityFrameworkCore.Tools` package installed.

---

## API Overview

| Method | Endpoint            | Description                        |
| ------ | ------------------- | ---------------------------------- |
| GET    | `/api/members`      | Get all family members             |
| POST   | `/api/members`      | Add a new family member            |
| PUT    | `/api/members/{id}` | Update a family member             |
| DELETE | `/api/members/{id}` | Delete a family member             |
| GET    | `/api/users`        | List users                         |
| POST   | `/api/users/invite` | Invite a user with a specific role |

> Note: All endpoints require authentication.

---

## Authentication & Authorization

- **Viewer** – can only read/view family data
- **Manager** – can add, edit, delete, and invite users

Authentication method (e.g., JWT, cookie, or session) can be configured as needed in `Startup.cs` or `Program.cs`.

---

## Media Handling

- Profile photo uploads are accepted as `multipart/form-data`
- Images are stored and returned via secure URLs for frontend use
- Backend serves or proxies image access securely

---

## Development Tips

- Use `dotnet watch run` for auto-reloading during development
- Use Swagger (via `Swashbuckle.AspNetCore`) for auto-generating API docs:

  - Add this in `Program.cs`:

    ```csharp
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    ```

  - Then access docs at `/swagger` after launching the app

---

## Deployment Notes

- Host on any .NET-compatible environment (Windows, Linux, Azure, etc.)
- Ensure environment variables or `appsettings.Production.json` are configured
- Use reverse proxy like NGINX if deploying behind HTTPS

---

## License

MIT License

---

## Author

**Henry Kakembo**
[GitHub](https://github.com/kakembohenri)

```

Let me know if you want me to generate a Swagger example or Postman collection reference too — I can add that in!
```

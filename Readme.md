# 🩺 Medical Appointments API (MVP)

This API is a Minimum Viable Product designed to manage medical appointments efficiently.
It allows patients to schedule appointments, doctors to create and manage their availability, and administrators to perform privileged actions for system management and oversight.

---

## 🛠 Technologies Used

- **.NET 8**  
- **Entity Framework Core**  
- **MediatR**  
- **Docker** & **Docker Compose**  
- **SQL Server** (in Docker)  
- **Redis** (optional, also containerized)  
- **Swagger / OpenAPI** for API documentation  
- **JWT** for authentication and authorization  
- **API Key support** for quick testing without login (Postman / Swagger)

⚙️ CI/CD with GitHub Actions + Docker Hub

This project is fully integrated with **GitHub Actions** for automated builds, tests, and Docker image deployment to Docker Hub.

### ✅ Pipeline Features

- Restores and builds the solution using `.NET 8`
- Runs all integration and unit tests with `xUnit`
- Builds the Docker image from the `MedicalAppts.Api/Dockerfile`
- Pushes the image to Docker Hub using the GitHub `secrets`:
  - `DOCKER_USERNAME`
  - `DOCKER_PASSWORD`

Once pushed, the Docker image will be available at:

```
https://hub.docker.com/r/fredoni/medicalappts
```

---

## 🐳 Docker & Setup

The app is fully containerized using **Docker** and orchestrated with **Docker Compose**.

### 🚀 To run the project:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/medical-appts-api.git
   ```
2. Open the solution in Visual Studio or VS Code.

3. Run the project using Docker (make sure Docker is running):

   From Visual Studio: just hit **F5**

   From CLI:
   ```bash
   docker compose up --build
   ```

---

🔐 Admin Credentials (Auto-Seeded)

The admin user is automatically created during the database seeding process (`InitialSeed.cs`):

- **Email**: admin@medicalapp.com
- **Password**: Admin123!

---

📡 API Authentication

This API supports two authentication mechanisms:

- **JWT Bearer Tokens** (standard login via `/login` endpoint)
- **API Key** for quick testing (no login required)

> **Header Name**: `AUTH-API-KEY`
>
> **Example Values**:
> - `api-safe-key-PATIENT` → Authenticates as role `PATIENT`
> - `api-safe-key-DOCTOR` → Authenticates as role `DOCTOR`
> - `api-safe-key-ADMIN` → Authenticates as role `ADMIN`

When an API key is provided, the system automatically assigns the corresponding role, allowing role-based authorization without needing to log in.

---

📦 Postman Collection

You can find a ready-to-use **Postman Collection** under the `Collection/` folder:

- **Collection Name**: `MedicalAppt_Postman_Collection`

Import it into Postman and start testing immediately!

---

📡 API Endpoints Overview

Some of the available endpoints:

- **POST** `/login` – Authenticate a user and receive a JWT
- **GET** `/appointments` – List appointments
- **POST** `/appointments` – Create a new appointment
- **PATCH** `/appointments/{id}/cancellation` – Cancel an appointment
- **GET** `/doctors/{doctorId}/appointments` – View appointments for a specific doctor
- **GET** `/doctors/available?speciality=...&appointmentDate=...` – View doctors with availability

Full interactive documentation available at **`/swagger`**.

---

🧠 Author

Developed by **[lennarx]** – feedback and contributions welcome!

---
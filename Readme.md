# 🩺 Medical Appointments API (MVP)

This API serves as a **Minimum Viable Product** for a medical appointments system used by clinics or medical centers to manage appointments and doctors' availability.

While email notifications to patients and doctors are planned, **this feature is not yet implemented**.

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

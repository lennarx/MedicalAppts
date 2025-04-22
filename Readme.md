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

---

## 🐳 Docker & Setup

The app is fully containerized using **Docker** and orchestrated with **Docker Compose**.

### 🚀 To run the project:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/medical-appts-api.git
2. Open the solution in Visual Studio or VS Code.

3. Run the project using Docker (make sure Docker is running):

	From Visual Studio: just hit F5
							
	From CLI: bash docker compose up --build

🔐 Admin Credentials (Auto-Seeded)
The admin user is automatically created during the database seeding process (InitialSeed.cs):

Email: admin@medicalapp.com

Password: Admin123!

📡 API Endpoints Overview
Some of the available endpoints:

POST /login – Authenticate a user and receive a JWT

GET /appointments – List appointments

POST /appointments – Create a new appointment

PATCH /appointments/{id}/cancellation – Cancel an appointment

GET /doctors/{doctorId}/appointments – View appointments for a specific doctor

GET /doctors/available?speciality=...&appointmentDate=... – View doctors with availability

Full interactive documentation available at /swagger

🧩 How to Extend
Planned future improvements:

✅ Email notifications (SMTP)

✅ Role-based authorization (Admin / Doctor / Patient)

🔜 Doctor availability calendar view

🔜 Patient self-booking UI (external frontend or mobile app)

🔜 Azure deployment + monitoring

Feel free to open issues or contribute via pull requests!

🧠 Author
Developed by [lennarx] – feedback and contributions welcome!

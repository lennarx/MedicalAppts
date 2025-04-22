This API acts as an MVP with the purpose to emulate a Medical appointments app used by a medical center to manage appointments for their patients,
as well as showing doctors availability. It's planned to send notifications to patients and doctors via email but this feature is not implemented yet.

It uses docker to enable containerization and is orchestrated by docker-compose.

To run it, just clone the repo and run it from visual studio so the containers can be created and initialized.
The admin user is seeded in the InitialSeed class with the following credentials:
Username: admin@medicalapp.com
Password: Admin123!
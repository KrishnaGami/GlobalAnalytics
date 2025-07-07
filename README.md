# GlobalAnalytics
Overview
A modular, scalable backend system that enables data analysts and enterprise clients to:
- Run complex ad-hoc queries
- Export data in **JSON, CSV, PDF**
- Handle large data volumes with high performance
- Integrate into CI/CD pipelines with **Jenkins + Docker**


#Initial Database Setup
- Create the GlobalAnalytics database.
- Execute the script GlobalAnalyticsDbScript.sql (shared via Email) to set up the schema and initial dataset.


#How to Run the Application using Swagger
- Login First
	- Call the /api/Auth/login endpoint to authenticate.
	- It will return a JWT token upon successful login.

- Authorize Requests
	- Click on the "Authorize" button in Swagger UI.
	- Paste the JWT token in the authorization popup (usually as: Bearer <token>).

- Access APIs
	- Once authorized, you can call any secured API endpoint directly from Swagger.

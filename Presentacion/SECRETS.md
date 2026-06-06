WARNING: This file contains development credentials. Do NOT commit or share these credentials publicly. Remove or rotate them before sharing the repository.

## Development database credentials (created by setup)

- Database: ecefiad
- Host: localhost
- Port: 1433

Non-SA application user (recommended for local dev):
- Username: ece_app_user
- Password: E@0a&2SXhjrbx%uu

SA account used for container management (do NOT use in production):
- Username: sa
- Password: EceFiad@2026

Connection string examples

- Connection string using the application user (recommended for app):

  Server=localhost,1433;Database=ecefiad;User Id=ece_app_user;Password=E@0a&2SXhjrbx%uu;TrustServerCertificate=True;MultipleActiveResultSets=True

- Connection string using SA (for maintenance only):

  Server=localhost,1433;Database=ecefiad;User Id=sa;Password=EceFiad@2026;TrustServerCertificate=True;MultipleActiveResultSets=True

How to configure the application (choose one)

1) Use dotnet user-secrets (recommended for development)

```bash
cd Presentacion
# initialize (only once)
dotnet user-secrets init
# set the connection string in the secret store
dotnet user-secrets set "ConnectionStrings:ConexionBD" "Server=localhost,1433;Database=ecefiad;User Id=ece_app_user;Password=E@0a&2SXhjrbx%uu;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

2) Use an environment variable (for local shell or CI)

```bash
# zsh/bash
export ConnectionStrings__ConexionBD="Server=localhost,1433;Database=ecefiad;User Id=ece_app_user;Password=E@0a&2SXhjrbx%uu;TrustServerCertificate=True;MultipleActiveResultSets=True"
# then run the app
dotnet run --project Presentacion/Presentacion.csproj
```

Cleanup and security

- When you stop using these credentials, delete the user or rotate the password:

```bash
# to delete user/login (run via sqlcmd as SA)
-- drop user from database
USE ecefiad;
DROP USER IF EXISTS ece_app_user;
-- drop server login
DROP LOGIN IF EXISTS ece_app_user;
```

- Remove this file from the repo before publishing or add it to `.gitignore`.
- For production, use a managed secret store (Azure Key Vault, AWS Secrets Manager, or similar) and do not store plain passwords in config files.

If you want, I can:
- Remove this file after you confirm you've saved the password elsewhere.
- Create a `docker-compose.yml` that passes the connection string as an environment variable into the app service instead of storing it in the repo.


Default project in package manager console is `Ordering.Infrastructure` and startup project is `Ordering.API`
Add-Migration InitialCreate -OutputDir Data/Migrations -Project Ordering.Infrastructure -StartupProject Ordering.API

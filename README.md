 RaceDay - Part 3: MVC Web Application

A web front-end for the RaceDay system, built with ASP.NET Core MVC.
This is the user-facing website that connects to the Part 2 Web API.

How Part 3 Connects to Part 2

Part 2 and Part 3 are NOT two separate systems. They work together:

Part 2
- Project: `Raceday`
- Talks directly to the database `RaceDayDB`
- Has: Data, Models, Migrations, API Controllers
- Handles: Auth, Events, Categories, Enrolments, Results
- Returns JSON only, no pretty pages
- Runs at: `http://localhost:5120` - Swagger at `/swagger`

Part 3 
- Project: `RaceDay.Web`
- Has NO database of its own
- Has NO Data / Migrations / Models folder
- Has: Controllers, Views, ViewModels, Services, Filters, wwwroot
- When you click Login or View Events, it calls the Part 2 API

**Flow:**

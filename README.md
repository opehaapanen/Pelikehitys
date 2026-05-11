# Pelikehitys
PowerSchell - OHJE Minimal API Server harjoitukseen



\- Luo kansio C-asemalle (esim. DotNetSqlite)

\- aja komento dotnet new web

\- aja komento dotnet run



\*\* Jos tässä vaiheessa tulee käyttöesto virhe, niin suorita alla oleva komento \*\*

\- *Add-MpPreference -AttackSurfaceReductionOnlyExclusions "<fully qualified path or resource>"*

\*\* esim. Add-MpPreference -AttackSurfaceReductionOnlyExclusions "C:\\DotNet\_Sqlite\\bin\\Debug\\net10.0\\DotNet\_Sqlite.exe" \*\*



Voit lisätä SQLite-tietokannan Minimal API -projektiin käyttämällä Entity Framework Core + SQLite-provideria. Alla yksinkertainen tapa tehdä se vaiheittain.



\## 1. Asenna NuGet-paketit



Projektikansiossa:



```bash

dotnet add package Microsoft.EntityFrameworkCore.Sqlite

dotnet add package Microsoft.EntityFrameworkCore.Design

```



\---



\## 2. Luo DbContext



Lisää uusi tiedosto esimerkiksi `ScoreContext.cs`:



```csharp

using Microsoft.EntityFrameworkCore;



public class ScoreContext : DbContext

{

&#x20;   public ScoreContext(DbContextOptions<ScoreContext> options)

&#x20;       : base(options)

&#x20;   {

&#x20;   }



&#x20;   public DbSet<Score> Scores => Set<Score>();

}

```



\---



\## 3. Muuta Score-luokka Entity Frameworkille sopivaksi



Entity Framework tarvitsee yleensä `Id`-kentän.



```csharp

public class Score

{

&#x20;   public int Id { get; set; }



&#x20;   public string Name { get; set; } = "";



&#x20;   public int Points { get; set; }



&#x20;   public Score()

&#x20;   {

&#x20;   }



&#x20;   public Score(string name, int points)

&#x20;   {

&#x20;       Name = name;

&#x20;       Points = points;

&#x20;   }

}

```



\---



\## 4. Lisää SQLite yhteys Program.cs:ään



Korvaa nykyinen koodi tällä:



```csharp

using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ScoreContext>(options =>

&#x20;   options.UseSqlite("Data Source=scores.db"));



var app = builder.Build();



app.MapPost("/score", async (Score score, ScoreContext db) =>

{

&#x20;   db.Scores.Add(score);

&#x20;   await db.SaveChangesAsync();



&#x20;   return Results.Ok();

});



app.MapGet("/leaderboard", async (ScoreContext db) =>

{

&#x20;   return await db.Scores

&#x20;       .OrderByDescending(s => s.Points)

&#x20;       .Take(5)

&#x20;       .ToListAsync();

});



app.Run();

```



\---



\## 5. Luo tietokanta migrationeilla



Suorita:



```bash

dotnet ef migrations add InitialCreate

dotnet ef database update

```



Tämän jälkeen projektiin syntyy:



\* `scores.db` → SQLite-tietokanta

\* `Migrations`-kansio



\---



\## 6. Testaa API



POST:



```http

POST /score

Content-Type: application/json



{

&#x20; "name": "Matti",

&#x20; "points": 123

}

```



GET:



```http

GET /leaderboard

```



\---



\## Lopputulos



Nyt data:



\* säilyy ohjelman sammuttamisen jälkeen

\* tallentuu SQLite-tiedostoon

\* toimii oikeana tietokantana muistissa olevan `List<Score>`-listan sijaan



Jos haluat, voin myös näyttää:



\* miten lisätään automaattinen tietokannan luonti ilman migrationeja

\* miten käytetään `appsettings.json`

\* miten tehdään Docker-yhteensopiva SQLite-ratkaisu

\* miten lisätään Swagger/OpenAPI tähän Minimal API:in




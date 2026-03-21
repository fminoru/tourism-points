# Tourism Points (Pontos Turísticos)

Full-stack sample application for managing Brazilian tourist points: an **ASP.NET Core** REST API with **SQLite** and **Entity Framework Core**, plus a **React** (Vite + TypeScript) client.

---

## Table of contents

1. [What you need installed](#what-you-need-installed)
2. [Repository layout](#repository-layout)
3. [Clone the repository](#clone-the-repository)
4. [Backend: setup and run](#backend-setup-and-run)
5. [Frontend: setup and run](#frontend-setup-and-run)
6. [Using the application](#using-the-application)
7. [Configuration: ports and CORS](#configuration-ports-and-cors)
8. [Database and seed data](#database-and-seed-data)
9. [Production build (overview)](#production-build-overview)
10. [Troubleshooting](#troubleshooting)

---

## What you need installed

| Tool | Purpose | Notes |
|------|---------|--------|
| [.NET SDK](https://dotnet.microsoft.com/download) | Build and run the API | This project targets **.NET 10** (`net10.0`). Install the matching SDK (preview or stable, depending on your environment). Run `dotnet --version` to confirm. |
| [Node.js](https://nodejs.org/) | Build and run the React app | **20 LTS** or newer is recommended. Includes `npm`. Run `node --version` and `npm --version`. |
| **Git** | Clone the repository | Any recent version. |

You do **not** need a separate SQLite installation: the app uses the SQLite package bundled with Entity Framework Core.

---

## Repository layout

```
tourism-points/
├── TourismPoints.slnx          # Solution (XML format)
├── README.md                   # This file
├── .gitignore
└── src/
    ├── TourismPoints.API/      # ASP.NET Core Web API (entry point for the backend)
    ├── TourismPoints.Client/   # React + Vite frontend
    ├── TourismPoints.Domain/   # Entities and domain types
    └── TourismPoints.Infrastructure/  # EF Core DbContext, repositories, seed data
```

The API project references **Domain** and **Infrastructure**. The client is independent and talks to the API over HTTP.

---

## Clone the repository

Replace the URL with your own GitHub (or other) remote if different.

```bash
git clone https://github.com/YOUR_USERNAME/tourism-points.git
cd tourism-points
```

Confirm you are on the branch you expect (often `main`):

```bash
git branch
git status
```

---

## Backend: setup and run

All commands below assume your current directory is the repository root (`tourism-points/`).

### Restore dependencies

```bash
dotnet restore
```

You can restore a single project:

```bash
dotnet restore src/TourismPoints.API/TourismPoints.API.csproj
```

### Run the API

From the repository root:

```bash
dotnet run --project src/TourismPoints.API/TourismPoints.API.csproj
```

Or change into the API folder:

```bash
cd src/TourismPoints.API
dotnet run
```

**Expected output:** the host listens on **HTTP port `5151`** (see `Properties/launchSettings.json`, profile `http`).

Useful URLs (with the API running in **Development**):

| URL | Description |
|-----|-------------|
| [http://localhost:5151/](http://localhost:5151/) | Short HTML page with links to Swagger and the client |
| [http://localhost:5151/swagger](http://localhost:5151/swagger) | Swagger UI (OpenAPI) |
| [http://localhost:5151/api/touristpoints](http://localhost:5151/api/touristpoints) | List endpoint (supports `page`, `pageSize`, `search` query parameters) |

Stop the server with **Ctrl+C** in that terminal.

### Optional: HTTPS profile

The `https` launch profile also binds **https://localhost:7161** and **http://localhost:5151**. If you switch the client to HTTPS, you must update the frontend `API_URL` and CORS in the API accordingly (see [Configuration](#configuration-ports-and-cors)).

---

## Frontend: setup and run

Open a **second** terminal. The dev server must stay running while you use the browser.

### Install dependencies

```bash
cd src/TourismPoints.Client
npm install
```

### Start the development server

```bash
npm run dev
```

The Vite config pins the dev server to **port `5175`** (`strictPort: true`) so it does not collide with other tools that use `5173`.

**Open in the browser:** [http://localhost:5175/](http://localhost:5175/)

### Optional: listen on all interfaces

Useful for testing from another device on your network:

```bash
npm run dev:host
```

If you use a host or port different from `localhost:5175`, update the API **CORS** policy in `TourismPoints.API/Program.cs` so the browser is allowed to call the API.

---

## Using the application

1. Start the **API** (`dotnet run` in `TourismPoints.API`) and leave it running.
2. Start the **client** (`npm run dev` in `TourismPoints.Client`) and leave it running.
3. Browse to **http://localhost:5175/**.

From the UI you can search, list, view details, create, edit, and delete tourist points. All of that goes through the REST API under `/api/touristpoints`.

---

## Configuration: ports and CORS

These pieces must stay aligned for local development:

| Piece | File | Default |
|-------|------|---------|
| API HTTP port | `src/TourismPoints.API/Properties/launchSettings.json` | `5151` |
| Frontend API base URL | `src/TourismPoints.Client/src/services/api.ts` | `http://localhost:5151/api` |
| CORS allowed origin | `src/TourismPoints.API/Program.cs` | `http://localhost:5175` |
| Vite dev port | `src/TourismPoints.Client/vite.config.ts` | `5175` |

If you change the API port, update **`api.ts`** and any bookmarks. If you change the Vite port, update **`Program.cs`** CORS `WithOrigins(...)`.

---

## Database and seed data

- **Engine:** SQLite  
- **File:** `tourism.db` is created in the API project directory (`src/TourismPoints.API/`) on first run (when the app starts and `EnsureCreated` runs).

The file is listed in **`.gitignore`** (`*.db`, etc.) so each machine keeps its own database.

### Seed data

On startup, if the `TouristPoints` table is **empty**, the app inserts **20** sample Brazilian tourist points (see `TourismPoints.Infrastructure/Data/TourismDbSeeder.cs`).

To **reload** the sample data from scratch:

1. Stop the API.
2. Delete `src/TourismPoints.API/tourism.db` (and `-shm` / `-wal` if present).
3. Start the API again.

---

## Production build (overview)

### API

Publish to a folder or container as you normally would for ASP.NET Core, for example:

```bash
dotnet publish src/TourismPoints.API/TourismPoints.API.csproj -c Release -o ./publish
```

Configure `ASPNETCORE_ENVIRONMENT`, URLs, and a production connection string or database path as appropriate. Adjust CORS to your real frontend origin.

### Client

```bash
cd src/TourismPoints.Client
npm run build
```

Static output is emitted under `dist/`. Serve those files with your web server or CDN, and set `API_URL` (e.g. via build-time env variables) to your production API base URL.

---

## Troubleshooting

### Browser shows CORS errors

The API must allow the exact origin of the page (scheme + host + port). For local dev that is `http://localhost:5175`. If you changed the Vite port or use `127.0.0.1` instead of `localhost`, update `Program.cs` CORS to match.

### Client cannot reach the API (`Network Error`, failed fetch)

- Confirm the API is running and listening on **5151**.
- Confirm `API_URL` in `src/services/api.ts` matches that host and port.
- Try opening `http://localhost:5151/api/touristpoints` directly in the browser.

### Port already in use

- **5151:** Another process is using it. Stop that process or change `applicationUrl` in `launchSettings.json` and update the client `API_URL`.
- **5175:** Vite uses `strictPort: true`, so it will exit if the port is busy. Free the port or change `server.port` in `vite.config.ts` and update CORS.

### `dotnet` or `npm` command not found

Install the [.NET SDK](https://dotnet.microsoft.com/download) and [Node.js](https://nodejs.org/), then open a new terminal so `PATH` is refreshed.

### Empty list after expecting seed data

The seeder only runs when the table has **no rows**. If you already had data, it will not add duplicates. Delete `tourism.db` and restart the API to force a fresh database and seed (see [Database and seed data](#database-and-seed-data)).

---

## License

Unless the repository owner specifies otherwise, treat licensing as defined in this repo’s `LICENSE` file (add one if you need a formal license).

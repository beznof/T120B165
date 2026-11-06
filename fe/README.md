# T120B165 frontend

Run these commands from the `fe` directory with Node.js 22.12+ (22.x) or 24+ and npm.

```sh
npm ci
npm run dev
```

The starter route renders a blank page. To use the API from frontend code, start the configured ASP.NET API from the repository root with `dotnet run --project be/Languages.WebApi --launch-profile http`. Vite forwards `/api` requests to port 5000. The session module stores tokens in memory and clears cached queries when the session changes.

In production, forward `/api` to the backend or set `VITE_API_BASE_URL` to its API base (including `/api`) at build time and allow the frontend origin on the API. Configure the static host to serve index.html for frontend routes.

After backend contract changes, run `npm run api:types` against the local development API and commit the generated types. Run `npm test` for the session regression tests.

Add routes under `src/routes`. Vite generates `src/routeTree.gen.ts` during development and production builds. Keep the generated file in Git so type checking works on a fresh checkout.

The router context and React Query provider share one query client, so route loaders and components can use the same cache. Tailwind CSS and HeroUI styles are loaded in `src/styles.css`.

Run `npm run check` for lint, formatting, and TypeScript checks, or `npm run format` to apply formatting. Run `npm run build` to produce `dist/`, then `npm run preview` to serve the production build locally.

# T120B165 frontend

Run these commands from the `fe` directory with Node.js 22.12+ (22.x) or 24+ and npm.

```sh
npm ci
npm run dev
```

The home route intentionally renders an empty page. Add route files under `src/routes`. Vite generates `src/routeTree.gen.ts` during development and production builds. Keep the generated file in Git so type checking works on a fresh checkout.

The router context and React Query provider share one query client, so route loaders and components can use the same cache. Tailwind CSS and HeroUI styles are loaded in `src/styles.css`.

Run `npm run check` for lint, formatting, and TypeScript checks, or `npm run format` to apply formatting. Run `npm run build` to produce `dist/`, then `npm run preview` to serve the production build locally.

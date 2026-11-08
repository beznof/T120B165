# T120B165 frontend

Run these commands from the `fe` directory with Node.js 22.12+ (22.x) or 24+ and npm.

```sh
npm ci
npm run dev
```

The `/` route opens the languages catalog. Use `/register` to create a learner or teacher account and `/login` to sign in. Choose a language and dictionary from the cover grids to open vocabulary review. Select an entry or use Previous and Next to review entries across pages. Search, pagination, and word or phrase filters use the API.

Admins can manage languages, dictionaries, and entries. Teachers can create dictionaries and manage their own dictionaries and entries. The API enforces these permissions. The session module stores tokens in memory and clears cached queries when the session changes.

The browser calls `/api` on the frontend origin. Vite forwards those requests to `https://api.saitynai.vilvis.lt` during development and local preview. To use a local backend, set `API_UPSTREAM=http://localhost:5000` in `fe/.env.local` and start the API from the repository root with `dotnet run --project be/Languages.WebApi --launch-profile http`.

Set `API_UPSTREAM=https://api.saitynai.vilvis.lt` in the frontend Container App’s environment variables before running the frontend deployment. Use the origin alone, without a path or trailing slash. The Nginx image expands `nginx.conf.template` at startup and forwards `/api` requests to that origin. The deployment workflow validates the config against the Azure setting before updating the image. To change the origin later, create a new revision with the new value and reuse the same image.

After backend contract changes, run `npm run api:types` against the local development API and commit the generated types.

Add routes under `src/routes`. Vite generates `src/routeTree.gen.ts` during development and production builds. Keep the generated file in Git so type checking works on a fresh checkout.

The router context and React Query provider share one query client, so route loaders and components can use the same cache. Tailwind CSS and HeroUI styles are loaded in `src/styles.css`.

Run `npm run check` for lint, formatting, and TypeScript checks, or `npm run format` to apply formatting. Run `npm run build` to produce `dist/`, then `npm run preview` to serve the production build locally.

Render Lucide glyphs through `src/ui/Icon.tsx`, for example `<Icon icon={ArrowRight} size={18} />`. It defaults to 20px and a 1.75 stroke and accepts Lucide SVG props. Label icon-only buttons on the button itself.

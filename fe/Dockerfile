ARG APP_DIR="/app"

# Build
FROM node:24-bookworm-slim AS build

ARG APP_DIR

WORKDIR "${APP_DIR}"

COPY "package.json" "package-lock.json" ./
RUN npm ci

COPY . .
RUN npm run build


# Run
FROM nginx:stable-alpine

ARG APP_DIR

COPY "nginx.conf" "/etc/nginx/conf.d/default.conf"
COPY --from=build "${APP_DIR}/dist/" "/usr/share/nginx/html/"

EXPOSE 80

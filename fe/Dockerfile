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

ENV NGINX_ENVSUBST_FILTER="^API_UPSTREAM$"

COPY "nginx.conf.template" "/etc/nginx/templates/default.conf.template"
COPY --from=build "${APP_DIR}/dist/" "/usr/share/nginx/html/"

EXPOSE 80

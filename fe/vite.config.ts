import { cwd } from 'node:process'
import tailwindcss from '@tailwindcss/vite'
import { tanstackRouter } from '@tanstack/router-plugin/vite'
import react from '@vitejs/plugin-react'
import { defineConfig, loadEnv } from 'vite'

export default defineConfig(({ mode }) => {
	const { API_PROXY_TARGET } = loadEnv(mode, cwd(), 'API_PROXY_')

	return {
		server: {
			proxy: {
				'/api': {
					target: API_PROXY_TARGET || 'https://api.saitynai.vilvis.lt',
					changeOrigin: true,
					proxyTimeout: 10_000,
				},
			},
		},
		plugins: [
			tanstackRouter({ target: 'react', autoCodeSplitting: true }),
			react(),
			tailwindcss(),
		],
	}
})

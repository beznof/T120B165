import { cwd } from 'node:process'
import tailwindcss from '@tailwindcss/vite'
import { tanstackRouter } from '@tanstack/router-plugin/vite'
import react from '@vitejs/plugin-react'
import { defineConfig, loadEnv } from 'vite'

export default defineConfig(({ mode }) => {
	const { API_UPSTREAM } = loadEnv(mode, cwd(), 'API_UPSTREAM')

	return {
		plugins: [
			tanstackRouter({ target: 'react', autoCodeSplitting: true }),
			react(),
			tailwindcss(),
		],
		server: {
			proxy: {
				'^/api(?:/|\\?|$)': {
					target: API_UPSTREAM || 'https://api.saitynai.vilvis.lt',
					changeOrigin: true,
				},
			},
		},
	}
})

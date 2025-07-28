import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Determine if we're building for GitHub Pages
const isGitHubPages = process.env.GITHUB_PAGES === 'true'

// https://vite.dev/config/
export default defineConfig({
	base: isGitHubPages ? '/WebPlayground/' : '/',
	plugins: [react()],
})

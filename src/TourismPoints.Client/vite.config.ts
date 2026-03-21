import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // Use a project-specific port so this app is not confused with other Vite apps
    // that often default to 5173 (browser cache / service workers are per origin).
    port: 5175,
    strictPort: true,
    headers: {
      'Cache-Control': 'no-store',
    },
  },
})

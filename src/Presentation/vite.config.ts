import { reactRouter } from "@react-router/dev/vite";
import tailwindcss from "@tailwindcss/vite";
import { defineConfig } from "vite";
import tsconfigPaths from "vite-tsconfig-paths";

export default defineConfig({
  plugins: [
    tailwindcss(), 
    reactRouter(), 
    tsconfigPaths()
  ],
  publicDir: 'assets',
  build: {
    target: 'esnext',
    sourcemap: true,
    chunkSizeWarningLimit: 2000
  },
  server: {
    port: 3000,
    host: '0.0.0.0',
    proxy: {
      '/api': {
        target: 'https://localhost:5000',
        secure: false,
        changeOrigin: true
      }
    },
    hmr: {
      port: 3000
    }
  },
  optimizeDeps: {
    include: ['react-router'],
  }
});

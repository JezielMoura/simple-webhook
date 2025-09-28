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
    https: {
      pfx: '/Users/moura/.aspnet/dev-certs/https/aspnetcore-localhost-ACDC6905A0EF96AC0F949CDBD2641927E8148502.pfx'
    },
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

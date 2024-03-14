import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// TODO: Need to fix this proxy to avoid cords issues
// For now I'm having to use a cors unblock browser plugin

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    cors: {
      origin: "http://localhost:5264/",
    },
    proxy: {
      "/lotteryapi": {
        target: "http://localhost:5264/",
        changeOrigin: false,
        secure: false,
        rewrite: (path) => path.replace(/^\/lotteryapi/, ""),
        configure: (proxy, _options) => {
          proxy.on("error", (err, _req, _res) => {
            console.log("Proxy error", err);
          });
          proxy.on("proxyReq", (proxyReq, req, _res) => {
            const { method, protocol, host, path, ...x } = proxyReq;
            const headers = proxyReq.getHeaders();
            const data = { method, protocol, host, path, headers };
            console.log("Proxy request:", data);
          });
          proxy.on("proxyRes", (proxyRes, req, _res) => {
            console.log("Proxy response:", {
              status: proxyRes.statusCode,
              url: req.url,
              headers: proxyRes.headers,
            });
          });
        },
      },
    },
  },
});

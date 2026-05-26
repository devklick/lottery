import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// TODO: Need to fix this proxy to avoid cords issues
// For now I'm having to use a cors unblock browser plugin

// TODO: Make the API path an env var. At the moment it's hardcoded to the URL/port
// for running in docker, but we may want to run the UI + API outside of docker
// for HMR, attaching debugger etc

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    cors: {
      origin: "http://localhost:5000/",
    },
    proxy: {
      "/lotteryapi": {
        target: "http://localhost:5000/",
        changeOrigin: false,
        secure: false,
        rewrite: (path) => path.replace(/^\/lotteryapi/, ""),
        configure: (proxy, _options) => {
          proxy.on("error", (err, _req, _res) => {
            console.log("Proxy error", err);
          });
          proxy.on("proxyReq", (proxyReq, _req, _res) => {
            const { method, protocol, host, path, ..._x } = proxyReq;
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

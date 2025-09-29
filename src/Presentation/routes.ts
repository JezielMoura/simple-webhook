import { type RouteConfig, index, layout, route } from "@react-router/dev/routes";

export default [
  index("./pages/home.tsx"),
  route("webhook/:id", "./pages/webhook.tsx"),
] satisfies RouteConfig;

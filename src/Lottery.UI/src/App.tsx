import { MantineProvider } from "@mantine/core";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Notifications } from "@mantine/notifications";

import routes from "./Routing/routes.tsx";
import theme from "./theme.ts";

import "./App.css";

const router = createBrowserRouter(routes);

const queryClient = new QueryClient();

function App() {
  return (
    <MantineProvider defaultColorScheme="auto" theme={theme}>
      <Notifications />
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router}></RouterProvider>
      </QueryClientProvider>
    </MantineProvider>
  );
}

export default App;

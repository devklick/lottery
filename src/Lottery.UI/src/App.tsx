import { MantineProvider } from "@mantine/core";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import routes from "./Routing/routes.tsx";

import "./App.css";

const router = createBrowserRouter(routes);

const queryClient = new QueryClient();

function App() {
  return (
    <MantineProvider defaultColorScheme="auto">
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router}></RouterProvider>
      </QueryClientProvider>
    </MantineProvider>
  );
}

export default App;

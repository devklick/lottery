import {
  localStorageColorSchemeManager,
  MantineColorScheme,
  MantineProvider,
} from "@mantine/core";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Notifications } from "@mantine/notifications";
import { useState } from "react";

import routes from "./Routing/routes.tsx";
import { useCreateTheme } from "./theme.ts";

import "./App.css";

const router = createBrowserRouter(routes);

const queryClient = new QueryClient();

function App() {
  const [colorScheme, _setColorScheme] = useState<MantineColorScheme>("auto");
  const theme = useCreateTheme(colorScheme);
  const colorSchemeManager = localStorageColorSchemeManager({
    key: "color-scheme",
  });

  const setColorScheme = (colorScheme: MantineColorScheme) => {
    _setColorScheme(colorScheme);
    colorSchemeManager.set(colorScheme);
  };

  return (
    <MantineProvider
      theme={theme}
      colorSchemeManager={{
        ...colorSchemeManager,
        get: () => colorScheme,
        set: setColorScheme,
      }}
    >
      <Notifications />
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router}></RouterProvider>
      </QueryClientProvider>
    </MantineProvider>
  );
}

export default App;

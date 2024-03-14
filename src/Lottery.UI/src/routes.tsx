import { RouteObject } from "react-router-dom";
import App from "./App.tsx";
import pages from "./pages/index.ts";

const routes: Array<RouteObject> = [
  {
    path: "/",
    element: <App />,
    children: [
      {
        index: true,
        element: <pages.Home.Page />,
      },
      {
        path: "/home",
        element: <pages.Home.Page />,
      },
      {
        path: "/account",
        element: <pages.Account.Page />,
      },
      {
        path: "/account/signIn",
        element: <pages.Account.SignIn.Page />,
      },
      {
        path: "/account/signUp",
        element: <pages.Account.SignUp.Page />,
      },
      {
        path: "/games",
        element: <pages.Games.Page />,
      },
    ],
  },
];

export default routes;

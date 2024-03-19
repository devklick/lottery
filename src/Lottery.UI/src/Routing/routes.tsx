import { RouteObject } from "react-router-dom";
import App from "../App.tsx";
import pages from "../pages.ts";
import ProtectedRoute from "./ProtectedRoute.tsx";
import Unauthorized from "../Account/Unauthorized/Unauthorized.tsx";

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
      {
        path: "/games/create",
        element: (
          <ProtectedRoute
            permittedUserTypes={["Admin"]}
            notPermittedErrorMessage="You do not have permission to create games"
          >
            <pages.Games.CreateGame.Page />
          </ProtectedRoute>
        ),
      },
      {
        path: "/games/:id",
        element: <pages.Games.GameDetail.Page />,
      },
      {
        path: "account/unauthorized",
        element: <Unauthorized />,
      },
    ],
  },
];

export default routes;

import { RouteObject } from "react-router-dom";

import pages from "../Pages";
import ProtectedRoute from "./ProtectedRoute.tsx";
import Layout from "../Layout/Layout.tsx";

const routes: Array<RouteObject> = [
  {
    path: "/",
    element: <Layout />,
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
        path: "/games/:id/edit",
        element: (
          <ProtectedRoute
            permittedUserTypes={["Admin"]}
            notPermittedErrorMessage="You do not have permission to edit games"
          >
            <pages.Games.EditGame.Page />
          </ProtectedRoute>
        ),
      },
      {
        path: "/users",
        element: (
          <ProtectedRoute
            permittedUserTypes={["Admin"]}
            notPermittedErrorMessage="You do not have permission to view users"
          >
            <pages.Users.Page />
          </ProtectedRoute>
        ),
      },
      {
        path: "/users/invite",
        element: (
          <ProtectedRoute
            permittedUserTypes={["Admin"]}
            notPermittedErrorMessage="You do not have permission to invite users"
          >
            <pages.Users.Invite.Page />
          </ProtectedRoute>
        ),
      },
      {
        path: "/users/invite/accept",
        element: <pages.Users.Invite.Accept.Page />,
      },
      {
        path: "account/unauthorized",
        element: <pages.Account.Unauthorized />,
      },
    ],
  },
];

export default routes;
